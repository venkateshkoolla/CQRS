using System;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class UpdateEducationStatus : IRequest, IApplicantUser
    {
        public Guid ApplicantId { get; set; }
        public bool? EnrolledInHighSchool { get; set; }
        public bool? GraduatedHighSchool { get; set; }
        public string GraduationHighSchoolDate { get; set; }
        public IPrincipal User { get; set; }
    }
}
