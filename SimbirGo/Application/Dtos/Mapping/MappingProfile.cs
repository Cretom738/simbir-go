using AutoMapper;
using Domain.Enumerations;
using Domain.Models;

namespace Application.Dtos.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapAccount();
            MapRent();
            MapTransport();
        }

        private void MapAccount()
        {
            CreateMap<AccountAdminCreateDto, Account>()
                .AfterMap((aacd, a) => a.AccountRoleId = GetAccountRoleId(aacd.IsAdmin));
            CreateMap<AccountAdminUpdateDto, Account>()
                .AfterMap((aaud, a) => a.AccountRoleId = GetAccountRoleId(aaud.IsAdmin));
            CreateMap<AccountSignInDto, Account>()
                .AfterMap((asid, a) => a.AccountRoleId = (int)AccountRoleEnum.User);
            CreateMap<AccountSignUpDto, Account>()
                .AfterMap((asud, a) => a.AccountRoleId = (int)AccountRoleEnum.User);
            CreateMap<AccountUpdateDto, Account>()
                .AfterMap((aud, a) => a.AccountRoleId = (int)AccountRoleEnum.User);
            CreateMap<Account, AccountAdminDto>()
                .ForMember(aad => aad.Role, 
                    o => o.MapFrom(a => GetAccountRoleName(a.AccountRoleId)));
            CreateMap<Account, AccountDto>()
                .ForMember(ad => ad.Role, 
                    o => o.MapFrom(a => GetAccountRoleName(a.AccountRoleId)));
        }

        private void MapRent()
        {
            CreateMap<RentAdminCreateDto, Rent>()
                .ForMember(r => r.RentType, o => o.Ignore())
                .AfterMap((racd, r) => r.RentTypeId = (int)racd.RentType);
            CreateMap<RentAdminUpdateDto, Rent>()
                .ForMember(r => r.RentType, o => o.Ignore())
                .AfterMap((raud, r) => r.RentTypeId = (int)raud.RentType);
            CreateMap<Rent, RentAdminDto>()
                .ForMember(rad => rad.RentType, o => o.Ignore())
                .ForMember(rad => rad.RentType, 
                    o => o.MapFrom(r => GetRentTypeName(r.RentTypeId)));
            CreateMap<Rent, RentDto>()
                .ForMember(rd => rd.RentType, o => o.Ignore())
                .ForMember(rd => rd.RentType, 
                    o => o.MapFrom(r => GetRentTypeName(r.RentTypeId)));
        }

        private void MapTransport()
        {
            CreateMap<TransportAdminCreateDto, Transport>()
                .ForMember(t => t.TransportType, o => o.Ignore())
                .AfterMap((tacd, t) => t.TransportTypeId = (int)tacd.TransportType);
            CreateMap<TransportAdminUpdateDto, Transport>()
                .ForMember(t => t.TransportType, o => o.Ignore())
                .AfterMap((taud, t) => t.TransportTypeId = (int)taud.TransportType);
            CreateMap<TransportCreateDto, Transport>()
                .ForMember(t => t.TransportType, o => o.Ignore())
                .AfterMap((tcd, t) => t.TransportTypeId = (int)tcd.TransportType);
            CreateMap<TransportUpdateDto, Transport>();
            CreateMap<Transport, TransportAdminDto>()
                .ForMember(tad => tad.TransportType, o => o.Ignore())
                .ForMember(tad => tad.TransportType, 
                    o => o.MapFrom(t => GetTransportTypeName(t.TransportTypeId)));
            CreateMap<Transport, TransportDto>()
                .ForMember(td => td.TransportType, o => o.Ignore())
                .ForMember(td => td.TransportType, 
                    o => o.MapFrom(t => GetTransportTypeName(t.TransportTypeId)));
        }

        private int GetAccountRoleId(bool isAdmin)
        {
            return (int)(isAdmin ? AccountRoleEnum.Admin : AccountRoleEnum.User);
        }

        private string GetAccountRoleName(int accountRoleId)
        {
            return Enum.GetName((AccountRoleEnum)accountRoleId)!;
        }

        private string GetRentTypeName(int RentTypeId)
        {
            return Enum.GetName((RentTypeEnum)RentTypeId)!;
        }

        private string GetTransportTypeName(int TransportTypeId)
        {
            return Enum.GetName((TransportTypeEnum)TransportTypeId)!;
        }
    }
}
