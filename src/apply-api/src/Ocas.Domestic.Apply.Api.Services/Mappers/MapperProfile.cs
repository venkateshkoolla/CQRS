using System;
using AutoMapper;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // DateTimes
            CreateMap<DateTime, string>().ConvertUsing<DateTimeTypeConverter>();
            CreateMap<DateTime?, string>().ConvertUsing<DateTimeTypeConverter>();
            CreateMap<string, DateTime>().ConvertUsing<UtcStringTypeConverter>();
            CreateMap<string, DateTime?>().ConvertUsing<UtcStringTypeConverter>();

            // Crm Entities
            CreateMap<Dto.AcademicRecord, SupportingDocument>()
                .ForMember(x => x.ReceivedDate, opt => opt.MapFrom(s => s.CreatedOn))
                .ForMember(x => x.Processing, opt => opt.MapFrom(_ => false))
                .ForMember(x => x.Type, opt => opt.MapFrom(_ => SupportingDocumentType.Grades))
                .ForMember(x => x.Name, opt => opt.Ignore());

            CreateMap<Dto.Address, MailingAddress>();
            CreateMap<Dto.Address, ApplicantAddress>().ReverseMap();

            CreateMap<Dto.Contact, Applicant>()
                .ForMember(x => x.EnrolledInHighSchool, opt => opt.MapFrom(c => c.HighSchoolEnrolled))
                .ForMember(x => x.GraduatedHighSchool, opt => opt.MapFrom(c => c.HighSchoolGraduated))
                .ForMember(x => x.GraduationHighSchoolDate, opt => opt.MapFrom(c => c.HighSchoolGraduationDate.ToStringOrDefault(Domestic.Constants.DateFormat.YearMonthOnly)))
                .ForMember(x => x.AgreedToCasl, opt => opt.MapFrom((contact, _) =>
                {
                    if (contact.DoNotSendMM is null || contact.LastUsedInCampaign is null) return null;

                    return (bool?)!contact.DoNotSendMM.Value;
                }));

            CreateMap<Dto.ApplicantMessage, ApplicantMessage>()
                .ForMember(x => x.Read, opt => opt.MapFrom(m => m.HasRead))
                .ForMember(x => x.Title, opt => opt.MapFrom(m => m.LocalizedSubject))
                .ForMember(x => x.Message, opt => opt.MapFrom(m => m.LocalizedText));

            CreateMap<Dto.Application, Application>();

            CreateMap<Dto.CollegeInformation, CollegeInformation>()
                .ForMember(x => x.CollegeId, opt => opt.MapFrom(ci => ci.CollegeId))
                .ForMember(x => x.WelcomeText, opt => opt.MapFrom(ci => ci.LocalizedWelcomeText))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Dto.Education, Models.Education>()
                .ForMember(x => x.CanDelete, opt => opt.MapFrom(s => !(s.HasTranscripts || s.HasPaidApplication)))
                .ForMember(x => x.TranscriptFee, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Dto.FinancialTransaction, FinancialTransaction>();

            CreateMap<Dto.Offer, Offer>()
                .ForMember(x => x.StudyMethodId, opt => opt.MapFrom(y => y.OfferStudyMethodId));

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

            CreateMap<Dto.ProgramChoice, ProgramChoice>()
                .ForMember(x => x.IntakeId, opt => opt.MapFrom(pc => pc.ProgramIntakeId))
                .ReverseMap();

            CreateMap<Dto.Receipt, Receipt>();

            CreateMap<Dto.ShoppingCartDetail, ShoppingCartDetail>()
                .ForMember(x => x.ContextId, opt => opt.MapFrom((entity, _) =>
                {
                    if (entity.Type == Domestic.Enums.ShoppingCartItemType.Voucher)
                    {
                        return entity.Description;
                    }

                    return entity.ReferenceId.HasValue ? entity.ReferenceId.ToString() : null;
                }));

            CreateMap<Dto.ShoppingCart, ShoppingCart>();

            CreateMap<Dto.SupportingDocument, SupportingDocument>()
                .ForMember(x => x.ReceivedDate, opt => opt.MapFrom(s => s.DateReceived))
                .ForMember(x => x.Processing, opt => opt.MapFrom(s => s.Availability != SupportingDocumentAvailability.AvailableforDistribution))
                .ForMember(x => x.Type, opt => opt.MapFrom(_ => Enums.SupportingDocumentType.Other));

            CreateMap<Dto.Transcript, SupportingDocument>()
                .ForMember(x => x.ReceivedDate, opt => opt.MapFrom(s => s.CreatedOn))
                .ForMember(x => x.Processing, opt => opt.MapFrom(_ => false))
                .ForMember(x => x.Type, opt => opt.MapFrom(_ => Enums.SupportingDocumentType.Transcript));

            CreateMap<Dto.Test, SupportingDocument>()
                .ForMember(x => x.ReceivedDate, opt => opt.MapFrom(s => s.CreatedOn))
                .ForMember(x => x.Processing, opt => opt.MapFrom(_ => false))
                .ForMember(x => x.Type, opt => opt.MapFrom(_ => Enums.SupportingDocumentType.StandardizedTest));

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

            // Lookups, 1-way mapping only
            CreateMap<Dto.Model<Guid>, LookupItem>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.AboriginalStatus, AboriginalStatus>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.ApplicationCycle, ApplicationCycle>();
            CreateMap<Dto.City, City>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.CredentialEvaluationAgency, LookupItem>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.Name));
            CreateMap<Dto.College, College>().ForMember(x => x.Address, opt => opt.MapFrom(s => s.MailingAddress));
            CreateMap<Dto.CollegeApplicationCycle, CollegeApplicationCycle>().ForMember(x => x.MasterId, opt => opt.MapFrom(c => c.ApplicationCycleId));
            CreateMap<Dto.Campus, Campus>().ForMember(x => x.CollegeId, opt => opt.MapFrom(m => m.ParentId));
            CreateMap<Dto.Country, Country>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.DocumentPrint, DocumentPrint>();
            CreateMap<Dto.HighSchool, HighSchool>().ForMember(x => x.Address, opt => opt.MapFrom(s => s.MailingAddress));
            CreateMap<Dto.Institute, LookupItem>().ForMember(x => x.Label, opt => opt.MapFrom(s => s.Name));
            CreateMap<Dto.PrivacyStatement, PrivacyStatement>();
            CreateMap<Dto.ProvinceState, ProvinceState>().ForMember(x => x.Label, opt => opt.MapFrom(m => m.LocalizedName));
            CreateMap<Dto.ReferralPartner, ReferralPartner>();
            CreateMap<Dto.TranscriptRequestException, InstituteWarning>().ForMember(x => x.Content, opt => opt.MapFrom(s => s.LocalizedName));
            CreateMap<Dto.University, University>().ForMember(x => x.Address, opt => opt.MapFrom(s => s.MailingAddress));
        }
    }
}
