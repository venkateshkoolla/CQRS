using System;
using FluentValidation.Validators;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Services.Validators
{
    public class NullableGuidEmptyValidator : PropertyValidator
    {
        public NullableGuidEmptyValidator()
           : base("'{PropertyName}' must not be empty.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return !(context.PropertyValue as Guid?).IsEmpty();
        }
    }
}
