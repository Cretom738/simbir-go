using Application.Dtos;
using Application.Services;
using Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Controllers.Conventions;

namespace WebApi.Controllers
{
    [Route("api/Admin/Account")]
    [ApiController]
    [Authorize(Roles = nameof(AccountRoleEnum.Admin))]
    [ApiConventionType(typeof(AccountAdminApiConventention))]
    public class AccountAdminController : ControllerBase
    {
        private readonly IAccountAdminService _accountAdminService;

        public AccountAdminController(IAccountAdminService accountAdminService)
        {
            _accountAdminService = accountAdminService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountAdminDto>>> GetListAsync(
            [FromQuery] AccountAdminListDto dto)
        {
            return Ok(await _accountAdminService.GetListAsync(dto));
        }

        [HttpGet("{accountId:long}")]
        public async Task<ActionResult<AccountAdminDto>> GetByIdAsync(
            [Range(1, long.MaxValue)] long accountId)
        {
            return Ok(await _accountAdminService.GetByIdAsync(accountId));
        }

        [HttpPost]
        public async Task<ActionResult<AccountAdminDto>> CreateAsync(
            [FromBody] AccountAdminCreateDto dto)
        {
            AccountAdminDto resultDto = await _accountAdminService.CreateAsync(dto);
            return Created(GetLocationPath(resultDto.Id), resultDto);
        }

        [HttpPut("{accountId:long}")]
        public async Task<ActionResult<AccountAdminDto>> UpdateByIdAsync(
            [Range(1, long.MaxValue)] long accountId, 
            [FromBody] AccountAdminUpdateDto dto)
        {
            return Ok(await _accountAdminService.UpdateByIdAsync(accountId, dto));
        }

        [HttpDelete("{accountId:long}")]
        public async Task<ActionResult> DeleteByIdAsync(
            [Range(1, long.MaxValue)] long accountId)
        {
            await _accountAdminService.DeleteByIdAsync(accountId);
            return Ok();
        }

        private string GetLocationPath(long accountId)
        {
            return $"/api/Admin/Account/{accountId}";
        }
    }
}
