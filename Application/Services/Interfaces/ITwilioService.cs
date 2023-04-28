using Twilio.Rest.Api.V2010.Account;
using System.Threading.Tasks;
using Application.Helpers;

namespace Identity.Interfaces
{
    public interface ITwilioService
    {
        Task<SuccessResponse<MessageResource>> TwilioSendAsync(string message, string to);
    }
}
