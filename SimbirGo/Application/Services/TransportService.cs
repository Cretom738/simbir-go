using Application.Dtos;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services
{
    public class TransportService : ITransportService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _claimsPrincipal;
        private long CurrentUserAccountId => long.Parse(
            _claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public TransportService
            (ApplicationDbContext context, 
            IMapper mapper, 
            ClaimsPrincipal claimsPrincipal)
        {
            _context = context;
            _mapper = mapper;
            _claimsPrincipal = claimsPrincipal;
        }

        public async Task<TransportDto> GetByIdAsync(long transportId)
        {
            Transport? transport = await FindTransportByIdAsync(transportId);
            if (transport == null)
            {
                throw new NotFoundException("transport.not.found.by.id");
            }
            return _mapper.Map<TransportDto>(transport);
        }

        public async Task<TransportDto> CreateAsync(TransportCreateDto dto)
        {
            if (dto.CanBeRented
                && dto.MinutePrice == null
                && dto.DayPrice == null)
            {
                throw new BadRequestException("cannot.be.rentable.without.price");
            }
            if (!await DoesAccountExistAsync(CurrentUserAccountId))
            {
                throw new UnauthorizedException("not.authorized");
            }
            Transport newTransport = _mapper.Map<Transport>(dto);
            newTransport.OwnerId = CurrentUserAccountId;
            await _context.Transports.AddAsync(newTransport);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransportDto>(newTransport);
        }

        public async Task<TransportDto> UpdateByIdAsync(long transportId, TransportUpdateDto dto)
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
            if (transport.OwnerId != CurrentUserAccountId)
            {
                throw new ForbiddenException("cannot.update.not.own.transport");
            }
            _mapper.Map(dto, transport);
            await _context.SaveChangesAsync();
            return _mapper.Map<TransportDto>(transport);
        }

        public async Task DeleteByIdAsync(long transportId)
        {
            Transport? transport = await FindTransportByIdAsync(transportId);
            if (transport == null)
            {
                throw new NotFoundException("transport.not.found.by.id");
            }
            if (transport.OwnerId != CurrentUserAccountId)
            {
                throw new ForbiddenException("cannot.delete.not.own.transport");
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

        private async Task<bool> IsTransportAssociatedWithUnfinishedRentAsync(long transportId)
        {
            return await _context.Rents
                .AnyAsync(r => r.TimeEnd == null
                    && r.TransportId == transportId);
        }

        private async Task<bool> DoesAccountExistAsync(long accountId)
        {
            return await _context.Accounts
                .AnyAsync(a => a.Id == accountId);
        }
    }
}
