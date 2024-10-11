using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public async Task<PagedResult<ApplicantBrief>> GetApplicantBriefs(GetApplicantBriefOptions options, UserType userType, string partnerCode)
        {
            var firstName = string.IsNullOrWhiteSpace(options.FirstName) ? null : $"%{options.FirstName}%";
            var lastName = string.IsNullOrWhiteSpace(options.LastName) ? null : $"%{options.LastName}%";
            var middleName = string.IsNullOrWhiteSpace(options.MiddleName) ? null : $"%{options.MiddleName}%";
            var mident = string.IsNullOrWhiteSpace(options.Mident) ? null : $"%{options.Mident}%";
            var previousLastName = string.IsNullOrWhiteSpace(options.PreviousLastName) ? null : $"%{options.PreviousLastName}%";

            var parameters = new DynamicParameters();
            parameters.Add(Sprocs.ApplicantBriefsGet.UserType, (int)userType);
            parameters.Add(Sprocs.ApplicantBriefsGet.PartnerId, partnerCode);
            parameters.Add(Sprocs.ApplicantBriefsGet.AccountNumber, options.AccountNumber);
            parameters.Add(Sprocs.ApplicantBriefsGet.ApplicationCycleId, options.ApplicationCycleId);
            parameters.Add(Sprocs.ApplicantBriefsGet.ApplicationNumber, options.ApplicationNumber);
            parameters.Add(Sprocs.ApplicantBriefsGet.ApplicationStatusId, options.ApplicationStatusId);
            parameters.Add(Sprocs.ApplicantBriefsGet.BirthDate, options.BirthDate);
            parameters.Add(Sprocs.ApplicantBriefsGet.Email, options.Email);
            parameters.Add(Sprocs.ApplicantBriefsGet.FirstName, firstName);
            parameters.Add(Sprocs.ApplicantBriefsGet.LastName, lastName);
            parameters.Add(Sprocs.ApplicantBriefsGet.MiddleName, middleName);
            parameters.Add(Sprocs.ApplicantBriefsGet.Mident, mident);
            parameters.Add(Sprocs.ApplicantBriefsGet.OntarioEducationNumber, options.OntarioEducationNumber);
            parameters.Add(Sprocs.ApplicantBriefsGet.PaymentLocked, options.PaymentLocked);
            parameters.Add(Sprocs.ApplicantBriefsGet.PhoneNumber, options.PhoneNumber);
            parameters.Add(Sprocs.ApplicantBriefsGet.PreviousLastName, previousLastName);
            parameters.Add(Sprocs.ApplicantBriefsGet.PageNumber, options.PageNumber ?? 1);
            parameters.Add(Sprocs.ApplicantBriefsGet.PageSize, options.PageSize ?? 100);
            parameters.Add(Sprocs.ApplicantBriefsGet.SortBy, options.SortBy.ToString());
            parameters.Add(Sprocs.ApplicantBriefsGet.SortDirection, options.SortDirection.ToString());
            parameters.Add(Sprocs.ApplicantBriefsGet.TotalCount, dbType: DbType.Int32, direction: ParameterDirection.Output);

            var applicantBriefs = await Connection.QueryAsync<ApplicantBrief>(Sprocs.ApplicantBriefsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
            var totalCount = parameters.Get<int>("TotalCount");

            return new PagedResult<ApplicantBrief>
            {
                TotalCount = totalCount,
                Items = applicantBriefs
            };
        }
    }
}
