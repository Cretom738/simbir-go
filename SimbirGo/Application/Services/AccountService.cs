using Application.Options;
using Application.Dtos;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;
using BC = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Options;
using Domain.Enumerations;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private const int BCryptWorkFactor = 13;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _claimsPrincipal;
        private readonly JwtOptions _jwtSettings;
        private long CurrentUserAccountId => long.Parse(
            _claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private string CurrentUsername => _claimsPrincipal.FindFirstValue(ClaimTypes.Name)!;

        public AccountService(
            ApplicationDbContext context, 
            IMapper mapper,
            ClaimsPrincipal claimsPrincipal,
            IOptions<JwtOptions> jwtSettings)
        {
            _context = context;
            _mapper = mapper;
            _claimsPrincipal = claimsPrincipal;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AccountDto> GetCurrentUserAsync()
        {
            Account? currentUserAccount = await FindAccountByIdAsync(CurrentUserAccountId);
            if (currentUserAccount == null)
            {
                throw new UnauthorizedException("not.authorized");
            }
            return _mapper.Map<AccountDto>(currentUserAccount);
        }

        public async Task<JwtDto> SignInAsync(AccountSignInDto dto)
        {
            Account? account = await GetAccountByUsernameAsync(dto.Username);
            if (account == null
                || !BC.EnhancedVerify(dto.Password, account.PasswordHash, HashType.SHA512))
            {
                throw new UnauthorizedException("wrong.username.or.password");
            }
            return new JwtDto
            {
                Token = GetToken(account)
            };
        }

        public async Task<AccountDto> SignUpAsync(AccountSignUpDto dto)
        {
            if (await IsUsernameUniqueAsync(dto.Username))
            {
                throw new ConflictException("username.already.exists");
            }
            Account newAccount = _mapper.Map<Account>(dto);
            newAccount.PasswordHash = EncryptPassword(dto.Password);
            newAccount.AccountRoleId = (int)AccountRoleEnum.User;
            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountDto>(newAccount);
        }

        public async Task<AccountDto> UpdateCurrentUserAsync(AccountUpdateDto dto)
        {
            if (CurrentUsername != dto.Username
                && await IsUsernameUniqueAsync(dto.Username))
            {
                throw new ConflictException("username.already.exists");
            }
            Account? currentUserAccount = await FindAccountByIdAsync(CurrentUserAccountId);
            if (currentUserAccount == null)
            {
                throw new UnauthorizedException("not.authorized");
            }
            _mapper.Map(dto, currentUserAccount);
            currentUserAccount.PasswordHash = EncryptPassword(dto.Password);
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountDto>(currentUserAccount);
        }

        private string GetToken(Account account)
        {
            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                    new Claim(ClaimTypes.Name, account.Username),
                    new Claim(ClaimTypes.Role, Enum.GetName((AccountRoleEnum)account.AccountRoleId)!)
                },
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(TimeSpan.FromSeconds(_jwtSettings.SecondsLifeTime)),
                signingCredentials: new SigningCredentials(
                    _jwtSettings.SecurityKey, SecurityAlgorithms.HmacSha256)));
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

        private async Task<Account?> GetAccountByUsernameAsync(string username)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Username == username);
        }

        private async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return await _context.Accounts
                .AnyAsync(a => a.Username == username);
        }
    }
}
