using Application.Dtos;

namespace Application.Services
{
    public interface IAccountAdminService
    {
        Task<IEnumerable<AccountAdminDto>> GetListAsync(AccountAdminListDto dto);
        Task<AccountAdminDto> GetByIdAsync(long accountId);
        Task<AccountAdminDto> CreateAsync(AccountAdminCreateDto dto);
        Task<AccountAdminDto> UpdateByIdAsync(long accountId, AccountAdminUpdateDto dto);
        Task DeleteByIdAsync(long accountId);
    }
}
