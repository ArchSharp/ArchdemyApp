using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Common;
using Stripe;
using Stripe_Payments_Web_Api.Models.Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class StripeService : IStripeService
    {
        private readonly ChargeService _chargeService;
        private readonly CustomerService _customerService;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public StripeService(
            ChargeService chargeService,
            CustomerService customerService,
            TokenService tokenService,
            IMapper mapper)
        {
            _chargeService = chargeService;
            _customerService = customerService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task <SuccessResponse<StripeCustomer>> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct)
        {
            // Set Stripe Token options based on customer data
            TokenCreateOptions tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Name = customer.Name,
                    Number = customer.CreditCard.CardNumber,
                    ExpYear = customer.CreditCard.ExpirationYear,
                    ExpMonth = customer.CreditCard.ExpirationMonth,
                    Cvc = customer.CreditCard.Cvc
                }
            };

            // Create new Stripe Token
            Token stripeToken = await _tokenService.CreateAsync(tokenOptions, null, ct);

            // Set Customer options using
            CustomerCreateOptions customerOptions = new CustomerCreateOptions
            {
                Name = customer.Name,
                Email = customer.Email,
                Source = stripeToken.Id
            };

            // Create customer at Stripe
            Customer createdCustomer = await _customerService.CreateAsync(customerOptions, null, ct);

            var response = _mapper.Map<StripeCustomer>(createdCustomer);

            // Return the created customer at stripe
            return new SuccessResponse<StripeCustomer>
            {
                Data = response,
                code = 200,
                Message = ResponseMessages.StripeCustomerCreated,
                ExtraInfo = "",
            };
        }

        public async Task<SuccessResponse<StripePayment>> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct)
        {
            // Set the options for the payment we would like to create at Stripe
            ChargeCreateOptions paymentOptions = new ChargeCreateOptions
            {
                Customer = payment.CustomerId,
                ReceiptEmail = payment.ReceiptEmail,
                Description = payment.Description,  
                Currency = payment.Currency,
                Amount = payment.Amount
            };

            // Create the payment
            var createdPayment = await _chargeService.CreateAsync(paymentOptions, null, ct);

            var response = _mapper.Map<StripePayment>(createdPayment);
            // Return the payment to requesting method
            return new SuccessResponse<StripePayment>
            {
                Data = response,
                code = 200,
                Message = ResponseMessages.StripePaymentSuccess,
                ExtraInfo = "",
            };
        }
    }
}
