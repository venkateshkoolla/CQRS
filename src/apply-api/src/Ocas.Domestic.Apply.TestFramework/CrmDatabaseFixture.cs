using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bogus;
using Dapper;
using Ocas.Domestic.Apply.TestFramework.RuleCollections;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.TestFramework
{
    public class CrmDatabaseFixture
    {
        private readonly Faker _faker;
        private readonly ModelFakerFixture _modelFakerFixture;

        public CrmDatabaseFixture(ModelFakerFixture modelFakerFixture)
        {
            _faker = new Faker();
            _modelFakerFixture = modelFakerFixture;
        }

        public async Task<string> CreateApplicationNumber(string year)
        {
            const int maxAttempts = 15;
            var attempts = 0;
            var count = int.MaxValue;
            var number = string.Empty;

            // make sure the number we generated is unique
            while (count > 0 || attempts > maxAttempts)
            {
                number = _faker.GenerateApplicationNumber(year);
                var sql = $"SELECT COUNT(*) FROM [dbo].[ocaslr_applicationBase] WHERE [ocaslr_applicationnumber] = '{number}'";
                using (var conn = new SqlConnection(CrmConstants.ConnectionString))
                {
                    conn.Open();

                    count = await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
                }
                attempts++;
            }

            return number;
        }

        public async Task<string> CreateVoucher(Guid collegeId, Guid applicationCycleId)
        {
            var voucher = new Dto.Voucher();
            var voucherCount = int.MaxValue;
            string sql;

            // make sure the code we generated is unique
            while (voucherCount > 0)
            {
                voucher = _modelFakerFixture.GetVoucher().Generate();
                sql = $"SELECT COUNT(*) FROM [dbo].[ocaslr_voucherBase] WHERE [ocaslr_name] = '{voucher.Code}'";
                using (var conn = new SqlConnection(CrmConstants.ConnectionString))
                {
                    conn.Open();

                    voucherCount = await conn.ExecuteScalarAsync<int>(sql, commandType: CommandType.Text);
                }
            }

            var parameters = new Dictionary<string, object>
                {
                    { "VoucherCode", voucher.Code },
                    { "CollegeId", collegeId },
                    { "ApplicationCycleId", applicationCycleId }
                };
            sql = await ReadAsync("Ocas.Domestic.Apply.TestFramework.Resources.CreateVoucher.sql");

            using (var conn = new SqlConnection(CrmConstants.ConnectionString))
            {
                conn.Open();

                await conn.ExecuteAsync(sql, parameters, commandType: CommandType.Text);
            }

            return voucher.Code;
        }

        public async Task<OcFileProcessHeader> GetOcFileProcessHeader(string collegeCode)
        {
            OcFileProcessHeader originalFile;
            using (var conn = new SqlConnection(CrmConstants.ConnectionString))
            {
                conn.Open();

                var parameters = new Dictionary<string, object>
                    {
                        { "CollegeCode", collegeCode }
                    };

                const string sql = "SELECT TOP (1) ocaslr_offerfilename AS [FileName], ocaslr_filenumber AS [FileNumber] FROM dbo.ocaslr_ocfileprocessheaderBase WHERE ocaslr_collegecode = @CollegeCode AND ocaslr_filefailed = 0 ORDER BY CreatedOn DESC";

                originalFile = await conn.QueryFirstOrDefaultAsync<OcFileProcessHeader>(sql, parameters, commandType: CommandType.Text);

                if ((originalFile?.FileName ?? originalFile?.FileNumber) is null)
                {
                    throw new Exception($"Failed to retrieve ocaslr_ocfileprocessheaderBase for college: {collegeCode}");
                }

                return originalFile;
            }
        }

        public async Task<(Guid?, IList<string>)> GetSpecialCodes(Guid collegeId)
        {
            var collegeAppCycleIds = _modelFakerFixture.AllApplyLookups.CollegeApplicationCycles.Where(a => a.CollegeId == collegeId).Select(c => c.Id);
            const string sql = "SELECT ocaslr_code FROM [dbo].[ocaslr_programspecialcodeBase] WHERE statecode = 0 AND ocaslr_collegeapplicatiocycleid = @CollegeApplicationId";

            foreach (var collegeAppCycleId in collegeAppCycleIds)
            {
                using (var conn = new SqlConnection(CrmConstants.ConnectionString))
                {
                    conn.Open();

                    var results = await conn.QueryAsync<string>(sql, new { CollegeApplicationId = collegeAppCycleId }, commandType: CommandType.Text);
                    if (results.Any()) return (collegeAppCycleId, results.AsList());
                }
            }

            return (null, new List<string>());
        }

        public Task<Guid> GetTranscriptByApplicantId(Guid applicantId)
        {
            var sql = $"SELECT [ocaslr_transcriptId] FROM [dbo].[ocaslr_transcriptBase] WHERE [ocaslr_contactid] = '{applicantId}'";
            using (var conn = new SqlConnection(CrmConstants.ConnectionString))
            {
                conn.Open();
                return conn.QueryFirstOrDefaultAsync<Guid>(sql, commandType: CommandType.Text);
            }
        }

        public async Task<string> GetTranscriptRequestReferenceNumber(string applicationNumber)
        {
            const string sql = "SELECT TOP 1 [Ocaslr_ReferenceNumber] FROM [dbo].[Ocaslr_etmstranscriptrequestBase] WHERE [Ocaslr_ApplicationNumber] = @ApplicationNumber";

            var parameters = new Dictionary<string, object>
            {
                { "@ApplicationNumber", applicationNumber }
            };

            using (var conn = new SqlConnection(CrmConstants.ConnectionString))
            {
                conn.Open();

                return await conn.ExecuteScalarAsync<string>(sql, parameters, commandType: CommandType.Text);
            }
        }

        public async Task UpdateApplicantBalance(Guid applicantId, decimal? adjustmentNeeded)
        {
            using (var conn = new SqlConnection(CrmConstants.ConnectionString))
            {
                conn.Open();

                var parameters = new Dictionary<string, object>
                {
                    { "ContactId", applicantId },
                    { "Balance", adjustmentNeeded * -1.0M }
                };

                const string sql = "UPDATE dbo.ContactBase SET ocaslr_balance = @Balance WHERE ContactId = @ContactId";

                await conn.ExecuteAsync(sql, parameters, commandType: CommandType.Text);
            }
        }

        public async Task UpdateTranscriptRequestStatus(Guid transcriptRequestId, Guid transcriptRequestStatusId)
        {
            using (var conn = new SqlConnection(CrmConstants.ConnectionString))
            {
                conn.Open();
                var sql = "UPDATE ocaslr_transcriptrequestBase "
                          + $"SET ocaslr_transcriptrequeststatusid = '{transcriptRequestStatusId}' "
                          + $"WHERE ocaslr_transcriptrequestId = '{transcriptRequestId}'";

                await conn.ExecuteAsync(sql, commandType: CommandType.Text);
            }
        }

        private async Task<string> ReadAsync(string resourceName, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public class OcFileProcessHeader
        {
            public string FileName { get; set; }
            public string FileNumber { get; set; }
        }
    }
}
