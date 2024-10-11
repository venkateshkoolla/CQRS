using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class ApplicantCompletedSteps
    {
        public bool HasChanged { get; set; }
        public CompletedSteps? NewCompletedSteps { get; set; }
    }
}
