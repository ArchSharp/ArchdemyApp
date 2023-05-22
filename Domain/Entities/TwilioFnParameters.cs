﻿namespace Identity.Data.Models
{
    public class TwilioFnParameters
    {
        public string AccountSID { get; set; } = null!;
        public string AccountToken { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
