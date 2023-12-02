using Application.Dtos;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Controllers.Conventions;

namespace WebApi.Controllers
{
    [Route("api/Rent")]
    [ApiController]
    [Authorize]
    [ApiConventionType(typeof(RentApiConvention))]
    public class RentController : ControllerBase
    {
        private readonly IRentService _rentService;

        public RentController(IRentService rentService)
        {
            _rentService = rentService;
        }

        [AllowAnonymous]
        [HttpGet("Transport")]
        public async Task<ActionResult<IEnumerable<RentDto>>> GetAvaliableTransportListAsync(
            [FromQuery] AvaliableTransportListDto dto)
        {
            return Ok(await _rentService.GetAvaliableTransportListAsync(dto));
        }

        [HttpGet("{rentId:long}")]
        public async Task<ActionResult<RentDto>> GetByIdAsync(
            [Range(1, long.MaxValue)] long rentId)
        {
            return Ok(await _rentService.GetByIdAsync(rentId));
        }

        [HttpGet("MyHistory")]
        public async Task<ActionResult<IEnumerable<RentDto>>> GetAccountHistoryByIdAsync()
        {
            return Ok(await _rentService.GetCurrentUserHistoryAsync());
        }

        [HttpGet("TransportHistory/{transportId:long}")]
        public async Task<ActionResult<IEnumerable<RentDto>>> GetTransportHistoryByIdAsync(
            [Range(1, long.MaxValue)] long transportId)
        {
            return Ok(await _rentService.GetTransportHistoryByIdAsync(transportId));
        }

        [HttpPost("New/{transportId:long}")]
        public async Task<ActionResult<RentDto>> StartByIdAsync(
            [Range(1, long.MaxValue)] long transportId,
            [FromQuery] RentStartDto dto)
        {
            RentDto resultDto = await _rentService.StartByIdAsync(transportId, dto);
            return Created(GetLocationPath(resultDto.Id), resultDto);
        }

        [HttpPost("End/{rentId:long}")]
        public async Task<ActionResult<RentDto>> EndByIdAsync(
            [Range(1, long.MaxValue)] long rentId,
            [FromQuery] RentEndDto dto)
        {
            return Ok(await _rentService.EndByIdAsync(rentId, dto));
        }

        private string GetLocationPath(long rentId)
        {
            return $"/api/Rent/{rentId}";
        }
    }
}
