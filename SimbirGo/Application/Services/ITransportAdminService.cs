using Application.Dtos;

namespace Application.Services
{
    public interface ITransportAdminService
    {
        Task<IEnumerable<TransportAdminDto>> GetListAsync(TransportAdminListDto dto);
        Task<TransportAdminDto> GetByIdAsync(long transportId);
        Task<TransportAdminDto> CreateAsync(TransportAdminCreateDto dto);
        Task<TransportAdminDto> UpdateByIdAsync(long transportId, TransportAdminUpdateDto dto);
        Task DeleteByIdAsync(long transportId);
    }
}
