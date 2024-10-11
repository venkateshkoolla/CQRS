using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Models.Templates
{
    public class StandardizedTestViewModel
    {
        public string NormingGroup { get; set; }
        public string ApplicationCycle { get; set; }
        public string TestType { get; set; }
        public string TestTypeCode { get; set; }
        public string Country { get; set; }
        public string ProvinceState { get; set; }
        public string City { get; set; }
        public string DateTestTaken { get; set; }
        public string TestDescription { get; set; }

        public IList<StandardizedTestDetail> Details { get; set; }
        public StandardizedTestLabels Labels { get; set; }

        public void LoadTranslations(TranslationsDictionary translationsDictionary)
        {
            Labels = new StandardizedTestLabels
            {
                TestInformation = translationsDictionary.Get("transcript.standardized_test.test_information"),
                TestType = translationsDictionary.Get("transcript.standardized_test.test_type"),
                Country = translationsDictionary.Get("transcript.standardized_test.country"),
                ProvinceState = translationsDictionary.Get("transcript.standardized_test.province_state"),
                City = translationsDictionary.Get("transcript.standardized_test.city"),
                DateTestTaken = translationsDictionary.Get("transcript.standardized_test.date_test_taken"),
                TestDescription = translationsDictionary.Get("transcript.standardized_test.test_description"),
                GeneralInformation = translationsDictionary.Get("transcript.standardized_test.general_information"),
                NormingGroup = translationsDictionary.Get("transcript.standardized_test.norming_group"),
                Official = translationsDictionary.Get("transcript.standardized_test.official"),
                OfficialYes = translationsDictionary.Get("transcript.standardized_test.official_yes"),
                ApplicationCycle = translationsDictionary.Get("transcript.standardized_test.application_cycle"),
                TestDetails = translationsDictionary.Get("transcript.standardized_test.test_details"),
                Tests = translationsDictionary.Get("transcript.standardized_test.tests"),
                RawScores = translationsDictionary.Get("transcript.standardized_test.raw_scores"),
                Percentile = translationsDictionary.Get("transcript.standardized_test.percentile")
            };
        }
    }

    public class StandardizedTestDetail
    {
        public string Description { get; set; }
        public string RawScore { get; set; }
        public string Percentile { get; set; }
    }

    public class StandardizedTestLabels
    {
        public string TestInformation { get; set; }
        public string TestType { get; set; }
        public string Country { get; set; }
        public string ProvinceState { get; set; }
        public string City { get; set; }
        public string DateTestTaken { get; set; }
        public string TestDescription { get; internal set; }
        public string GeneralInformation { get; set; }
        public string NormingGroup { get; set; }
        public string Official { get; set; }
        public string OfficialYes { get; set; }
        public string ApplicationCycle { get; set; }
        public string TestDetails { get; set; }
        public string Tests { get; set; }
        public string RawScores { get; set; }
        public string Percentile { get; set; }
    }
}
