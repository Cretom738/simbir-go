using Application.Dtos;

namespace Application.Services
{
    public interface IRentAdminService
    {
        Task<RentAdminDto> GetByIdAsync(long rentId);
        Task<IEnumerable<RentAdminDto>> GetAccountHistoryByIdAsync(long accountId);
        Task<IEnumerable<RentAdminDto>> GetTransportHistoryByIdAsync(long transportId);
        Task<RentAdminDto> CreateAsync(RentAdminCreateDto dto);
        Task<RentAdminDto> EndByIdAsync(long rentId, RentAdminEndDto dto);
        Task<RentAdminDto> UpdateByIdAsync(long rentId, RentAdminUpdateDto dto);
        Task DeleteByIdAsync(long rentId);
    }
}
