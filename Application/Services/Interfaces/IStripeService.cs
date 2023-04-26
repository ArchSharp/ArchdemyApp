using Application.Helpers;
using Stripe_Payments_Web_Api.Models.Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IStripeService : IAutoDependencyService
    {
        Task<SuccessResponse<StripeCustomer>> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct);
        Task<SuccessResponse<StripePayment>> AddStripePaymentAsync(AddStripePayment payment, CancellationToken ct);
    }
}
