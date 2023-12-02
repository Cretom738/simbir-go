using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Conventions;
using WebApi.Extensions;
using WebApp.Filters;

namespace WebApi.Controllers
{
    [Route("api/Account")]
    [ApiController]
    [Authorize]
    [ApiConventionType(typeof(AccountApiConvention))]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("Me")]
        public async Task<ActionResult<AccountDto>> GetCurrentUserAsync()
        {
            return Ok(await _accountService.GetCurrentUserAsync());
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        [ForbidAuthenticatedFilter]
        public async Task<ActionResult<JwtDto>> SignInAsync(
            [FromBody] AccountSignInDto dto)
        {
            JwtDto resultDto = await _accountService.SignInAsync(dto);
            HttpContext.Session.SetActiveJwt(resultDto.Token);
            return Ok(resultDto);
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        [ForbidAuthenticatedFilter]
        public async Task<ActionResult<AccountDto>> SignUpAsync(
            [FromBody] AccountSignUpDto dto)
        {
            return Created(GetLocationPath(), await _accountService.SignUpAsync(dto));
        }

        [HttpPost("SignOut")]
        public ActionResult SignOutAsync()
        {
            HttpContext.Session.RemoveActiveJwt();
            return Ok();
        }

        [HttpPut("Update")]
        public async Task<ActionResult<AccountDto>> UpdateCurrentUserAsync(
            [FromBody] AccountUpdateDto dto)
        {
            return Ok(await _accountService.UpdateCurrentUserAsync(dto));
        }

        private string GetLocationPath()
        {
            return $"/api/Account/Me";
        }
    }
}
