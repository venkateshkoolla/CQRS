using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class UpdateOntarioStudentCourseCredit : IRequest<OntarioStudentCourseCredit>, IApplicantUser
    {
        public OntarioStudentCourseCredit OntarioStudentCourseCredit { get; set; }
        public Guid OntarioStudentCourseCreditId { get; set; }
        public Guid ApplicantId { get; set; }
        public IPrincipal User { get; set; }
    }
}
