using Application.Dtos;
using Application.Services;
using Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Controllers.Conventions;

namespace WebApi.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    [Authorize(Roles = nameof(AccountRoleEnum.Admin))]
    [ApiConventionType(typeof(RentAdminApiConvention))]
    public class RentAdminController : ControllerBase
    {
        private readonly IRentAdminService _rentAdminService;

        public RentAdminController(IRentAdminService rentAdminService)
        {
            _rentAdminService = rentAdminService;
        }

        [HttpGet("Rent/{rentId:long}")]
        public async Task<ActionResult<RentAdminDto>> GetByIdAsync(
            [Range(1, long.MaxValue)] long rentId)
        {
            return Ok(await _rentAdminService.GetByIdAsync(rentId));
        }

        [HttpGet("UserHistory/{accountId:long}")]
        public async Task<ActionResult<IEnumerable<RentAdminDto>>> GetAccountHistoryByIdAsync(
            [Range(1, long.MaxValue)] long accountId)
        {
            return Ok(await _rentAdminService.GetAccountHistoryByIdAsync(accountId));
        }

        [HttpGet("TransportHistory/{transportId:long}")]
        public async Task<ActionResult<IEnumerable<RentAdminDto>>> GetTransportHistoryByIdAsync(
            [Range(1, long.MaxValue)] long transportId)
        {
            return Ok(await _rentAdminService.GetTransportHistoryByIdAsync(transportId));
        }

        [HttpPost("Rent")]
        public async Task<ActionResult<RentAdminDto>> CreateAsync(
            [FromBody] RentAdminCreateDto dto)
        {
            RentAdminDto resultDto = await _rentAdminService.CreateAsync(dto);
            return Created(GetLocationPath(resultDto.Id), resultDto);
        }

        [HttpPost("Rent/End/{rentId:long}")]
        public async Task<ActionResult<RentAdminDto>> EndByIdAsync(
            [Range(1, long.MaxValue)] long rentId,
            [FromQuery] RentAdminEndDto dto)
        {
            return Ok(await _rentAdminService.EndByIdAsync(rentId, dto));
        }

        [HttpPut("Rent/{rentId:long}")]
        public async Task<ActionResult<RentAdminDto>> UpdateByIdAsync(
            [Range(1, long.MaxValue)] long rentId,
            [FromBody] RentAdminUpdateDto dto)
        {
            return Ok(await _rentAdminService.UpdateByIdAsync(rentId, dto));
        }

        [HttpDelete("Rent/{rentId:long}")]
        public async Task<ActionResult> DeleteByIdAsync(
            [Range(1, long.MaxValue)] long rentId)
        {
            await _rentAdminService.DeleteByIdAsync(rentId);
            return Ok();
        }

        private string GetLocationPath(long rentId)
        {
            return $"/api/Admin/Rent/{rentId}";
        }
    }
}
