using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Behaviors;
using Ocas.Domestic.Apply.Core.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Behaviors
{
    public class UserAuthorizationBehaviorTests
    {
        [Fact]
        [UnitTest("Behaviors")]
        public async Task Behavior_ShouldPass_With_Authorized()
        {
            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.CanAccessApplicantAsync(It.IsAny<IPrincipal>(), It.IsAny<Guid>(), It.IsAny<bool>())).Returns(Task.CompletedTask);
            var behavior = new UserAuthorizationBehavior<MyRequest, Unit>(userAuthorization.Object);

            var request = new MyRequest
            {
                User = TestConstants.TestUser.ApplicantPrincipal,
                ApplicantId = Guid.NewGuid()
            };

            await behavior.Handle(request, CancellationToken.None, () => Unit.Task);
        }

        [Fact]
        [UnitTest("Behaviors")]
        public void Behavior_ShouldFail_With_Unauthorized()
        {
            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.CanAccessApplicantAsync(It.IsAny<IPrincipal>(), It.IsAny<Guid>(), It.IsAny<bool>())).Throws<NotAuthorizedException>();
            var behavior = new UserAuthorizationBehavior<MyRequest, Unit>(userAuthorization.Object);

            var request = new MyRequest
            {
                User = TestConstants.TestUser.ApplicantPrincipal,
                ApplicantId = Guid.NewGuid()
            };

            Func<Task> behaviorFunc = async () => await behavior.Handle(request, CancellationToken.None, () => Unit.Task);
            behaviorFunc.Should().Throw<NotAuthorizedException>();
        }

        internal class MyRequest : IRequest, IApplicantUser
        {
            public IPrincipal User { get; set; }
            public Guid ApplicantId { get; set; }
        }
    }
}
