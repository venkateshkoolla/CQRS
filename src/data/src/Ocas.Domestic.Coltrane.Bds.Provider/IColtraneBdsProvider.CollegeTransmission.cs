using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Coltrane.Bds.Provider
{
    public partial interface IColtraneBdsProvider
    {
        Task<IList<CollegeTransmission>> GetCollegeTransmissions(string applicationNumber, GetCollegeTransmissionHistoryOptions options = null);
    }
}
