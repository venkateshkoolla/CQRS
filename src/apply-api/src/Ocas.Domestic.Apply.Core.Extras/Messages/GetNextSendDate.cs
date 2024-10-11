using System;
using MediatR;

namespace Ocas.Domestic.Apply.Core.Messages
{
    public class GetNextSendDate : IRequest<DateTime?>
    {
        public GetNextSendDate()
        {
            UtcNow = DateTime.UtcNow;
        }

        public DateTime UtcNow { get; set; }
    }
}
