using Application.Dtos;
using AutoMapper;
using Domain.Enumerations;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Application.Services
{
    public class RentAdminService : IRentAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RentAdminService
            (ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RentAdminDto> GetByIdAsync(long rentId)
        {
            Rent? rent = await FindRentByIdAsync(rentId);
            if (rent == null)
            {
                throw new NotFoundException("rent.not.found.by.id");
            }
            return _mapper.Map<RentAdminDto>(rent);
        }

        public async Task<IEnumerable<RentAdminDto>> GetAccountHistoryByIdAsync(long accountId)
        {
            if (!await DoesAccountExistAsync(accountId))
            {
                throw new NotFoundException("account.not.found.by.id");
            }
            return _mapper.Map<IEnumerable<RentAdminDto>>(await _context.Rents
                .Where(r => r.RenterId == accountId)
                .ToListAsync());
        }


        public async Task<IEnumerable<RentAdminDto>> GetTransportHistoryByIdAsync(long transportId)
        {
            if (!await DoesTransportExistAsync(transportId))
            {
                throw new NotFoundException("transport.not.found.by.id");
            }
            return _mapper.Map<IEnumerable<RentAdminDto>>(await _context.Rents
                .Where(r => r.TransportId == transportId)
                .ToListAsync());
        }

        public async Task<RentAdminDto> CreateAsync(RentAdminCreateDto dto)
        {
            if (dto.TimeEnd != null
                && dto.TimeStart > dto.TimeEnd)
            {
                throw new BadRequestException("start.time.more.than.end.time");
            }
            if (!await DoesAccountExistAsync(dto.RenterId))
            {
                throw new NotFoundException("account.not.found.by.id");
            }
            Transport? transport = await FindTransportByIdAsync(dto.TransportId);
            if (transport == null)
            {
                throw new NotFoundException("transport.not.found.by.id");
            }
            if (transport.Rents.Any(r => r.TimeEnd != null))
            {
                throw new ConflictException("cannot.rent.rented.transport");
            }
            if (transport.OwnerId == dto.RenterId)
            {
                throw new ConflictException("owner.cannot.rent.his.own.transport");
            }
            Rent newRent = _mapper.Map<Rent>(dto);
            await _context.Rents.AddAsync(newRent);
            await _context.SaveChangesAsync();
            return _mapper.Map<RentAdminDto>(newRent);
        }

        public async Task<RentAdminDto> EndByIdAsync(long rentId, RentAdminEndDto dto)
        {
            Rent? rent = await FindRentByIdAsync(rentId);
            if (rent == null)
            {
                throw new NotFoundException("rent.not.found.by.id");
            }
            rent.TimeEnd ??= DateTime.UtcNow;
            rent.FinalPrice ??= (RentTypeEnum)rent.RentTypeId switch
            {
                RentTypeEnum.Minutes => rent.FinalMinutePrice,
                RentTypeEnum.Days => rent.FinalDayPrice,
                _ => throw new InvalidEnumArgumentException()
            };
            rent.Renter.Balance -= rent.FinalPrice.Value;
            Transport transport = rent.Transport;
            transport.Latitude = dto.Latitude;
            transport.Longitude = dto.Longitude;
            await _context.SaveChangesAsync();
            return _mapper.Map<RentAdminDto>(rent);
        }

        public async Task<RentAdminDto> UpdateByIdAsync(long rentId, RentAdminUpdateDto dto)
        {
            if (dto.TimeEnd != null
                && dto.TimeStart > dto.TimeEnd)
            {
                throw new BadRequestException("start.time.more.than.end.time");
            }
            Rent? rent = await FindRentByIdAsync(rentId);
            if (rent == null)
            {
                throw new NotFoundException("rent.not.found.by.id");
            }
            if (!await DoesAccountExistAsync(dto.RenterId))
            {
                throw new NotFoundException("account.not.found.by.id");
            }
            Transport? transport = await FindTransportByIdAsync(dto.TransportId);
            if (transport == null)
            {
                throw new NotFoundException("transport.not.found.by.id");
            }
            if (transport.OwnerId == dto.RenterId)
            {
                throw new ConflictException("owner.cannot.rent.his.own.transport");
            }
            _mapper.Map(dto, rent);
            await _context.SaveChangesAsync();
            return _mapper.Map<RentAdminDto>(rent);
        }

        public async Task DeleteByIdAsync(long rentId)
        {
            Rent? rent = await FindRentByIdAsync(rentId);
            if (rent == null)
            {
                throw new NotFoundException("rent.not.found.by.id");
            }
            _context.Rents.Remove(rent);
            await _context.SaveChangesAsync();
        }

        private async Task<Rent?> FindRentByIdAsync(long rentId)
        {
            return await _context.Rents
                .Include(r => r.Renter)
                .Include(r => r.Transport)
                .FirstOrDefaultAsync(r => r.Id == rentId);
        }

        private Task<Transport?> FindTransportByIdAsync(long transportId)
        {
            return _context.Transports
                .Include(t => t.Rents)
                .FirstOrDefaultAsync(t => t.Id == transportId);
        }

        private async Task<bool> DoesAccountExistAsync(long accountId)
        {
            return await _context.Accounts
                .AnyAsync(a => a.Id == accountId);
        }

        private async Task<bool> DoesTransportExistAsync(long transportId)
        {
            return await _context.Transports
                .AnyAsync(t => t.Id == transportId);
        }
    }
}
