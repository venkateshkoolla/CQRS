using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class GetOntarioHighSchoolCourseCode : IRequest<OntarioHighSchoolCourseCode>, IIdentityUser
    {
        public string Code { get; set; }

        public IPrincipal User { get; set; }
    }
}
