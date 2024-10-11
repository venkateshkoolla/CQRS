using System;
using System.Security.Principal;
using MediatR;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class DeleteOntarioStudentCourseCredit : IRequest, IIdentityUser
    {
        public Guid OntarioStudentCourseCreditId { get; set; }
        public IPrincipal User { get; set; }
    }
}
