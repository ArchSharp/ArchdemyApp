using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Stripe_Payments_Web_Api.Models.Stripe;

namespace Server.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/Payment")]
public class PaymentController : Controller
{
    private readonly IStripeService _stripeService;
    private readonly IConfiguration _configuration;
    
    public PaymentController(IConfiguration configuration, IStripeService stripeService)
    {
        _configuration = configuration;
        _stripeService = stripeService;
    }


    /// <summary>
    /// Endpoint to add customer to stripe
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    [HttpPost()]
    [Route("AddCustomerToStripe")]
    [ProducesResponseType(typeof(SuccessResponse<StripeCustomer>), 200)]
    public async Task<IActionResult> AddStripeCustomer([FromBody] AddStripeCustomer customer, CancellationToken ct)
    {
        var createdCustomer = await _stripeService.AddStripeCustomerAsync(customer, ct);

        return Ok(createdCustomer);
    }

    /// <summary>
    /// Endpoint to make stripe payment
    /// </summary>
    /// <param name="payment"></param>
    /// <returns></returns>
    [HttpPost()]
    [Route("PayStripe")]
    [ProducesResponseType(typeof(SuccessResponse<StripePayment>), 200)]
    public async Task<IActionResult> AddStripePayment([FromBody] AddStripePayment payment, CancellationToken ct)
    {
        var createdPayment = await _stripeService.AddStripePaymentAsync(payment, ct);

        return Ok(createdPayment);
    }
}