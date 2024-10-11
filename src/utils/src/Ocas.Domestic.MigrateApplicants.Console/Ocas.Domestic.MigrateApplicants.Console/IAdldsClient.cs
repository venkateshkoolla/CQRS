using System;
using System.Collections.Generic;

namespace Ocas.Identity.Auth.Clients
{
    public interface IAdldsClient
    {
        Dictionary<string, AdldsUser> FindAll();
        void CreateAspNetIdentityUsers(Dictionary<string, AdldsUser> adldsUsers);
    }

    public class AdldsUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] PasswordQuestions { get; set; }
        public string[] PasswordAnswers { get; set; }
        public string Username { get; set; }
        public bool EmailConfirmed { get; set; }

        // metadata
        public bool Migrate { get; set; }
    }
}
