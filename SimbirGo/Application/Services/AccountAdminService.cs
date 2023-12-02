using Application.Dtos;
using AutoMapper;
using BCrypt.Net;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BC = BCrypt.Net.BCrypt;

namespace Application.Services
{
    public class AccountAdminService : IAccountAdminService
    {
        private const int BCryptWorkFactor = 13;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _claimsPrincipal;
        private long CurrentUserAccountId => long.Parse(
            _claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public AccountAdminService(
            ApplicationDbContext context,
            IMapper mapper,
            ClaimsPrincipal claimsPrincipal)
        {
            _context = context;
            _mapper = mapper;
            _claimsPrincipal = claimsPrincipal;
        }

        public async Task<IEnumerable<AccountAdminDto>> GetListAsync(AccountAdminListDto dto)
        {
            return _mapper.Map<IEnumerable<AccountAdminDto>>(await _context.Accounts
                .Skip(dto.Start)
                .Take(dto.Count)
                .ToListAsync());
        }

        public async Task<AccountAdminDto> GetByIdAsync(long accountId)
        {
            Account? account = await FindAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<AccountAdminDto>(account);
        }

        public async Task<AccountAdminDto> CreateAsync(AccountAdminCreateDto dto)
        {
            if (await IsUsernameUniqueAsync(dto.Username))
            {
                throw new ConflictException();
            }
            Account newAccount = _mapper.Map<Account>(dto);
            newAccount.PasswordHash = EncryptPassword(dto.Password);
            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountAdminDto>(newAccount);
        }

        public async Task<AccountAdminDto> UpdateByIdAsync(long accountId, AccountAdminUpdateDto dto)
        {
            Account? account = await FindAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new NotFoundException();
            }
            if (account.Username != dto.Username
                && await IsUsernameUniqueAsync(dto.Username))
            {
                throw new ConflictException();
            }
            _mapper.Map(dto, account);
            account.PasswordHash = EncryptPassword(dto.Password);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountAdminDto>(account);
        }

        public async Task DeleteByIdAsync(long accountId)
        {
            if (CurrentUserAccountId == accountId)
            {
                throw new ConflictException();
            }
            Account? account = await FindAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new NotFoundException();
            }
            if (await IsAccountAssociatedWithUnfinishedRentAsync(accountId))
            {
                throw new ConflictException();
            }
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        private string EncryptPassword(string password)
        {
            return BC.EnhancedHashPassword(password, BCryptWorkFactor, HashType.SHA512);
        }

        private async Task<Account?> FindAccountByIdAsync(long accountId)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId);
        }

        private async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return await _context.Accounts
                .AnyAsync(a => a.Username == username);
        }

        private async Task<bool> IsAccountAssociatedWithUnfinishedRentAsync(long accountId)
        {
            return await _context.Rents
                .AnyAsync(r => r.TimeEnd == null
                    && r.RenterId == accountId);
        }
    }
}
