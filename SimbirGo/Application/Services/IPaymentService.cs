using Application.Dtos;

namespace Application.Services
{
    public interface IPaymentService
    {
        Task<AccountDto> AddAccountMoneyAsync(long accountId);
    }
}
