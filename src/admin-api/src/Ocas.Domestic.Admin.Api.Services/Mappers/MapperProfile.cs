using System;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Dto = Ocas.Domestic.Models;
using DtoEnums = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public class MapperProfile : Profile
    {
        public bool IsOcasUser { get; set; }

        public MapperProfile()
        {
            // DateTimes
            CreateMap<DateTime, string>().ConvertUsing<DateTimeTypeConverter>();
            CreateMap<DateTime?, string>().ConvertUsing<DateTimeTypeConverter>();
            CreateMap<string, DateTime>().ConvertUsing<UtcStringTypeConverter>();
            CreateMap<string, DateTime?>().ConvertUsing<UtcStringTypeConverter>();

            //Auditable
            CreateMap<Dto.IAuditable, IAuditable>().ForMember(d => d.ModifiedBy, opt => opt.MapFrom((src, _) =>
            {
                const string userObfuscation = "OCAS";

                if (string.IsNullOrEmpty(src.ModifiedBy)) return userObfuscation;

                if (!IsOcasUser)
                {
                    var ocasEmail = new Regex(@"^(@ocas\.ca|@ontariocolleges\.ca)$", RegexOptions.IgnoreCase);
                    if (ocasEmail.IsMatch(src.ModifiedBy)) return userObfuscation;
                }

                return src.ModifiedBy;
            })).IncludeAllDerived();

            // Brief
            CreateMap<HighSchool, HighSchoolBrief>();

            // Lookups, 1-way mapping only
            CreateMap<Dto.AboriginalStatus, AboriginalStatus>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.ApplicationCycle, ApplicationCycle>();
            CreateMap<Dto.Campus, Campus>().ForMember(x => x.CollegeId, opt => opt.MapFrom(m => m.ParentId));
            CreateMap<Dto.City, City>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.College, College>().ForMember(x => x.Address, opt => opt.MapFrom(s => s.MailingAddress));
            CreateMap<Dto.CollegeApplicationCycle, CollegeApplicationCycle>().ForMember(x => x.MasterId, opt => opt.MapFrom(c => c.ApplicationCycleId));
            CreateMap<Dto.Country, Country>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.CredentialEvaluationAgency, LookupItem>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.Name));
            CreateMap<Dto.DocumentPrint, DocumentPrint>();
            CreateMap<Dto.HighestEducation, LookupItem>().ForMember(x => x.Code, opt => opt.MapFrom(s => s.Name));
            CreateMap<Dto.HighSchool, HighSchool>().ForMember(x => x.Address, opt => opt.MapFrom(s => s.MailingAddress));
            CreateMap<Dto.Institute, LookupItem>().ForMember(x => x.Label, opt => opt.MapFrom(s => s.Name));
            CreateMap<Dto.McuCode, McuCode>().ForMember(x => x.Title, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.Model<Guid>, LookupItem>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName)).IncludeAllDerived();
            CreateMap<Dto.PrivacyStatement, PrivacyStatement>();
            CreateMap<Dto.ProgramSubCategory, SubCategory>().ForMember(x => x.CategoryId, opt => opt.MapFrom(m => m.ProgramCategoryId))
                                                            .ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.ProvinceState, ProvinceState>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.ReferralPartner, ReferralPartner>();
            CreateMap<Dto.TranscriptRequestException, InstituteWarning>().ForMember(x => x.Content, opt => opt.MapFrom(s => s.LocalizedName));
            CreateMap<Dto.University, University>().ForMember(x => x.Address, opt => opt.MapFrom(s => s.MailingAddress));

            // Crm entities
            CreateMap<Dto.AcademicRecord, SupportingDocument>()
                .ForMember(x => x.ReceivedDate, opt => opt.MapFrom(s => s.CreatedOn))
                .ForMember(x => x.Processing, opt => opt.MapFrom(_ => false))
                .ForMember(x => x.Type, opt => opt.MapFrom(_ => SupportingDocumentType.Grades))
                .ForMember(x => x.Name, opt => opt.Ignore());

            CreateMap<Dto.AcademicRecord, AcademicRecord>();

            CreateMap<Dto.Address, MailingAddress>();
            CreateMap<Dto.Address, ApplicantAddress>().ReverseMap();
            CreateMap<Dto.ApplicantBrief, ApplicantBrief>();
            CreateMap<Dto.Application, Application>();

            CreateMap<Dto.CollegeTransmission, CollegeTransmissionHistory>()
                .ForMember(x => x.ContextId, opt => opt.MapFrom(c => c.BusinessKey))
                .ForMember(x => x.Sent, opt => opt.MapFrom(c => c.LastLoadDateTime));

            CreateMap<Dto.Contact, Applicant>()
                .ForMember(x => x.EnrolledInHighSchool, opt => opt.MapFrom(c => c.HighSchoolEnrolled))
                .ForMember(x => x.GraduatedHighSchool, opt => opt.MapFrom(c => c.HighSchoolGraduated))
                .ForMember(x => x.GraduationHighSchoolDate, opt => opt.MapFrom(c => c.HighSchoolGraduationDate.ToStringOrDefault(Constants.DateFormat.YearMonthDashed)))
                .ForMember(x => x.AgreedToCasl, opt => opt.MapFrom((contact, _) =>
                {
                    if (contact.DoNotSendMM is null || contact.LastUsedInCampaign is null) return null;
                    return (bool?)!contact.DoNotSendMM.Value;
                }));

            CreateMap<Dto.Contact, ApplicantUpdateInfo>();

            CreateMap<Dto.CustomAudit, ApplicantHistory>()
                .ForMember(x => x.Message, opt => opt.MapFrom(x => x.CustomMessage))
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.CustomEntityLabel));

            CreateMap<Dto.CustomAuditDetail, ApplicantHistoryDetail>()
                .ForMember(x => x.ApplicantHistoryId, opt => opt.MapFrom(x => x.CustomAuditId));

            CreateMap<Dto.Education, Education>()
                .ForMember(x => x.CanDelete, opt => opt.MapFrom(s => !(s.HasTranscripts || s.HasPaidApplication)))
                .ForMember(x => x.TranscriptFee, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Dto.FinancialTransaction, FinancialTransaction>();

            CreateMap<Dto.Offer, Offer>()
                .ForMember(x => x.StudyMethodId, opt => opt.MapFrom(y => y.OfferStudyMethodId));

            CreateMap<Dto.OfferAcceptance, OfferHistory>();

            CreateMap<Dto.OrderDetail, OrderDetail>()
                .ForMember(x => x.Amount, opt => opt.MapFrom(y => y.PricePerUnit))
                .ForMember(x => x.ContextId, opt => opt.MapFrom((entity, _) =>
                {
                    if (entity.Type == Domestic.Enums.ShoppingCartItemType.Voucher)
                    {
                        return entity.Description;
                    }

                    return entity.ReferenceId.HasValue ? entity.ReferenceId.ToString() : null;
                }));

            CreateMap<Dto.Order, Order>()
                .ForMember(x => x.Number, opt => opt.MapFrom(y => y.OrderNumber))
                .ForMember(x => x.Amount, opt => opt.MapFrom(y => y.FinalTotal));

            CreateMap<Dto.OntarioHighSchoolCourseCode, OntarioHighSchoolCourseCode>()
                .ForMember(x => x.Code, opt => opt.MapFrom(y => y.Name))
                .ForMember(x => x.Label, opt => opt.MapFrom(y => y.Title));

            CreateMap<Dto.OntarioStudentCourseCredit, OntarioStudentCourseCredit>()
                .ForMember(x => x.CompletedDate, opt => opt.MapFrom(y => y.CompletedDate.ToNullableDateTime(Constants.DateFormat.CompletedDate).ToStringOrDefault(Constants.DateFormat.YearMonthDashed)))
                .ForMember(x => x.Notes, opt => opt.MapFrom(y => y.Notes.Select(c => c.ToString()).ToList()));

            CreateMap<Dto.ProgramBase, ProgramBase>()
                .ForMember(x => x.ApplicationCycleId, opt => opt.MapFrom(y => y.CollegeApplicationCycleId))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(y => y.ModifiedOn));

            CreateMap<Dto.Program, Program>()
                .ForMember(x => x.ApplicationCycleId, opt => opt.MapFrom(y => y.CollegeApplicationCycleId))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(y => y.ModifiedOn));

            CreateMap<Dto.Program, ProgramBrief>();

            CreateMap<Dto.ProgramChoice, ProgramChoice>()
                .ForMember(x => x.IntakeId, opt => opt.MapFrom(pc => pc.ProgramIntakeId))
                .ReverseMap();

            CreateMap<Dto.ProgramIntake, ProgramIntake>()
                .ForMember(x => x.IntakeAvailabilityId, opt => opt.MapFrom(y => y.AvailabilityId))
                .ForMember(x => x.IntakeStatusId, opt => opt.MapFrom(y => y.ProgramIntakeStatusId))
                .ForMember(x => x.EnrolmentEstimate, opt => opt.MapFrom(y => y.EnrolmentProjection))
                .ForMember(x => x.EnrolmentMax, opt => opt.MapFrom(y => y.EnrolmentMaximum))
                .ForMember(x => x.IntakeExpiryActionId, opt => opt.MapFrom(y => y.ExpiryActionId))
                .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(y => y.ModifiedOn));

            CreateMap<Dto.ProgramIntake, IntakeBrief>()
                .ForMember(x => x.DeliveryId, opt => opt.MapFrom(p => p.ProgramDeliveryId))
                .ForMember(x => x.IntakeAvailabilityId, opt => opt.MapFrom(p => p.AvailabilityId))
                .ForMember(x => x.IntakeStatusId, opt => opt.MapFrom(p => p.ProgramIntakeStatusId))
                .ForMember(x => x.EligibleEntryLevelIds, opt => opt.MapFrom(p => p.EntryLevels));

            CreateMap<Dto.Receipt, Receipt>();

            CreateMap<Dto.ShoppingCartDetail, ShoppingCartDetail>()
                .ForMember(x => x.ContextId, opt => opt.MapFrom((entity, _) =>
                {
                    if (entity.Type == DtoEnums.ShoppingCartItemType.Voucher)
                    {
                        return entity.Description;
                    }

                    return entity.ReferenceId.HasValue ? entity.ReferenceId.ToString() : null;
                }));

            CreateMap<Dto.ShoppingCart, ShoppingCart>();

            CreateMap<Dto.ProgramSpecialCode, SpecialCode>()
                .ForMember(x => x.ApplicationCycleId, opt => opt.MapFrom(s => s.CollegeApplicationId))
                .ForMember(x => x.Description, opt => opt.MapFrom(s => s.LocalizedName));

            CreateMap<Dto.SupportingDocument, SupportingDocument>()
               .ForMember(x => x.ReceivedDate, opt => opt.MapFrom(s => s.DateReceived))
               .ForMember(x => x.Processing, opt => opt.MapFrom(s => s.Availability != DtoEnums.SupportingDocumentAvailability.AvailableforDistribution))
               .ForMember(x => x.Type, opt => opt.MapFrom(_ => SupportingDocumentType.Other));

            CreateMap<Dto.Transcript, SupportingDocument>()
                .ForMember(x => x.ReceivedDate, opt => opt.MapFrom(s => s.CreatedOn))
                .ForMember(x => x.Processing, opt => opt.MapFrom(_ => false))
                .ForMember(x => x.Type, opt => opt.MapFrom(_ => SupportingDocumentType.Transcript));

            CreateMap<Dto.TranscriptRequest, TranscriptRequest>()
                .ForMember(x => x.Amount, opt => opt.MapFrom(s => s.TranscriptFee))
                .ForMember(x => x.FromInstituteId, opt => opt.MapFrom(s => s.FromSchoolId))
                .ForMember(x => x.FromInstituteName, opt => opt.MapFrom(s => s.FromSchoolName))
                .ForMember(x => x.RequestStatusId, opt => opt.MapFrom(s => s.TranscriptRequestStatusId))
                .ForMember(x => x.ToInstituteId, opt => opt.MapFrom(s => s.ToSchoolId))
                .ForMember(x => x.ToInstituteName, opt => opt.MapFrom(s => s.ToSchoolName))
                .ForMember(x => x.TransmissionId, opt => opt.MapFrom(s => s.TranscriptTransmissionId));

            CreateMap<Dto.TranscriptTransmission, TranscriptTransmission>()
                .ForMember(x => x.EligibleUntil, opt => opt.MapFrom(m => m.TermDueDate))
                .ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));

            CreateMap<Dto.Test, SupportingDocument>()
                .ForMember(x => x.ReceivedDate, opt => opt.MapFrom(s => s.CreatedOn))
                .ForMember(x => x.Processing, opt => opt.MapFrom(_ => false))
                .ForMember(x => x.CanDownload, opt => opt.MapFrom(s => s.IsOfficial == true))
                .ForMember(x => x.Type, opt => opt.MapFrom(_ => SupportingDocumentType.StandardizedTest));
        }
    }
}
