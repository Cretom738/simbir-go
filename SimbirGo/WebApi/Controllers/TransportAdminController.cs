using Application.Dtos;
using Application.Services;
using Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Controllers.Conventions;

namespace WebApi.Controllers
{
    [Route("api/Admin/Transport")]
    [ApiController]
    [Authorize(Roles = nameof(AccountRoleEnum.Admin))]
    [ApiConventionType(typeof(TransportAdminApiConventention))]
    public class TransportAdminController : ControllerBase
    {
        private readonly ITransportAdminService _transportAdminService;

        public TransportAdminController(ITransportAdminService transportAdminService)
        {
            _transportAdminService = transportAdminService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransportAdminDto>>> GetListAsync(
            [FromQuery] TransportAdminListDto dto)
        {
            return Ok(await _transportAdminService.GetListAsync(dto));
        }

        [HttpGet("{transportId:long}")]
        public async Task<ActionResult<TransportAdminDto>> GetByIdAsync(
            [Range(1, long.MaxValue)] long transportId)
        {
            return Ok(await _transportAdminService.GetByIdAsync(transportId));
        }

        [HttpPost]
        public async Task<ActionResult<TransportAdminDto>> CreateAsync(
            [FromBody] TransportAdminCreateDto dto)
        {
            TransportAdminDto resultDto = await _transportAdminService.CreateAsync(dto);
            return Created(GetLocationPath(resultDto.Id), resultDto);
        }

        [HttpPut("{transportId:long}")]
        public async Task<ActionResult<TransportAdminDto>> UpdateByIdAsync(
            [Range(1, long.MaxValue)] long transportId,
            [FromBody] TransportAdminUpdateDto dto)
        {
            return Ok(await _transportAdminService.UpdateByIdAsync(transportId, dto));
        }

        [HttpDelete("{transportId:long}")]
        public async Task<ActionResult> DeleteByIdAsync(
            [Range(1, long.MaxValue)] long transportId)
        {
            await _transportAdminService.DeleteByIdAsync(transportId);
            return Ok();
        }

        private string GetLocationPath(long transportId)
        {
            return $"/api/Admin/Transport/{transportId}";
        }
    }
}
