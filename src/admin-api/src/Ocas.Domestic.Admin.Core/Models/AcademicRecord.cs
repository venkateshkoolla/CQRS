using System;
using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class AcademicRecord : AcademicRecordBase, IAuditable
    {
        public IList<HighSchoolBrief> HighSchools { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class AcademicRecordBase
    {
        public Guid ApplicantId { get; set; }
        public Guid CommunityInvolvementId { get; set; }
        public string DateCredentialAchieved { get; set; }
        public Guid HighestEducationId { get; set; }
        public Guid? HighSkillsMajorId { get; set; }
        public Guid LiteracyTestId { get; set; }
        public Guid? SchoolId { get; set; }
    }
}