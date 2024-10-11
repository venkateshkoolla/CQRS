using System;
using Bogus;
using Microsoft.Xrm.Sdk;
using Ocas.Domestic.Crm.Entities;

namespace Ocas.Domestic.Data.TestFramework
{
    public class EntityFakerFixture
    {
        public Faker<EntityReference> EntityReference { get; set; }
        public Faker<ocaslr_program> Program { get; }

        public EntityFakerFixture()
        {
            EntityReference = new Faker<EntityReference>()
                .RuleFor(x => x.Id, f => Guid.NewGuid())
                .RuleFor(x => x.LogicalName, f => f.Random.Word());

            Program = new Faker<ocaslr_program>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.ocaslr_collegeapplicationcycleid, new EntityReference("ocaslr_collegeapplicationcycleid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_campusid, new EntityReference("ParentAccountId", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_campusid, new EntityReference("ocaslr_campusid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_ProgramDelivery, new EntityReference("ocaslr_ProgramDelivery", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_programtypeid, new EntityReference("ocaslr_programtypeid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_unitofmeasureid, new EntityReference("ocaslr_unitofmeasureid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_mcucodeid, new EntityReference("ocaslr_mcucodeid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_credentialid, new EntityReference("ocaslr_credentialid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_entrylevelid, new EntityReference("ocaslr_entrylevelid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_studyareaid, new EntityReference("ocaslr_studyareaid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_highlycompetitiveid, new EntityReference("ocaslr_highlycompetitiveid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_programlanguageid, new EntityReference("ocaslr_programlanguageid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_programlevelid, new EntityReference("ocaslr_programlevelid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_promotionid, new EntityReference("ocaslr_promotionid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_adulttrainingid, new EntityReference("ocaslr_adulttrainingid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_specialcodeid, new EntityReference("ocaslr_specialcodeid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_apsnumber, f => f.Random.Number(1500))
                .RuleFor(o => o.ocaslr_ministryapprovalid, new EntityReference("ocaslr_ministryapprovalid", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_programcategory1id, new EntityReference("ocaslr_programcategory1id", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_subcategory1id, new EntityReference("ocaslr_subcategory1id", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_programcategory2id, new EntityReference("ocaslr_programcategory2id", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_programsubcategory2id, new EntityReference("ocaslr_programsubcategory2id", Guid.NewGuid()))
                .RuleFor(o => o.ocaslr_code, f => f.Name.JobType())
                .RuleFor(o => o.ocaslr_programcode, f => f.Name.JobArea())
                .RuleFor(o => o.ocaslr_title, f => f.Name.JobTitle())
                .RuleFor(o => o.ocaslr_name, f => f.Name.JobTitle())
                .RuleFor(o => o.ocaslr_modifiedbyuser, f => f.Internet.Email());
        }
    }
}
