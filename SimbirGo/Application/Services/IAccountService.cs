using Application.Dtos;

namespace Application.Services
{
    public interface IAccountService
    {
        Task<AccountDto> GetCurrentUserAsync();
        Task<JwtDto> SignInAsync(AccountSignInDto dto);
        Task<AccountDto> SignUpAsync(AccountSignUpDto dto);
        Task<AccountDto> UpdateCurrentUserAsync(AccountUpdateDto dto);
    }
}
