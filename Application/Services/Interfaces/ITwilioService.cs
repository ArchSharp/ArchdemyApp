using Twilio.Rest.Api.V2010.Account;
using System.Threading.Tasks;
using Application.Helpers;

namespace Application.Services.Interfaces
{
    public interface ITwilioService : IAutoDependencyService
    {
        Task<SuccessResponse<MessageResource>> TwilioSendAsync(string message, string to);
    }
}
