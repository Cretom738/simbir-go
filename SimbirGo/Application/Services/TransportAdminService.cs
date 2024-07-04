using Application.Dtos;
using AutoMapper;
using Domain.Enumerations;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class TransportAdminService : ITransportAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransportAdminService
            (ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransportAdminDto>> GetListAsync(TransportAdminListDto dto)
        {
            IQueryable<Transport> transports = _context.Transports
                .AsQueryable();
            transports = dto.TransportType == TransportTypeListEnum.All
                ? transports
                : transports.Where(t => t.TransportTypeId == (int)dto.TransportType);
            return _mapper.Map<IEnumerable<TransportAdminDto>>(await transports
                .Skip(dto.Start)
                .Take(dto.Count)
                .ToListAsync());
        }

        public async Task<TransportAdminDto> GetByIdAsync(long transportId)
        {
            Transport? transport = await FindTransportByIdAsync(transportId);
            if (transport == null)
            {
                throw new NotFoundException("transport.not.found.by.id");
            }
            return _mapper.Map<TransportAdminDto>(transport);
        }

        public async Task<TransportAdminDto> CreateAsync(TransportAdminCreateDto dto)
        {
            if (dto.CanBeRented
                && dto.MinutePrice == null
                && dto.DayPrice == null)
            {
                throw new BadRequestException("cannot.be.rentable.without.price");
            }
            if (!await DoesAccountExistAsync(dto.OwnerId))
            {
                throw new NotFoundException("account.not.found.by.id");
            }
            Transport newTransport = _mapper.Map<Transport>(dto);
            await _context.Transports.AddAsync(newTransport);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransportAdminDto>(newTransport);
        }

        public async Task<TransportAdminDto> UpdateByIdAsync(long transportId, TransportAdminUpdateDto dto)
        {
            if (dto.CanBeRented
                && dto.MinutePrice == null
                && dto.DayPrice == null)
            {
                throw new BadRequestException("cannot.be.rentable.without.price");
            }
            Transport? transport = await FindTransportByIdAsync(transportId);
            if (transport == null)
            {
                throw new NotFoundException("transport.not.found.by.id");
            }
            if (!await DoesAccountExistAsync(dto.OwnerId))
            {
                throw new NotFoundException("account.not.found.by.id");
            }
            _mapper.Map(dto, transport);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransportAdminDto>(transport);
        }

        public async Task DeleteByIdAsync(long transportId)
        {
            Transport? transport = await FindTransportByIdAsync(transportId);
            if (transport == null)
            {
                throw new NotFoundException("transport.not.found.by.id");
            }
            if (await IsTransportAssociatedWithUnfinishedRentAsync(transportId))
            {
                throw new ConflictException("cannot.delete.transport.with.unfinished.rents");
            }
            _context.Transports.Remove(transport);
            await _context.SaveChangesAsync();
        }

        private Task<Transport?> FindTransportByIdAsync(long transportId)
        {
            return _context.Transports
                .FirstOrDefaultAsync(t => t.Id == transportId);
        }

        private async Task<bool> DoesAccountExistAsync(long accountId)
        {
            return await _context.Accounts
                .AnyAsync(a => a.Id == accountId);
        }

        private async Task<bool> IsTransportAssociatedWithUnfinishedRentAsync(long transportId)
        {
            return await _context.Rents
                .AnyAsync(r => r.TimeEnd == null
                    && r.TransportId == transportId);
        }
    }
}
