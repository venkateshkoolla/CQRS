using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Contact = Ocas.Domestic.Crm.Entities.Contact;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public void PatchContactBase(ContactBase model, Contact entity)
        {
            entity.FirstName = model.FirstName;
            entity.MiddleName = model.MiddleName;
            entity.LastName = model.LastName;
            entity.OCASLR_PreferredName = model.PreferredName;
            entity.EMailAddress1 = model.Email;
            entity.OCASLR_UserName = model.Username;
            entity.ocaslr_userid = model.SubjectId;
            entity.OCASLR_ModifiedByUser = model.ModifiedBy;
            entity.BirthDate = model.BirthDate;
            entity.DoNotSendMM = model.DoNotSendMM;
            entity.OCASLR_ContactTypeEnum = (Contact_OCASLR_ContactType)model.ContactType;
            entity.ocaslr_accountstatusid = model.AccountStatusId.ToEntityReference(ocaslr_accountstatus.EntityLogicalName);
            entity.ocaslr_Source = model.SourceId.ToEntityReference(ocaslr_source.EntityLogicalName);
            entity.ocaslr_acceptedprivacystatementid = model.AcceptedPrivacyStatementId.ToEntityReference(ocaslr_privacystatement.EntityLogicalName);
            entity.ocaslr_preferredlanguageid = model.PreferredLanguageId.ToEntityReference(ocaslr_preferredlanguage.EntityLogicalName);
            entity.OCASLR_LastLogin = model.LastLogin;
            entity.OCASLR_isexceedlastloginperiod = model.LastLoginExceed;
            entity.ocaslr_sourcepartnerid = model.SourcePartnerId.ToEntityReference(Account.EntityLogicalName);
        }

        public void PatchContact(Models.Contact model, Contact entity)
        {
            PatchContactBase(model, entity);

            if (model.IsAboriginalPerson is null)
            {
                entity.ocaslr_IsAboriginalPersonEnum = null;
            }
            else if (model.IsAboriginalPerson.Value)
            {
                entity.ocaslr_IsAboriginalPersonEnum = Contact_ocaslr_IsAboriginalPerson.Yes;
            }
            else
            {
                entity.ocaslr_IsAboriginalPersonEnum = Contact_ocaslr_IsAboriginalPerson.No;
            }

            if (model.HighSchoolEnrolled is null)
            {
                entity.ocaslr_highschoolenrolledEnum = null;
            }
            else if (model.HighSchoolEnrolled.Value)
            {
                entity.ocaslr_highschoolenrolledEnum = Contact_ocaslr_highschoolenrolled.Yes;
            }
            else
            {
                entity.ocaslr_highschoolenrolledEnum = Contact_ocaslr_highschoolenrolled.No;
            }

            if (model.HighSchoolGraduated is null)
            {
                entity.ocaslr_highschoolgraduatedEnum = null;
            }
            else if (model.HighSchoolGraduated.Value)
            {
                entity.ocaslr_highschoolgraduatedEnum = Contact_ocaslr_highschoolgraduated.Yes;
            }
            else
            {
                entity.ocaslr_highschoolgraduatedEnum = Contact_ocaslr_highschoolgraduated.No;
            }

            entity.LastUsedInCampaign = model.LastUsedInCampaign;
            entity.MiddleName = model.MiddleName;
            entity.MobilePhone = model.MobilePhone;
            entity.OCASLR_DateOfArrival = model.DateOfArrival;
            entity.ocaslr_highschoolgraduationdate = model.HighSchoolGraduationDate;
            entity.ocaslr_otheraboriginalstatus = model.OtherAboriginalStatus;
            entity.Ocaslr_PaymentLocked = model.PaymentLocked;
            entity.OCASLR_PreviousLastName = model.PreviousLastName;
            entity.Telephone2 = model.HomePhone;
            entity.ocaslr_gendercodeid = model.GenderId.ToEntityReference(ocaslr_gendercode.EntityLogicalName);
            entity.ocaslr_firstgenerationapplicantid = model.FirstGenerationId.ToEntityReference(ocaslr_firstgenerationapplicant.EntityLogicalName);
            entity.ocaslr_firstlanguageid = model.FirstLanguageId.ToEntityReference(ocaslr_firstlanguage.EntityLogicalName);
            entity.ocaslr_titleid = model.TitleId.ToEntityReference(ocaslr_title.EntityLogicalName);
            entity.ocaslr_preferredcorrespondencemethodid = model.PreferredCorrespondenceMethodId.ToEntityReference(ocaslr_preferredcorrespondencemethod.EntityLogicalName);
            entity.ocaslr_countryofbirthid = model.CountryOfBirthId.ToEntityReference(ocaslr_country.EntityLogicalName);
            entity.ocaslr_countryofcitizenshipid = model.CountryOfCitizenshipId.ToEntityReference(ocaslr_country.EntityLogicalName);
            entity.ocaslr_aboriginalstatusid = model.AboriginalStatusId.ToEntityReference(ocaslr_aboriginalstatus.EntityLogicalName);
            entity.ocaslr_statusincanadaid = model.StatusInCanadaId.ToEntityReference(ocaslr_statusincanada.EntityLogicalName);
            entity.ocaslr_statusofvisaid = model.StatusOfVisaId.ToEntityReference(ocaslr_statusofvisa.EntityLogicalName);
            entity.ocaslr_preferredsponsoragencyid = model.SponsorAgencyId.ToEntityReference(ocaslr_preferredsponsoragency.EntityLogicalName);

            if (model.MailingAddress != null)
            {
                entity.Address1_AddressTypeCodeEnum = (Contact_Address1_AddressTypeCode?)ContactAddressType.Mailing;
                entity.Address1_Country = model.MailingAddress.Country;
                entity.Address1_Line2 = model.MailingAddress.Street;
                entity.Address1_Line1 = string.Empty;
                entity.Address1_City = model.MailingAddress.City;
                entity.Address1_StateOrProvince = model.MailingAddress.ProvinceState;
                entity.Address1_PostalCode = model.MailingAddress.PostalCode;
                entity.OCASLR_ValidatedAddress = model.MailingAddress.Verified;
            }
            else
            {
                entity.Address1_AddressTypeCodeEnum = null;
                entity.Address1_Country = null;
                entity.Address1_Line2 = null;
                entity.Address1_Line1 = string.Empty;
                entity.Address1_City = null;
                entity.Address1_StateOrProvince = null;
                entity.Address1_PostalCode = null;
                entity.OCASLR_ValidatedAddress = false;
            }
        }

        public Contact MapContactBase(ContactBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new Contact();
            PatchContactBase(model, entity);

            return entity;
        }
    }
}
