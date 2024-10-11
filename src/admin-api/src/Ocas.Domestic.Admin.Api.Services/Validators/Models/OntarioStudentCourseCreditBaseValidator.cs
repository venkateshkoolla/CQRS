using System;
using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models
{
    public class OntarioStudentCourseCreditBaseValidator : AbstractValidator<OntarioStudentCourseCreditBase>
    {
        public OntarioStudentCourseCreditBaseValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ApplicantId)
                .NotEmpty();

            RuleFor(x => x.CourseCode)
                .NotEmpty()
                .Length(5, 6)
                .MustAsync(async (y, _) =>
                {
                    var courseCode = await domesticContext.GetOntarioHighSchoolCourseCode(y);
                    return courseCode?.Name.Equals(y, StringComparison.OrdinalIgnoreCase) == true;
                })
                .WithMessage("'{PropertyName}' must exist.");

            RuleFor(x => x.Grade)
                .MustAsync(async (x, y, _) =>
                {
                    if (x.GradeTypeId.IsEmpty()) return false;

                    var gradeTypes = await lookupsCache.GetGradeTypes(Constants.Localization.EnglishCanada);
                    var gradeType = gradeTypes.FirstOrDefault(g => g.Id == x.GradeTypeId);

                    if (gradeType == null)
                        return false;

                    if (string.IsNullOrEmpty(y))
                    {
                        return gradeType.Code == Constants.OntarioHighSchool.GradeType.Projected
                               || gradeType.Code == Constants.OntarioHighSchool.GradeType.Current;
                    }

                    if (y.Length > 3)
                        return false;

                    if (int.TryParse(y, out var mark))
                    {
                        return mark >= 0 && mark <= 100;
                    }

                    if (gradeType.Code == Constants.OntarioHighSchool.GradeType.Midterm)
                    {
                        return y == Constants.OntarioHighSchool.CourseGrade.NotApplicable
                               || y == Constants.OntarioHighSchool.CourseGrade.AlternativeCourse
                               || y == Constants.OntarioHighSchool.CourseGrade.InsufficientEvidence;
                    }

                    return y == Constants.OntarioHighSchool.CourseGrade.Equivalent
                           || y == Constants.OntarioHighSchool.CourseGrade.NotApplicable
                           || y == Constants.OntarioHighSchool.CourseGrade.AlternativeCourse
                           || y == Constants.OntarioHighSchool.CourseGrade.InsufficientEvidence;
                })
                .WithMessage("'{PropertyName}' is invalid.");

            RuleFor(x => x.Credit)
                .InclusiveBetween(0, 99.99M);

            RuleFor(x => x.CourseMident)
                .NotEmpty()
                .Length(6)
                .MustAsync(async (y, _) =>
                {
                    var highSchools = await lookupsCache.GetHighSchools(Constants.Localization.EnglishCanada);
                    return highSchools.Any(h => h.Mident == y);
                })
                .WithMessage("'{PropertyName}' must exist.")
                .When(y => y.CourseMident != Constants.OntarioHighSchool.Mident.Default);

            RuleFor(x => x.CompletedDate)
                .NotEmpty()
                .Must(y => y.IsDate(Constants.DateFormat.YearMonthDashed))
                .WithMessage("'{PropertyName}' must be a valid date.")
                .MustAsync(async (x, y, _) =>
                {
                    if (x.GradeTypeId.IsEmpty()) return false;

                    var gradeTypes = await lookupsCache.GetGradeTypes(Constants.Localization.EnglishCanada);
                    if (!gradeTypes.Any(g => g.Id == x.GradeTypeId)) return false;

                    var gradeType = gradeTypes.First(g => g.Id == x.GradeTypeId);
                    if (gradeType.Code == Constants.OntarioHighSchool.GradeType.Final)
                    {
                        return y.ToDateTime(Constants.DateFormat.YearMonthDashed) <= DateTime.UtcNow.ToStringOrDefault(Constants.DateFormat.YearMonthDashed).ToDateTime(Constants.DateFormat.YearMonthDashed);
                    }

                    return y.ToDateTime(Constants.DateFormat.YearMonthDashed) >= DateTime.UtcNow.ToStringOrDefault(Constants.DateFormat.YearMonthDashed).ToDateTime(Constants.DateFormat.YearMonthDashed);
                })
                .WithMessage("'{PropertyName}' is invalid.");

            RuleFor(x => x.Notes)
                .Must(y => y.Count <= 5)
                .WithMessage(y => $"The count of '{{PropertyName}}' must be 5 items or fewer. You entered {y.Notes.Count} items.")
                .MustAsync(async (y, _) =>
                {
                    var ostNotes = await lookupsCache.GetOstNotes(Constants.Localization.EnglishCanada);
                    var ostNotesCodes = ostNotes.Select(n => n.Code);
                    return !y.Except(ostNotesCodes).Any();
                })
                .WithMessage("'{PropertyName}' must contain note codes.")
                .When(y => y.Notes?.Any() == true);

            RuleFor(x => x.GradeTypeId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var gradeTypes = await lookupsCache.GetGradeTypes(Constants.Localization.EnglishCanada);
                    return gradeTypes.Any(z => z.Id == y);
                })
                .WithMessage("'{PropertyName}' must exist.");

            RuleFor(x => x.CourseStatusId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var courseStatuses = await lookupsCache.GetCourseStatuses(Constants.Localization.EnglishCanada);
                    return courseStatuses.Any(z => z.Id == y);
                })
                .WithMessage("'{PropertyName}' must exist.")
                .MustAsync(async (x, y, _) =>
                {
                    var courseStatuses = await lookupsCache.GetCourseStatuses(Constants.Localization.EnglishCanada);
                    if (!courseStatuses.Any(c => c.Id == y)) return false;
                    if (courseStatuses.First(c => c.Id == y).Code != Constants.OntarioHighSchool.CourseStatus.Delete) return true;

                    if (x.GradeTypeId.IsEmpty()) return false;
                    var gradeTypes = await lookupsCache.GetGradeTypes(Constants.Localization.EnglishCanada);
                    if (!gradeTypes.Any(g => g.Id == x.GradeTypeId)) return false;

                    var gradeType = gradeTypes.First(g => g.Id == x.GradeTypeId);

                    return gradeType.Code == Constants.OntarioHighSchool.GradeType.Projected
                           || gradeType.Code == Constants.OntarioHighSchool.GradeType.Current;
                })
                .WithMessage("'{PropertyName}' is invalid.");

            RuleFor(x => x.CourseDeliveryId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var courseDeliveries = await lookupsCache.GetCourseDeliveries(Constants.Localization.EnglishCanada);
                    return courseDeliveries.Any(z => z.Id == y);
                })
                .WithMessage("'{PropertyName}' must exist.");

            RuleFor(x => x.CourseTypeId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var courseTypes = await lookupsCache.GetCourseTypes(Constants.Localization.EnglishCanada);
                    return courseTypes.Any(z => z.Id == y);
                })
                .WithMessage("'{PropertyName}' must exist.");
        }
    }
}