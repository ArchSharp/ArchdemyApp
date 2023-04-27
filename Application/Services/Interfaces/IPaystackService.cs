using Application.Helpers;
using Domain.Entities.PayStack;
using PayStack.Net;
using Stripe_Payments_Web_Api.Models.Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IPaystackService : IAutoDependencyService
    {
        PayStackPaymentInitializeResponse InitializePayment(PayStackPayment customer);
        TransactionVerifyResponse VerifyTransaction(string reference);
    }
}
