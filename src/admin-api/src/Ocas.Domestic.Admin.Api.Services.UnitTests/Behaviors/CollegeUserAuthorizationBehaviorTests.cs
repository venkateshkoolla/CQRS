using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Behaviors;
using Ocas.Domestic.Apply.Admin.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Behaviors
{
    public class CollegeUserAuthorizationBehaviorTests
    {
        [Fact]
        [UnitTest("Behaviors")]
        public void Behavior_ShouldPass_With_Authorized()
        {
            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.CanAccessCollegeAsync(It.IsAny<IPrincipal>(), It.IsAny<Guid>())).Returns(Task.CompletedTask);

            var behavior = new CollegeUserAuthorizationBehavior<MyRequest, Unit>(userAuthorization.Object);

            var request = new MyRequest
            {
                User = TestConstants.TestUser.College.TestPrincipal,
                CollegeId = Guid.NewGuid()
            };

            Func<Task> behaviorFunc = async () => await behavior.Handle(request, CancellationToken.None, () => Unit.Task);
            behaviorFunc.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Behaviors")]
        public void Behavior_ShouldFail_With_Unauthorized()
        {
            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.CanAccessCollegeAsync(It.IsAny<IPrincipal>(), It.IsAny<Guid>())).Throws<NotAuthorizedException>();
            var behavior = new CollegeUserAuthorizationBehavior<MyRequest, Unit>(userAuthorization.Object);

            var request = new MyRequest
            {
                User = TestConstants.TestUser.College.TestPrincipal,
                CollegeId = Guid.NewGuid()
            };

            Func<Task> behaviorFunc = async () => await behavior.Handle(request, CancellationToken.None, () => Unit.Task);
            behaviorFunc.Should().Throw<NotAuthorizedException>();
        }

        internal class MyRequest : IRequest, ICollegeUser
        {
            public IPrincipal User { get; set; }
            public Guid CollegeId { get; set; }
        }
    }
}
