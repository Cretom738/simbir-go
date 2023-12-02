using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Controllers.Conventions;

namespace WebApi.Controllers
{
    [Route("api/Transport")]
    [ApiController]
    [Authorize]
    [ApiConventionType(typeof(TransportApiController))]
    public class TransportController : ControllerBase
    {
        private readonly ITransportService _transportService;

        public TransportController(ITransportService transportService)
        {
            _transportService = transportService;
        }

        [HttpGet("{transportId:long}")]
        public async Task<ActionResult<TransportDto>> GetByIdAsync(
            [Range(1, long.MaxValue)] long transportId)
        {
            return Ok(await _transportService.GetByIdAsync(transportId));
        }

        [HttpPost]
        public async Task<ActionResult<TransportDto>> CreateAsync(
            [FromBody] TransportCreateDto dto)
        {
            TransportDto resultDto = await _transportService.CreateAsync(dto);
            return Created(GetLocationPath(resultDto.Id), resultDto);
        }

        [HttpPut("{transportId:long}")]
        public async Task<ActionResult<TransportDto>> UpdateByIdAsync(
            [Range(1, long.MaxValue)] long transportId,
            [FromBody] TransportUpdateDto dto)
        {
            return Ok(await _transportService.UpdateByIdAsync(transportId, dto));
        }

        [HttpDelete("{transportId:long}")]
        public async Task<ActionResult> DeleteByIdAsync(
            [Range(1, long.MaxValue)] long transportId)
        {
            await _transportService.DeleteByIdAsync(transportId);
            return Ok();
        }

        private string GetLocationPath(long transportId)
        {
            return $"/api/Transport/{transportId}";
        }
    }
}
