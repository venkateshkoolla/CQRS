using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public partial class ApiMapperBase
    {
        public FinancialTransaction MapFinancialTransaction(Dto.FinancialTransaction dbDto)
        {
            return _mapper.Map<FinancialTransaction>(dbDto);
        }
    }
}
