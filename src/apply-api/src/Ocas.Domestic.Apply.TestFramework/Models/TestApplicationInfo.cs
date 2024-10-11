using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.Models
{
    public class TestApplicationInfo : Application
    {
        public List<NewTestOfferInfo> Offers { get; set; }
        public List<AlgoliaProgramIntake> ProgramChoices { get; set; }
        public AlgoliaProgramIntake AlternateProgramChoice { get; set; }
    }
}
