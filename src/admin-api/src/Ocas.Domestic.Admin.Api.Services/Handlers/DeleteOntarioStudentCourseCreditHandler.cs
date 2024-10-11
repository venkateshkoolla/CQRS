using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class DeleteOntarioStudentCourseCreditHandler : IRequestHandler<DeleteOntarioStudentCourseCredit>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;

        public DeleteOntarioStudentCourseCreditHandler(ILogger<DeleteOntarioStudentCourseCreditHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<Unit> Handle(DeleteOntarioStudentCourseCredit request, CancellationToken cancellationToken)
        {
            if (!_userAuthorization.IsOcasTier2User(request.User) && !_userAuthorization.IsHighSchoolUser(request.User))
            {
                throw new ForbiddenException();
            }

            var ontarioStudentCourseCredit = await _domesticContext.GetOntarioStudentCourseCredit(request.OntarioStudentCourseCreditId)
                ?? throw new NotFoundException("Ontario Student Course Credit Id not found.");

            await _userAuthorization.CanAccessApplicantAsync(request.User, ontarioStudentCourseCredit.ApplicantId);

            await _domesticContext.BeginTransaction();
            try
            {
                ontarioStudentCourseCredit.ModifiedBy = request.User.GetUpnOrEmail();
                await _domesticContext.UpdateOntarioStudentCourseCredit(ontarioStudentCourseCredit);
                await _domesticContext.DeleteOntarioStudentCourseCredit(ontarioStudentCourseCredit.Id);

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);
                throw;
            }

            return Unit.Value;
        }
    }
}