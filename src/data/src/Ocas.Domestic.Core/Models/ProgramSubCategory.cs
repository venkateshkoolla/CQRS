using System;

namespace Ocas.Domestic.Models
{
    public class ProgramSubCategory : Model<Guid>
    {
        public Guid ProgramCategoryId { get; set; }
    }
}
