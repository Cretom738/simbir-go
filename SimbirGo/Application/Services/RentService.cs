using Application.Dtos;
using AutoMapper;
using Domain.Enumerations;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Claims;

namespace Application.Services
{
    public class RentService : IRentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _claimsPrincipal;
        private long CurrentUserAccountId => long.Parse(
            _claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public RentService
            (ApplicationDbContext context,
            IMapper mapper,
            ClaimsPrincipal claimsPrincipal)
        {
            _context = context;
            _mapper = mapper;
            _claimsPrincipal = claimsPrincipal;
        }

        public async Task<IEnumerable<TransportDto>> GetAvaliableTransportListAsync(AvaliableTransportListDto dto)
        {
            IQueryable<Transport> transports = _context.Transports
                .Where(t => t.CanBeRented);
            transports = dto.TransportType == TransportTypeListEnum.All
                ? transports
                : transports.Where(t => t.TransportTypeId == (int)dto.TransportType);
            transports = transports
                .Where(t => Math.Sqrt(
                    Math.Pow(dto.Latitude - t.Latitude, 2)
                    + Math.Pow(dto.Longitude - t.Longitude, 2)) <= dto.Radius);
            return _mapper.Map<IEnumerable<TransportDto>>(await transports.ToListAsync());
        }

        public async Task<RentDto> GetByIdAsync(long rentId)
        {
            Rent? rent = await FindRentByIdAsync(rentId);
            if (rent == null)
            {
                throw new NotFoundException("rent.not.found.by.id");
            }
            if (rent.RenterId != CurrentUserAccountId
                && rent.Transport.OwnerId != CurrentUserAccountId)
            {
                throw new ForbiddenException("cannot.get.rent.when.not.owner.and.not.renter");
            }
            return _mapper.Map<RentDto>(rent);
        }

        public async Task<IEnumerable<RentDto>> GetCurrentUserHistoryAsync()
        {
            return _mapper.Map<IEnumerable<RentDto>>(await _context.Rents
                .Where(r => r.RenterId == CurrentUserAccountId)
                .ToListAsync());
        }

        public async Task<IEnumerable<RentDto>> GetTransportHistoryByIdAsync(long transportId)
        {
            Transport? transport = await FindTransportByIdAsync(transportId);
            if (transport == null)
            {
                throw new NotFoundException("transport.not.found.by.id");
            }
            if (transport.OwnerId != CurrentUserAccountId)
            {
                throw new ForbiddenException("cannot.get.transport.history.when.not.owner");
            }
            return _mapper.Map<IEnumerable<RentDto>>(await _context.Rents
                .Where(r => r.TransportId == transportId)
                .ToListAsync());
        }

        public async Task<RentDto> StartByIdAsync(long transportId, RentStartDto dto)
        {
            Transport? transport = await FindTransportByIdAsync(transportId);
            if (transport == null)
            {
                throw new NotFoundException("transport.not.found.by.id");
            }
            if (transport.OwnerId == CurrentUserAccountId)
            {
                throw new ConflictException("cannot.rent.own.transport");
            }
            Rent newRent = new Rent
            {
                RentTypeId = (int)dto.RentType
            };
            newRent.TimeStart = DateTime.UtcNow;
            newRent.TransportId = transportId;
            newRent.RenterId = CurrentUserAccountId;
            newRent.PriceOfUnit = dto.RentType switch
            {
                RentTypeEnum.Minutes => transport.MinutePrice 
                    ?? throw new ConflictException("cannot.rent.per.minute.when.no.minute.price"),
                RentTypeEnum.Days => transport.DayPrice 
                    ?? throw new ConflictException("cannot.rent.per.day.when.no.day.price"),
                _ => throw new BadRequestException()
            };
            Account? renter = await FindAccountByIdAsync(CurrentUserAccountId);
            if (renter == null)
            {
                throw new UnauthorizedException("not.authorized");
            }
            if (newRent.PriceOfUnit > renter.Balance)
            {
                throw new ConflictException("cannot.rent.when.not.enough.balance");
            }
            await _context.Rents.AddAsync(newRent);
            await _context.SaveChangesAsync();
            return _mapper.Map<RentDto>(newRent);
        }

        public async Task<RentDto> EndByIdAsync(long rentId, RentEndDto dto)
        {
            Rent? rent = await FindRentByIdAsync(rentId);
            if (rent == null)
            {
                throw new NotFoundException("rent.not.found.by.id");
            }
            if (rent.RenterId != CurrentUserAccountId)
            {
                throw new ConflictException("cannot.end.rent.when.not.renter");
            }
            if (rent.TimeEnd != null)
            {
                throw new ConflictException("rent.already.ended");
            }
            rent.TimeEnd = DateTime.UtcNow;
            double finalPrice = (RentTypeEnum)rent.RentTypeId switch
            {
                RentTypeEnum.Minutes => rent.FinalMinutePrice,
                RentTypeEnum.Days => rent.FinalDayPrice,
                _ => throw new InvalidEnumArgumentException()
            };
            rent.FinalPrice = finalPrice;
            rent.Renter.Balance -= finalPrice;
            Transport transport = rent.Transport;
            transport.Latitude = dto.Latitude;
            transport.Longitude = dto.Longitude;
            await _context.SaveChangesAsync();
            return _mapper.Map<RentDto>(rent);
        }

        private async Task<Rent?> FindRentByIdAsync(long rentId)
        {
            return await _context.Rents
                .Include(r => r.Transport)
                .Include(r => r.Renter)
                .FirstOrDefaultAsync(r => r.Id == rentId);
        }

        private Task<Transport?> FindTransportByIdAsync(long transportId)
        {
            return _context.Transports
                .FirstOrDefaultAsync(t => t.Id == transportId);
        }

        private async Task<Account?> FindAccountByIdAsync(long accountId)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId);
        }
    }
}
