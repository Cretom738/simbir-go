using Application.Dtos;

namespace Application.Services
{
    public interface IRentService
    {
        Task<IEnumerable<TransportDto>> GetAvaliableTransportListAsync(AvaliableTransportListDto dto);
        Task<RentDto> GetByIdAsync(long rentId);
        Task<IEnumerable<RentDto>> GetCurrentUserHistoryAsync();
        Task<IEnumerable<RentDto>> GetTransportHistoryByIdAsync(long transportId);
        Task<RentDto> StartByIdAsync(long transportId, RentStartDto dto);
        Task<RentDto> EndByIdAsync(long rentId, RentEndDto dto);
    }
}
