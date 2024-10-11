using System;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Ocas.Domestic.Data.Mappers;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Data.UnitTests.Mappers
{
    [Collection(nameof(XunitInjectionCollection))]
    public class EntityReferenceMapperTest
    {
        [Fact]
        [UnitTest("Mappers")]
        public void ToEntityReference_Should_ReturnEntityReference()
        {
            var id = Guid.NewGuid();
            var result = id.ToEntityReference("LogicalName");

            result.Should().BeOfType<EntityReference>();
            result.Id.Should().Be(id);
            result.LogicalName.Should().Be("LogicalName");
        }

        [Fact]
        [UnitTest("Mappers")]
        public void ToEntityReference_Should_ReturnNull()
        {
            Guid? id = null;
            var result = id.ToEntityReference("Null");

            result.Should().BeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void ToEntityReference_Nullable_Should_ReturnEntityReference()
        {
            Guid? id = Guid.NewGuid();
            var result = id.ToEntityReference("Null");
            result.Should().BeOfType<EntityReference>();

            result.Id.Should().Be(id.Value);
            result.LogicalName.Should().Be("Null");
        }
    }
}
