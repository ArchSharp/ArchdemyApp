using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Threading.Tasks;
using Application.Helpers;
using Application.DTOs;
using Domain.Common;
using Application.Services.Interfaces;
using Domain.Entities.Configurations;

namespace Application.Services.Implementations
{
    public class TwilioService : ITwilioService
    {
        private readonly TwilioFnParameters _twilioFnParams;

        public TwilioService(IOptions<TwilioFnParameters> twilioFnParams)
        {
            this._twilioFnParams = twilioFnParams.Value;
        }
        public async Task<SuccessResponse<MessageResource>> TwilioSendAsync(string message, string to)
        {
            TwilioClient.Init(_twilioFnParams.AccountSID, _twilioFnParams.AccountToken);

            var result = await MessageResource.CreateAsync(
                body: message,
                from: new PhoneNumber(_twilioFnParams.PhoneNumber),
                to: new PhoneNumber(to)
                
            );
            return new SuccessResponse<MessageResource>
            {
                Data = result,
                code = 200,
                Message = ResponseMessages.TwilioSMSFailed,
                ExtraInfo = "",
            };
        }
    }
}
