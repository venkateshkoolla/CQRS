using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Models
{
    public class Test : TestBase
    {
        public Guid Id { get; set; }
    }

    public class TestBase : Auditable
    {
        public Guid ApplicantId { get; set; }
        public Guid TestTypeId { get; set; }
        public Guid CountryId { get; set; }
        public string ProvinceStateName { get; set; }
        public Guid? ProvinceStateId { get; set; }
        public string CityName { get; set; }
        public Guid? CityId { get; set; }
        public DateTime DateTestTaken { get; set; }
        public string Description { get; set; }
        public bool? IsOfficial { get; set; }
        public string NormingGroupName { get; set; }
        public string ApplicationCycleName { get; set; }

        public IList<TestDetail> Details { get; set; }
    }
}
