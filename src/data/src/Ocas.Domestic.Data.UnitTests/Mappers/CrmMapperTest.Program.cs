using System;
using FluentAssertions;
using Ocas.Domestic.Crm.Entities;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Data.UnitTests.Mappers
{
    public partial class CrmMapperTest
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapProgram_Should_ReturnCrmEntity()
        {
            var model = _dataFakerFixture.Models.Program;
            var entity = _mapper.MapProgram(model);

            entity.Should().BeOfType<ocaslr_program>()
                .And.NotBeNull();

            entity.Id.Should().Be(model.Id);
            entity.ocaslr_title.Should().Be(model.Title);
            entity.ocaslr_programtypeid?.Id.Should().Be(model.ProgramTypeId);
            entity.ocaslr_length.Should().Be(model.Length);
            entity.ocaslr_unitofmeasureid?.Id.Should().Be(model.LengthTypeId);
            entity.ocaslr_mcucodeid?.Id.Should().Be(model.McuCodeId);
            entity.ocaslr_credentialid?.Id.Should().Be(model.CredentialId);
            entity.ocaslr_entrylevelid?.Id.Should().Be(model.DefaultEntryLevelId);
            entity.ocaslr_studyareaid?.Id.Should().Be(model.StudyAreaId);
            entity.ocaslr_highlycompetitiveid?.Id.Should().Be(model.HighlyCompetitiveId);
            entity.ocaslr_programlanguageid?.Id.Should().Be(model.LanguageId);
            entity.ocaslr_programlevelid?.Id.Should().Be(model.LevelId);
            entity.ocaslr_promotionid?.Id.Should().Be(model.PromotionId);
            entity.ocaslr_adulttrainingid?.Id.Should().Be(model.AdultTrainingId);
            entity.ocaslr_specialcodeid?.Id.Should().Be(model.SpecialCodeId ?? Guid.Empty);
            entity.ocaslr_apsnumber.Should().Be(model.ApsNumber);
            entity.ocaslr_ministryapprovalid?.Id.Should().Be(model.MinistryApprovalId);
            entity.ocaslr_url.Should().Be(model.Url);
            entity.ocaslr_programcategory1id?.Id.Should().Be(model.ProgramCategory1Id);
            entity.ocaslr_subcategory1id?.Id.Should().Be(model.ProgramSubCategory1Id);
            entity.ocaslr_programcategory2id?.Id.Should().Be(model.ProgramCategory2Id ?? Guid.Empty);
            entity.ocaslr_programsubcategory2id?.Id.Should().Be(model.ProgramSubCategory2Id ?? Guid.Empty);
            entity.ocaslr_modifiedbyuser.Should().Be(model.ModifiedBy);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapProgramBase_Should_ReturnCrmEntity()
        {
            var model = _dataFakerFixture.Models.Program;
            var entity = _mapper.MapProgramBase(model);

            entity.Should().BeOfType<ocaslr_program>()
                .And.NotBeNull();

            entity.Id.Should().BeEmpty();
            entity.ocaslr_title.Should().Be(model.Title);
            entity.ocaslr_programtypeid?.Id.Should().Be(model.ProgramTypeId);
            entity.ocaslr_length.Should().Be(model.Length);
            entity.ocaslr_unitofmeasureid?.Id.Should().Be(model.LengthTypeId);
            entity.ocaslr_mcucodeid?.Id.Should().Be(model.McuCodeId);
            entity.ocaslr_credentialid?.Id.Should().Be(model.CredentialId);
            entity.ocaslr_entrylevelid?.Id.Should().Be(model.DefaultEntryLevelId);
            entity.ocaslr_studyareaid?.Id.Should().Be(model.StudyAreaId);
            entity.ocaslr_highlycompetitiveid?.Id.Should().Be(model.HighlyCompetitiveId);
            entity.ocaslr_programlanguageid?.Id.Should().Be(model.LanguageId);
            entity.ocaslr_programlevelid?.Id.Should().Be(model.LevelId);
            entity.ocaslr_promotionid?.Id.Should().Be(model.PromotionId);
            entity.ocaslr_adulttrainingid?.Id.Should().Be(model.AdultTrainingId);
            entity.ocaslr_specialcodeid?.Id.Should().Be(model.SpecialCodeId ?? Guid.Empty);
            entity.ocaslr_apsnumber.Should().Be(model.ApsNumber);
            entity.ocaslr_ministryapprovalid?.Id.Should().Be(model.MinistryApprovalId);
            entity.ocaslr_url.Should().Be(model.Url);
            entity.ocaslr_programcategory1id?.Id.Should().Be(model.ProgramCategory1Id);
            entity.ocaslr_subcategory1id?.Id.Should().Be(model.ProgramSubCategory1Id);
            entity.ocaslr_programcategory2id?.Id.Should().Be(model.ProgramCategory2Id ?? Guid.Empty);
            entity.ocaslr_programsubcategory2id?.Id.Should().Be(model.ProgramSubCategory2Id ?? Guid.Empty);
            entity.ocaslr_modifiedbyuser.Should().Be(model.ModifiedBy);
        }
    }
}
