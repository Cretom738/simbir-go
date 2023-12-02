using Application.Dtos;

namespace Application.Services
{
    public interface ITransportService
    {
        Task<TransportDto> GetByIdAsync(long transportId);
        Task<TransportDto> CreateAsync(TransportCreateDto dto);
        Task<TransportDto> UpdateByIdAsync(long transportId, TransportUpdateDto dto);
        Task DeleteByIdAsync(long transportId);
    }
}
