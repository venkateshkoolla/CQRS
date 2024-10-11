using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Ocas.Domestic.Apply.Api.Configuration;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.UnitTests.Configuration
{
    public class DashedRoutingConventionTests
    {
        [Theory]
        [UnitTest("Api_Convention")]
        [ClassData(typeof(ControllerRouteTestData))]
        public void DashedRoutes_ShouldPass_WhenControllers(string controllerTemplate, string controllerName, string controllerExpected)
        {
            // Arrange
            var applicationModel = new ApplicationModel();
            var controllerType = typeof(object);
            var controllerModel = new ControllerModel(controllerType.GetTypeInfo(), Array.Empty<object>())
            {
                ControllerName = controllerName
            };
            controllerModel.Selectors.Add(new SelectorModel { AttributeRouteModel = new AttributeRouteModel { Template = controllerTemplate } });
            applicationModel.Controllers.Add(controllerModel);
            var options = new MvcOptions();
            options.Conventions.Add(new DashedRoutingConvention());

            // Act
            options.Conventions[0].Apply(applicationModel);

            // Assert
            applicationModel.Controllers.Should().ContainSingle();
            var controller = applicationModel.Controllers.First();

            controller.Selectors[0].AttributeRouteModel.Template.Should().Be(controllerExpected);
        }

        [Theory]
        [UnitTest("Api_Convention")]
        [ClassData(typeof(ActionRouteTestData))]
        public void DashedRoutes_ShouldPass_WhenActions(string actionTemplate, string actionName, string actionExpected)
        {
            // Arrange
            var applicationModel = BuildModel(actionTemplate, actionName);
            var options = new MvcOptions();
            options.Conventions.Add(new DashedRoutingConvention());

            // Act
            options.Conventions[0].Apply(applicationModel);

            // Assert
            applicationModel.Controllers.Should().ContainSingle();
            var controller = applicationModel.Controllers.First();

            controller.Actions[0].Selectors[0].AttributeRouteModel.Template.Should().Be(actionExpected);
        }

        private class ControllerRouteTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // route only has controller name/placeholder part
                yield return new object[] { "[controller]", "Applicants", "applicants" };
                yield return new object[] { "Applicants", "Applicants", "applicants" };
                yield return new object[] { "[controller]", "PrivacyStatements", "privacy-statements" };
                yield return new object[] { "PrivacyStatements", "PrivacyStatements", "privacy-statements" };
                // routes with multiple parts
                yield return new object[] { "api/v1/[controller]", "PrivacyStatements", "api/v1/privacy-statements" };
                yield return new object[] { "api/v1/PrivacyStatements", "PrivacyStatements", "api/v1/privacy-statements" };
                yield return new object[] { "api/v1/[controller]", "Applicants", "api/v1/applicants" };
                yield return new object[] { "api/v1/Applicants", "Applicants", "api/v1/applicants" };
                yield return new object[] { "api/v1/[controller]/TrailingValue", "PrivacyStatements", "api/v1/privacy-statements/trailing-value" };
                yield return new object[] { "api/v1/PrivacyStatements/TrailingValue", "PrivacyStatements", "api/v1/privacy-statements/trailing-value" };
                // routes with multiple parts and parameters
                yield return new object[] { "api/{apiVersion}/[controller]/TrailingValue", "PrivacyStatements", "api/{apiVersion}/privacy-statements/trailing-value" };
                yield return new object[] { "api/{apiVersion}/PrivacyStatements/TrailingValue", "PrivacyStatements", "api/{apiVersion}/privacy-statements/trailing-value" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private class ActionRouteTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "[action]", "Authorized", "authorized" };
                yield return new object[] { "Authorized", "Authorized", "authorized" };
                yield return new object[] { "[action]", "HealthCheck", "health-check" };
                yield return new object[] { "HealthCheck", "HealthCheck", "health-check" };
                yield return new object[] { "BeforePart/[action]/AfterPart", "HealthCheck", "before-part/health-check/after-part" };
                yield return new object[] { "BeforePart/HealthCheck/AfterPart", "HealthCheck", "before-part/health-check/after-part" };
                yield return new object[] { "BeforePart/[action]/{someValue}/AfterPart", "HealthCheck", "before-part/health-check/{someValue}/after-part" };
                yield return new object[] { "BeforePart/HealthCheck/{someValue}/AfterPart", "HealthCheck", "before-part/health-check/{someValue}/after-part" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private static ApplicationModel BuildModel(string actionTemplate, string actionName)
        {
            var app = new ApplicationModel();
            var controllerType = typeof(object);
            var controllerModel = new ControllerModel(controllerType.GetTypeInfo(), Array.Empty<object>())
            {
                ControllerName = "HelloWorld"
            };
            controllerModel.Selectors.Add(new SelectorModel { AttributeRouteModel = new AttributeRouteModel { Template = "api/v1/hello-world" } });
            app.Controllers.Add(controllerModel);

            var actionModel1 = new ActionModel(controllerType.GetMethod(nameof(object.GetType)), Array.Empty<object>())
            {
                ActionName = actionName
            };
            actionModel1.Selectors.Add(new SelectorModel { AttributeRouteModel = new AttributeRouteModel { Template = actionTemplate } });
            controllerModel.Actions.Add(actionModel1);

            return app;
        }
    }
}
