using System;
using FluentValidation;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators
{
    internal class TestValidatorCollection : InlineValidator<TestObject>
    {
        public TestValidatorCollection(params Action<TestValidatorCollection>[] actions)
        {
            foreach (var action in actions)
            {
                action(this);
            }
        }
    }
}
