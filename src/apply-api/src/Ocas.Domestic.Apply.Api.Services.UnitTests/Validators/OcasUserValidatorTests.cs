using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Validators;
using Ocas.Domestic.Data;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators
{
    public static class OcasUserValidatorTests
    {
        public class BeOcasUserValidatorTests
        {
            private readonly TestValidatorCollection _validator;

            public BeOcasUserValidatorTests()
            {
                var userAuthorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), new AppSettingsMock());

                _validator = new TestValidatorCollection(v => v.RuleFor(o => o.GenericUser).BeOcasUser(userAuthorization));
            }

            [Fact]
            [UnitTest("Validators")]
            public async Task BeOcasUserValidator_ShouldPass()
            {
                var model = new TestObject
                {
                    GenericUser = new ClaimsPrincipal(
                      new ClaimsIdentity(
                          new[]
                          {
                            new Claim("role", TestConstants.Identity.OcasRole)
                          },
                          "Bearer",
                          "name",
                          "role"))
                };

                var result = await _validator.ValidateAsync(model);

                result.IsValid.Should().BeTrue();
            }

            [Fact]
            [UnitTest("Validators")]
            public void BeOcasUserValidator_ShouldThrow_When_NotOcasUser()
            {
                var model = new TestObject
                {
                    GenericUser = new ClaimsPrincipal(
                      new ClaimsIdentity(
                          new[]
                          {
                            new Claim("role", "ASDF")
                          },
                          "Bearer",
                          "name",
                          "role"))
                };

                Func<Task> action = () => _validator.ValidateAsync(model);

                action.Should().Throw<ForbiddenException>()
                    .WithMessage("'Generic User' must be an OCAS user.");
            }
        }

        public class NotBeOcasUserValidatorTests
        {
            private readonly TestValidatorCollection _validator;

            public NotBeOcasUserValidatorTests()
            {
                var userAuthorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), new AppSettingsMock());

                _validator = new TestValidatorCollection(v => v.RuleFor(o => o.GenericUser).NotBeOcasUser(userAuthorization));
            }

            [Fact]
            [UnitTest("Validators")]
            public async Task NotBeOcasUserValidator_ShouldPass()
            {
                var model = new TestObject
                {
                    GenericUser = new ClaimsPrincipal(
                      new ClaimsIdentity(
                          new[]
                          {
                            new Claim("role", "ASDF")
                          },
                          "Bearer",
                          "name",
                          "role"))
                };

                var result = await _validator.ValidateAsync(model);

                result.IsValid.Should().BeTrue();
            }

            [Fact]
            [UnitTest("Validators")]
            public void NotBeOcasUserValidator_ShouldThrow_When_NotOcasUser()
            {
                var model = new TestObject
                {
                    GenericUser = new ClaimsPrincipal(
                      new ClaimsIdentity(
                          new[]
                          {
                            new Claim("role", TestConstants.Identity.OcasRole)
                          },
                          "Bearer",
                          "name",
                          "role"))
                };
                Func<Task> action = () => _validator.ValidateAsync(model);

                action.Should().Throw<ForbiddenException>()
                    .WithMessage("'Generic User' must not be an OCAS user.");
            }
        }
    }
}
