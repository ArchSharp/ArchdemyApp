using Application.Helpers;
using Application.Services.Interfaces;
using Application.Utilities;
using AutoMapper;
using Domain.Entities.PayStack;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PayStack.Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class PayStackService : IPaystackService
    {
        private readonly PayStackSettings _payStackSettings;
        private readonly IMapper _mapper;
        private PayStackApi Paystack { get; set; }

        public PayStackService(IOptions<PayStackSettings> payStackSettings, IMapper mapper)
        {
            _payStackSettings = payStackSettings.Value;
            Paystack = new PayStackApi(_payStackSettings.SecretKey);
            _mapper = mapper;
        }

        public PayStackPaymentInitializeResponse InitializePayment(PayStackPayment customer)
        {
            string secretKey = _payStackSettings.SecretKey;
            TransactionInitializeRequest request = new()
            {
                AmountInKobo = customer.Amount * 100,
                Email = customer.Email,
                Reference = RefGenerator.Generate().ToString(),
                Currency = "NGN",
                CallbackUrl = "http://localhost:36222/donate/verify"
            };

            TransactionInitializeResponse response = Paystack.Transactions.Initialize(request);

            PayStackPaymentInitializeResponse iResponse = new PayStackPaymentInitializeResponse
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            };
            
            return iResponse;            
        }

        public TransactionVerifyResponse VerifyTransaction(string reference)
        {
            var response = Paystack.Transactions.Verify(reference);
            
            return response;
        }
    }
}
