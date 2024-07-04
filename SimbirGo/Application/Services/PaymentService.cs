using Application.Dtos;
using AutoMapper;
using Domain.Enumerations;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private const int HesoyamMoneyValue = 250000;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _claimsPrincipal;
        private long CurrentUserAccountId => long.Parse(
            _claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public PaymentService(
            ApplicationDbContext context, 
            IMapper mapper, 
            ClaimsPrincipal claimsPrincipal)
        {
            _context = context;
            _mapper = mapper;
            _claimsPrincipal = claimsPrincipal;
        }

        public async Task<AccountDto> AddAccountMoneyAsync(long accountId)
        {
            if (!IsInRole(AccountRoleEnum.Admin)
                && CurrentUserAccountId != accountId)
            {
                throw new ForbiddenException("cannot.add.money.to.another.account");
            }
            Account? account = await FindAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new NotFoundException("account.not.found.by.id");
            }
            account.Balance += HesoyamMoneyValue;
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountDto>(account);
        }

        private bool IsInRole(AccountRoleEnum role)
        {
            return _claimsPrincipal.IsInRole(role.ToString());
        }

        private async Task<Account?> FindAccountByIdAsync(long accountId)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId);
        }
    }
}
