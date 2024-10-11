namespace Ocas.Domestic.Apply.TestFramework.Models
{
    public class NewAccountModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Question1 { get; set; }
        public string Answer1 { get; set; }
        public string Question2 { get; set; }
        public string Answer2 { get; set; }
        public string Question3 { get; set; }
        public string Answer3 { get; set; }
        public bool AgreeToPrivacyStatement { get; set; }
        public bool AgreeToOCASMessages { get; set; }
        public bool AgreeToCollegeMessages { get; set; }
        public string ErrorMessage { get; set; }
    }
}
