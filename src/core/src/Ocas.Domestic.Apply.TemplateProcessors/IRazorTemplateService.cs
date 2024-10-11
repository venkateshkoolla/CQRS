using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models.Templates;

namespace Ocas.Domestic.Apply.TemplateProcessors
{
    public interface IRazorTemplateService
    {
        byte[] GenerateHighSchoolGrades(HighSchoolGradesViewModel model);
        Task<byte[]> GenerateHighSchoolGradesAsync(HighSchoolGradesViewModel model);
        byte[] GeneratePostSecondaryTranscript(PostSecondaryTranscriptViewModel model);
        Task<byte[]> GeneratePostSecondaryTranscriptAsync(PostSecondaryTranscriptViewModel model);
        byte[] GenerateStandardizedTest(StandardizedTestViewModel model);
        Task<byte[]> GenerateStandardizedTestAsync(StandardizedTestViewModel model);
        byte[] GenerateTestPdf();
        Task<byte[]> GenerateTestPdfAsync();
    }
}
