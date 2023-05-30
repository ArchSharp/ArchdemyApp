namespace Domain.Entities.Configurations
{
    public class TwilioFnParameters
    {
        public string AccountSID { get; set; } = null!;
        public string AccountToken { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
