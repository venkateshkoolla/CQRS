using System;
using System.Collections.Generic;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class Program : ProgramBase, IModel<Guid>
    {
        public Guid Id { get; set; }
        public IList<Guid> EntryLevels { get; set; }
        public State StateCode { get; set; }
    }
}
