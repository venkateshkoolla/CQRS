using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_test MapTestBase(TestBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new ocaslr_test();
            PatchTestBase(model, entity);

            return entity;
        }

        public void PatchTest(Test model, ocaslr_test entity)
        {
            PatchTestBase(model, entity);
        }

        private void PatchTestBase(TestBase model, ocaslr_test entity)
        {
            entity.ocaslr_applicantid = model.ApplicantId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.ocaslr_testtypeid = model.TestTypeId.ToEntityReference(ocaslr_testtype.EntityLogicalName);
            entity.ocaslr_countryid = model.CountryId.ToEntityReference(ocaslr_country.EntityLogicalName);
            entity.ocaslr_provincestate = model.ProvinceStateName;
            entity.ocaslr_provincestateid = model.ProvinceStateId.ToEntityReference(ocaslr_provincestate.EntityLogicalName);
            entity.ocaslr_city = model.CityName;
            entity.ocaslr_cityid = model.CityId.ToEntityReference(ocaslr_city.EntityLogicalName);
            entity.ocaslr_datetesttaken = model.DateTestTaken;
            entity.ocaslr_description = model.Description;
            entity.ocaslr_official = model.IsOfficial;
            entity.ocaslr_modifiedbyuser = model.ModifiedBy;
        }
    }
}
