using System;

namespace Ocas.Domestic.Models
{
    public interface IAccount : IModel<Guid>
    {
        Address MailingAddress { get; set; }
    }
}
