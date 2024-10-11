using System;
using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<CollegeTransmissionHistory> MapCollegeTransmissionHistories(
            IList<Dto.CollegeTransmission> filteredDtoCollegeTransmissions,
            IList<College> colleges,
            IList<Dto.CollegeTransmission> allDtoCollegeTransmissions,
            TranslationsDictionary translationsDictionary)
        {
            var result = new List<CollegeTransmissionHistory>();
            foreach (var dtoTransmission in filteredDtoCollegeTransmissions)
            {
                var transmission = MapCollegeTransmissionHistory(dtoTransmission, colleges, translationsDictionary);
                if (dtoTransmission.TransactionType != Constants.CollegeTransmissionTransactionTypes.Insert)
                {
                    var details = allDtoCollegeTransmissions.Where(x => x.ColtraneXcId == dtoTransmission.ColtraneXcId && x.Id != dtoTransmission.Id)
                                                            .Distinct()
                                                            .OrderByDescending(x => x.LastLoadDateTime);

                    if (details.Any())
                    {
                        transmission.Details = new List<CollegeTransmissionHistory>();
                        foreach (var detail in details)
                        {
                            transmission.Details.Add(MapCollegeTransmissionHistory(detail, colleges, translationsDictionary));
                        }
                    }
                }

                result.Add(transmission);
            }

            return result;
        }

        private CollegeTransmissionHistory MapCollegeTransmissionHistory(Dto.CollegeTransmission dtoTransmission, IList<College> colleges, TranslationsDictionary translationsDictionary)
        {
            var transmission = _mapper.Map<CollegeTransmissionHistory>(dtoTransmission);

            var translations = new Dictionary<string, string>
            {
                { "ACI", translationsDictionary.Get("report.transmission.headers.ActivityCode_ACI") },
                { "ACN", translationsDictionary.Get("report.transmission.headers.ActivityCode_ACN") },
                { "ACP", translationsDictionary.Get("report.transmission.headers.ActivityCode_ACP") },
                { "ACR", translationsDictionary.Get("report.transmission.headers.ActivityCode_ACR") },
                { "ACU", translationsDictionary.Get("report.transmission.headers.ActivityCode_ACU") },
                { "ACX", translationsDictionary.Get("report.transmission.headers.ActivityCode_ACX") },
                { "ACW", translationsDictionary.Get("report.transmission.headers.ActivityCode_ACW") },
                { "CCD", translationsDictionary.Get("report.transmission.headers.ActivityCode_CCD") },
                { "CCI", translationsDictionary.Get("report.transmission.headers.ActivityCode_CCI") },
                { "CCN", translationsDictionary.Get("report.transmission.headers.ActivityCode_CCN") },
                { "CCU", translationsDictionary.Get("report.transmission.headers.ActivityCode_CCU") },
                { "CCY", translationsDictionary.Get("report.transmission.headers.ActivityCode_CCY") },
                { "ECI", translationsDictionary.Get("report.transmission.headers.ActivityCode_ECI") },
                { "ECU", translationsDictionary.Get("report.transmission.headers.ActivityCode_ECU") },
                { "GCD", translationsDictionary.Get("report.transmission.headers.ActivityCode_GCD") },
                { "GCI", translationsDictionary.Get("report.transmission.headers.ActivityCode_GCI") },
                { "GCU", translationsDictionary.Get("report.transmission.headers.ActivityCode_GCU") },
                { "ICD", translationsDictionary.Get("report.transmission.headers.ActivityCode_ICD") },
                { "ICI", translationsDictionary.Get("report.transmission.headers.ActivityCode_ICI") },
                { "ICU", translationsDictionary.Get("report.transmission.headers.ActivityCode_ICU") },
                { "NCD", translationsDictionary.Get("report.transmission.headers.ActivityCode_NCD") },
                { "NCI", translationsDictionary.Get("report.transmission.headers.ActivityCode_NCI") },
                { "NCU", translationsDictionary.Get("report.transmission.headers.ActivityCode_NCU") },
                { "SCD", translationsDictionary.Get("report.transmission.headers.ActivityCode_SCD") },
                { "SCI", translationsDictionary.Get("report.transmission.headers.ActivityCode_SCI") },
                { "SCU", translationsDictionary.Get("report.transmission.headers.ActivityCode_SCU") },
                { "TCD", translationsDictionary.Get("report.transmission.headers.ActivityCode_TCD") },
                { "TCI", translationsDictionary.Get("report.transmission.headers.ActivityCode_TCI") },
                { "TCU", translationsDictionary.Get("report.transmission.headers.ActivityCode_TCU") }
            };

            var college = colleges.FirstOrDefault(x => x.Code == dtoTransmission.CollegeCode);
            var collegeName = string.Empty;
            if (college != null)
            {
                transmission.CollegeId = college.Id;
                collegeName = college.Name;
            }

            if (dtoTransmission.TransactionCode == "AC")
            {
                switch (Enum.Parse(typeof(CollegeTransmissionActivity), dtoTransmission.TransactionCode + dtoTransmission.TransactionType))
                {
                    case CollegeTransmissionActivity.ACI:
                        transmission.Description = string.Format(translations["ACI"], collegeName);
                        break;
                    case CollegeTransmissionActivity.ACN:
                        transmission.Description = string.Format(translations["ACN"], collegeName);
                        break;
                    case CollegeTransmissionActivity.ACP:
                        transmission.Description = string.Format(translations["ACP"], collegeName);
                        break;
                    case CollegeTransmissionActivity.ACR:
                        transmission.Description = string.Format(translations["ACR"], collegeName);
                        break;
                    case CollegeTransmissionActivity.ACU:
                        transmission.Description = string.Format(translations["ACU"], collegeName);
                        break;
                    case CollegeTransmissionActivity.ACX:
                        transmission.Description = string.Format(translations["ACX"], collegeName);
                        break;
                    case CollegeTransmissionActivity.ACW:
                        transmission.Description = string.Format(translations["ACW"]);
                        break;
                    default:
                        transmission.Description = dtoTransmission.TransactionCode + "-" + ParseColtraneDescription(dtoTransmission.Description, dtoTransmission.TransactionCode);
                        break;
                }
            }
            else if (dtoTransmission.TransactionCode == "CC")
            {
                var program = dtoTransmission.Data.Substring(13, 7).Trim();
                var startDate = dtoTransmission.Data.Substring(28, 4).Trim();
                switch (Enum.Parse(typeof(CollegeTransmissionActivity), dtoTransmission.TransactionCode + dtoTransmission.TransactionType))
                {
                    case CollegeTransmissionActivity.CCD:
                        transmission.Description = string.Format(translations["CCD"], program, startDate, collegeName);
                        break;
                    case CollegeTransmissionActivity.CCI:
                        transmission.Description = string.Format(translations["CCI"], program, startDate, collegeName);
                        break;
                    case CollegeTransmissionActivity.CCN:
                        transmission.Description = string.Format(translations["CCN"], program, startDate, collegeName);
                        break;
                    case CollegeTransmissionActivity.CCU:
                        transmission.Description = string.Format(translations["CCU"], program, startDate, collegeName);
                        break;
                    case CollegeTransmissionActivity.CCY:
                        transmission.Description = string.Format(translations["CCY"], program, startDate, collegeName);
                        break;
                    case CollegeTransmissionActivity.GCU:
                        transmission.Description = string.Format(translations["GCU"], collegeName);
                        break;
                    default:
                        transmission.Description = dtoTransmission.TransactionCode + "-" + ParseColtraneDescription(dtoTransmission.Description, dtoTransmission.TransactionCode);
                        break;
                }
            }
            else if (dtoTransmission.TransactionCode == "EC")
            {
                var instName = dtoTransmission.Data.Substring(20, 30).Trim();
                switch (Enum.Parse(typeof(CollegeTransmissionActivity), dtoTransmission.TransactionCode + dtoTransmission.TransactionType))
                {
                    case CollegeTransmissionActivity.ECI:
                        transmission.Description = string.Format(translations["ECI"], instName, collegeName);
                        break;
                    case CollegeTransmissionActivity.ECU:
                        transmission.Description = string.Format(translations["ECU"], collegeName);
                        break;
                    default:
                        transmission.Description = dtoTransmission.TransactionCode + "-" + ParseColtraneDescription(dtoTransmission.Description, dtoTransmission.TransactionCode);
                        break;
                }
            }
            else if (dtoTransmission.TransactionCode == "GC")
            {
                var courseCode = dtoTransmission.Data.Substring(13, 6).Trim();
                switch (Enum.Parse(typeof(CollegeTransmissionActivity), dtoTransmission.TransactionCode + dtoTransmission.TransactionType))
                {
                    case CollegeTransmissionActivity.GCD:
                        transmission.Description = string.Format(translations["GCD"], courseCode, collegeName);
                        break;
                    case CollegeTransmissionActivity.GCI:
                        transmission.Description = string.Format(translations["GCI"], courseCode, collegeName);
                        break;
                    default:
                        transmission.Description = dtoTransmission.TransactionCode + "-" + ParseColtraneDescription(dtoTransmission.Description, dtoTransmission.TransactionCode);
                        break;
                }
            }
            else if (dtoTransmission.TransactionCode == "IC")
            {
                switch (Enum.Parse(typeof(CollegeTransmissionActivity), dtoTransmission.TransactionCode + dtoTransmission.TransactionType))
                {
                    case CollegeTransmissionActivity.ICD:
                        transmission.Description = string.Format(translations["ICD"], collegeName);
                        break;
                    case CollegeTransmissionActivity.ICI:
                        transmission.Description = string.Format(translations["ICI"], collegeName);
                        break;
                    case CollegeTransmissionActivity.ICU:
                        transmission.Description = string.Format(translations["ICU"], collegeName);
                        break;
                    default:
                        transmission.Description = dtoTransmission.TransactionCode + "-" + ParseColtraneDescription(dtoTransmission.Description, dtoTransmission.TransactionCode);
                        break;
                }
            }
            else if (dtoTransmission.TransactionCode == "NC")
            {
                switch (Enum.Parse(typeof(CollegeTransmissionActivity), dtoTransmission.TransactionCode + dtoTransmission.TransactionType))
                {
                    case CollegeTransmissionActivity.NCD:
                        transmission.Description = string.Format(translations["NCD"], collegeName);
                        break;
                    case CollegeTransmissionActivity.NCI:
                        transmission.Description = string.Format(translations["NCI"], collegeName);
                        break;
                    case CollegeTransmissionActivity.NCU:
                        transmission.Description = string.Format(translations["NCU"], collegeName);
                        break;
                    default:
                        transmission.Description = dtoTransmission.TransactionCode + "-" + ParseColtraneDescription(dtoTransmission.Description, dtoTransmission.TransactionCode);
                        break;
                }
            }
            else if (dtoTransmission.TransactionCode == "SC")
            {
                switch (Enum.Parse(typeof(CollegeTransmissionActivity), dtoTransmission.TransactionCode + dtoTransmission.TransactionType))
                {
                    case CollegeTransmissionActivity.SCD:
                        transmission.Description = string.Format(translations["SCD"], collegeName);
                        break;
                    case CollegeTransmissionActivity.SCI:
                        transmission.Description = string.Format(translations["SCI"], collegeName);
                        break;
                    case CollegeTransmissionActivity.SCU:
                        transmission.Description = string.Format(translations["SCU"], collegeName);
                        break;
                    default:
                        transmission.Description = dtoTransmission.TransactionCode + "-" + ParseColtraneDescription(dtoTransmission.Description, dtoTransmission.TransactionCode);
                        break;
                }
            }
            else if (dtoTransmission.TransactionCode == "TC")
            {
                var test = dtoTransmission.Data.Substring(13, 2).Trim();
                switch (Enum.Parse(typeof(CollegeTransmissionActivity), dtoTransmission.TransactionCode + dtoTransmission.TransactionType))
                {
                    case CollegeTransmissionActivity.TCD:
                        transmission.Description = string.Format(translations["TCD"], test, collegeName);
                        break;
                    case CollegeTransmissionActivity.TCI:
                        transmission.Description = string.Format(translations["TCI"], test, collegeName);
                        break;
                    case CollegeTransmissionActivity.TCU:
                        transmission.Description = string.Format(translations["TCU"], test, collegeName);
                        break;
                    default:
                        transmission.Description = dtoTransmission.TransactionCode + "-" + ParseColtraneDescription(dtoTransmission.Description, dtoTransmission.TransactionCode);
                        break;
                }
            }
            else
            {
                transmission.Description = dtoTransmission.TransactionCode + "-" + ParseColtraneDescription(dtoTransmission.Description, dtoTransmission.TransactionCode);
            }

            return transmission;
        }

        private string ParseColtraneDescription(string description, string transactionCode)
        {
            var retVal = string.Empty;

            if (!string.IsNullOrWhiteSpace(description))
            {
                switch (transactionCode)
                {
                    case Constants.CollegeTransmissionCodes.Applicant:
                        retVal = "AC";
                        break;
                    case Constants.CollegeTransmissionCodes.ProgramChoice:
                        retVal = GetSubstring(description, 32, 8)
                            + "-" + GetSubstring(description, 19, 4)
                            + "-" + GetSubstring(description, 40, 4)
                            + "-" + GetSubstring(description, 45, 2);
                        break;
                    case Constants.CollegeTransmissionCodes.Grade:
                        retVal = GetSubstring(description, 32, 8);
                        break;
                    case Constants.CollegeTransmissionCodes.SupportingDocument:
                        retVal = GetSubstring(description, 32, 2);
                        break;
                    case Constants.CollegeTransmissionCodes.Test:
                        retVal = GetSubstring(description, 32, 3) + "-" + GetSubstring(description, 35, 2);
                        break;
                    case Constants.CollegeTransmissionCodes.InternationalTranscriptEvalution:
                        retVal = GetSubstring(description, 32, 6);
                        break;
                    case Constants.CollegeTransmissionCodes.Education:
                        retVal = GetSubstring(description, 33, 6) + "-" + GetSubstring(description, 39, 30);
                        break;
                    case Constants.CollegeTransmissionCodes.NonFullTimeStudentActivity:
                        retVal = GetSubstring(description, 45, 50) + "-" + GetSubstring(description, 95, 25);
                        break;
                    default:
                        retVal = string.Empty;
                        break;
                }
            }

            return retVal;
        }

        private string GetSubstring(string value, int startIndex, int length)
        {
            if (!string.IsNullOrWhiteSpace(value) && value.Length >= (startIndex + length))
            {
                return value.Substring(startIndex, length).Trim();
            }

            return string.Empty;
        }
    }
}
