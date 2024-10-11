using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class AnnotationTests : BaseTest
    {
        [Fact]
        public async Task GetSupportingDocument_Binary_Data_ShouldPass()
        {
            // Act
            var supportingDocumentBinary = await Context.GetSupportingDocumentBinaryData(TestConstants.Annotation.Id);

            // Assert
            supportingDocumentBinary.Id.Should().Be(TestConstants.Annotation.Id);
            supportingDocumentBinary.DocumentBody.Should().NotBeNullOrEmpty();
            supportingDocumentBinary.MimeType.Should().NotBeNullOrEmpty();
            supportingDocumentBinary.CreatedOn.ToString().Should().NotBeNullOrEmpty();
            supportingDocumentBinary.FileName.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetSupportingDocument_Binary_Data_Shouldbe_Null_When_No_Id_Exists()
        {
            // Act
            var supportingDocumentBinary = await Context.GetSupportingDocumentBinaryData(Guid.NewGuid());

            // Assert
            supportingDocumentBinary.Should().BeNull();
        }
    }
}
