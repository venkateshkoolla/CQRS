using System;
using System.Security.Principal;
using MediatR;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Messages
{
    public class UpsertAcademicRecord : IRequest<UpsertAcademicRecordResult>, IApplicantUser
    {
        public AcademicRecordBase AcademicRecord { get; set; }
        public Guid ApplicantId { get; set; }
        public IPrincipal User { get; set; }
    }
}
