using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public void PatchApplicantMessage(ApplicantMessage model, ocaslr_applicantmessage entity)
        {
            // Only the read flag can be updated because all other fields are read-only system generated
            entity.ocaslr_read = model.HasRead;
        }
    }
}
