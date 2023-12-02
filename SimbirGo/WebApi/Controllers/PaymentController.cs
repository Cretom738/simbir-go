using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Controllers.Conventions;

namespace WebApi.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    [Authorize]
    [ApiConventionType(typeof(PaymentApiConvention))]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{accountId:long}")]
        public async Task<ActionResult<AccountDto>> AddAccountMoneyAsync(
            [Range(1, long.MaxValue)] long accountId)
        {
            return Ok(await _paymentService.AddAccountMoneyAsync(accountId));
        }
    }
}
