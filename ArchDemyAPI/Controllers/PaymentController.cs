using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.PayStack;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using PayStack.Net;
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
    private readonly IPaystackService _paystackService;
    private readonly IConfiguration _configuration;

    public PaymentController(IConfiguration configuration, IStripeService stripeService, IPaystackService paystackService)
    {
        _configuration = configuration;
        _stripeService = stripeService;
        _paystackService = paystackService;
    }


    /// <summary>
    /// Endpoint to add customer to stripe
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="ct"></param>
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
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost()]
    [Route("PayStripe")]
    [ProducesResponseType(typeof(SuccessResponse<StripePayment>), 200)]
    public async Task<IActionResult> AddStripePayment([FromBody] AddStripePayment payment, CancellationToken ct)
    {
        var createdPayment = await _stripeService.AddStripePaymentAsync(payment, ct);

        return Ok(createdPayment);
    }

    /// <summary>
    /// Endpoint to add customer to paystack
    /// </summary>
    /// <param name="payment"></param>
    /// <returns></returns>
    [HttpPost()]
    [Route("PayStack")]
    [ProducesResponseType(typeof(PayStackPaymentInitializeResponse), 200)]
    public IActionResult PayStackPayment([FromBody] PayStackPayment payment)
    {
        var createdPayment = _paystackService.InitializePayment(payment);

        return Ok(createdPayment);
    }

    /// <summary>
    /// Endpoint to make paystack payment
    /// </summary>
    /// <param name="reference"></param>
    /// <returns></returns>
    [HttpPost()]
    [Route("PayStackPayment")]
    [ProducesResponseType(typeof(TransactionVerifyResponse), 200)]
    public IActionResult MakePayStackPayment([FromQuery] string reference)
    {
        var createdPayment = _paystackService.VerifyTransaction(reference);

        return Ok(createdPayment);
    }
}