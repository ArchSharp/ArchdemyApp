namespace Application.Services.Interfaces
{
    public class GoogleTwoFactorAuthResponse
    {
        public string QrCodeSetupImageUrl { get; set; } = null!;
        public string Account { get; set; } = null!;
        public string ManualEntryKey { get; set; } = null!;
    }
}