using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Ocas.Domestic.Crm.Entities;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Data.UnitTests.Mappers
{
    public partial class CrmMapperTest
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapContactBase_Should_ReturnCrmEntity()
        {
            var model = _dataFakerFixture.Models.ContactBase.Generate();
            var entity = _mapper.MapContactBase(model);

            entity.Should().BeOfType<Contact>()
                .And.NotBeNull();

            entity.Id.Should().BeEmpty();
            entity.FirstName.Should().Be(model.FirstName);
            entity.LastName.Should().Be(model.LastName);
            entity.OCASLR_PreferredName.Should().Be(model.PreferredName);
            entity.OCASLR_UserName.Should().Be(model.Username);
            entity.ocaslr_userid.Should().Be(model.SubjectId);
            entity.EMailAddress1.Should().Be(model.Email);
            entity.BirthDate.Should().Be(model.BirthDate);
            entity.ocaslr_Source.Id.Should().Be(model.SourceId);
            entity.ocaslr_accountstatusid.Id.Should().Be(model.AccountStatusId);
            entity.OCASLR_ContactType.Value.Should().Be((int)model.ContactType);
            entity.ocaslr_acceptedprivacystatementid.Should()
                .Match<EntityReference>(x =>
                    (model.AcceptedPrivacyStatementId == null && x == null)
                    || model.AcceptedPrivacyStatementId.Value == x.Id);
            entity.ocaslr_preferredlanguageid.Id.Should().Be(model.PreferredLanguageId);
        }
    }
}
