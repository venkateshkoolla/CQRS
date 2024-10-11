using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Dapper;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework.Models;

namespace Ocas.Domestic.Apply.TestFramework
{
    public class AlgoliaFixture
    {
        private static readonly SemaphoreSlim _syncProgramLock = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _syncCollegeLock = new SemaphoreSlim(1, 1);
        private static readonly Dictionary<string, List<AlgoliaProgramIntake>> _programIntakes = new Dictionary<string, List<AlgoliaProgramIntake>>();
        private static readonly Dictionary<Guid, List<string>> _colleges = new Dictionary<Guid, List<string>>();
        private static readonly string _programIntakeSql = Read("Ocas.Domestic.Apply.TestFramework.Resources.GetAlgoliaProgramIntakes.sql");
        private static readonly string _collegeSql = Read("Ocas.Domestic.Apply.TestFramework.Resources.GetAlgoliaColleges.sql");

        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Faker _fakerFixture;
        private readonly List<Guid> _restrictedColleges;

        public AlgoliaFixture(ModelFakerFixture modelFakerFixture)
        {
            _modelFakerFixture = modelFakerFixture;
            _fakerFixture = new Faker();

            _restrictedColleges = new List<Guid>
            {
                _modelFakerFixture.AllApplyLookups.Colleges.Single(x => x.Code == "ALGO").Id, // ALGO has garbage program data
                _modelFakerFixture.AllApplyLookups.Colleges.Single(x => x.Code == "HUMB").Id, // HUMB uses XML format which we can't generate
                _modelFakerFixture.AllApplyLookups.Colleges.Single(x => x.Code == "KEMP").Id, // this college is open but has no programs
                _modelFakerFixture.AllApplyLookups.Colleges.Single(x => x.Code == "SENE").Id, // this college is open but has no programs
                _modelFakerFixture.AllApplyLookups.Colleges.Single(x => x.Code == "CONF").Id, // the offer file process for this college was never used
                _modelFakerFixture.AllApplyLookups.Colleges.Single(x => x.Code == "GBTC").Id // the batch file info for this college is corrupted
            };
        }

        public async Task<List<AlgoliaProgramIntake>> GetProgramOfferings(Guid applicationCycleId, int numIntakes = 1, IList<Guid> excludeColleges = null)
        {
            // see which colleges actually have any open intakes for this cycle
            var algoliaColleges = await GetColleges(applicationCycleId);

            // still remove colleges that have data problems or have exceeded max choices already
            var restrictedColleges = new List<Guid>(_restrictedColleges);

            if (excludeColleges != null)
                restrictedColleges.AddRange(excludeColleges);

            var actualCollegeOfferings = new List<AlgoliaProgramIntake>();
            for (var i = 0; i < numIntakes; i++)
            {
                var availableColleges = _modelFakerFixture.AllApplyLookups.Colleges
                    .Where(x => x.IsOpen && !restrictedColleges.Contains(x.Id) && algoliaColleges.Contains(x.Code))
                    .ToList();
                var college = _fakerFixture.PickRandom(availableColleges);
                var actualCollegeOffering = await GetCollegeOffering(applicationCycleId, college.Id, currentChoices: actualCollegeOfferings);

                actualCollegeOfferings.Add(actualCollegeOffering);

                // can't choose more than 3 from the same college
                if (actualCollegeOfferings.Count(x => x.CollegeCode == college.Code) >= 3)
                {
                    restrictedColleges.Add(college.Id);
                }
            }

            return actualCollegeOfferings;
        }

        public async Task<AlgoliaProgramIntake> GetCollegeOffering(Guid applicationCycleId, Guid collegeId, IList<AlgoliaProgramIntake> currentChoices)
        {
            AlgoliaProgramIntake actualCollegeOffering = null;
            var attempts = 0;
            while (actualCollegeOffering is null)
            {
                var actualCollegeOfferings = await GetProgramIntakes(applicationCycleId, collegeId);
                var potentialCollegeOfferings = actualCollegeOfferings.Where(x => currentChoices.All(y => y.IntakeId != x.IntakeId)).ToList();
                actualCollegeOffering = _fakerFixture.PickRandom(potentialCollegeOfferings);

                attempts++;
                if (attempts >= 10)
                {
                    throw new Exception($"Exclude this college from tests as it has no available programs: {collegeId}");
                }
            }

            return actualCollegeOffering;
        }

        public async Task<List<AlgoliaProgramIntake>> GetProgramIntakes(Guid applicationCycleId, Guid collegeId)
        {
            var key = collegeId.ToString() + applicationCycleId.ToString();

            await _syncProgramLock.WaitAsync();
            try
            {
                // memoize to improve performance
                if (!_programIntakes.ContainsKey(key))
                {
                    using (var conn = new SqlConnection(CrmConstants.ConnectionString))
                    {
                        conn.Open();

                        var parameters = new Dictionary<string, object>
                        {
                            { "CollegeId", collegeId },
                            { "ApplicationCycleId", applicationCycleId },
                            { "IntakeCanApply", true }
                        };

                        var intakeDictionary = new Dictionary<Guid, AlgoliaProgramIntake>();

                        conn.Query<AlgoliaProgramIntake, Guid?, AlgoliaProgramIntake>(
                            _programIntakeSql,
                            (intake, entryLevelId) =>
                            {
                                if (!entryLevelId.HasValue) return null;

                                if (!intakeDictionary.TryGetValue(intake.IntakeId, out var intakeEntry))
                                {
                                    intakeEntry = intake;
                                    intakeEntry.ProgramValidEntryLevelIds = new List<Guid>();
                                    intakeDictionary.Add(intakeEntry.IntakeId, intakeEntry);
                                }

                                if (_modelFakerFixture.AllApplyLookups.EntryLevels.FindIndex(x => x.Id == entryLevelId.Value) >= _modelFakerFixture.AllApplyLookups.EntryLevels.FindIndex(x => x.Id == intakeEntry.ProgramEntryLevelId))
                                {
                                    // fixing bad data:
                                    // if the "valid entry level" is below the default entry level, then don't add it to the list of valid entry levels
                                    intakeEntry.ProgramValidEntryLevelIds.Add(entryLevelId.Value);
                                }

                                return intakeEntry;
                            },
                            parameters,
                            commandType: CommandType.Text,
                            commandTimeout: 300,
                            splitOn: "EntryLevelId");

                        _programIntakes.Add(key, intakeDictionary.Values.ToList());
                    }
                }
            }
            finally
            {
                _syncProgramLock.Release();
            }

            return _programIntakes[key];
        }

        public async Task<List<string>> GetColleges(Guid applicationCycleId)
        {
            var key = applicationCycleId;

            await _syncCollegeLock.WaitAsync();
            try
            {
                // memoize to improve performance
                if (!_colleges.ContainsKey(key))
                {
                    using (var conn = new SqlConnection(CrmConstants.ConnectionString))
                    {
                        conn.Open();

                        var parameters = new Dictionary<string, object>
                        {
                            { "ApplicationCycleId", applicationCycleId },
                            { "IntakeCanApply", true }
                        };

                        var hits = conn.Query<AlgoliaCollege>(_collegeSql, parameters, commandTimeout: 300, commandType: CommandType.Text);

                        // only use colleges with at least 10 open intakes
                        _colleges.Add(key, hits.Where(x => x.Count > 10).Select(x => x.CollegeCode).ToList());
                    }
                }

                return _colleges[key];
            }
            finally
            {
                _syncCollegeLock.Release();
            }
        }

        private static string Read(string resourceName, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private class AlgoliaCollege
        {
            public string CollegeCode { get; set; }
            public int Count { get; set; }
        }
    }
}
