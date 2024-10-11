using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetAccountsOptions
    {
        public GetAccountsOptions()
        {
            StateCode = State.Active;
        }

        public State StateCode { get; set; }
        public AccountType AccountType { get; set; }
        public SchoolStatusCode? SchoolStatusCode { get; set; }
        public Guid? ParentId { get; set; }
    }
}
