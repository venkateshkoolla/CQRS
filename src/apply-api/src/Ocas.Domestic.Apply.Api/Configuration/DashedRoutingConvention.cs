using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Api.Configuration
{
    public class DashedRoutingConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers.Where(a => a.Selectors != null).ToList())
            {
                foreach (var controllerSelector in controller.Selectors.Where(c => c.AttributeRouteModel != null).ToList())
                {
                    SplitAndKebeCase(controllerSelector, "[controller]", controller.ControllerName);

                    foreach (var action in controller.Actions.Where(a => a.Selectors != null).ToList())
                    {
                        foreach (var actionSelector in action.Selectors.Where(a => a.AttributeRouteModel != null).ToList())
                        {
                            SplitAndKebeCase(actionSelector, "[action]", action.ActionName);
                        }
                    }
                }
            }
        }

        private static void SplitAndKebeCase(SelectorModel model, string template, string name)
        {
            var selectorTemplate = model.AttributeRouteModel.Template;
            if (selectorTemplate.Contains(template))
                selectorTemplate = selectorTemplate.Replace(template, name);

            model.AttributeRouteModel.Template = SplitAndKebabCase(selectorTemplate);
        }

        private static string SplitAndKebabCase(string value)
        {
            return string.Join("/", value.Split('/').Select(x => IsRouteParam(x) ? x : x.ToKebabCase()));
        }

        private static bool IsRouteParam(string routePart)
        {
            return routePart.StartsWith("{") && routePart.EndsWith("}");
        }
    }
}
