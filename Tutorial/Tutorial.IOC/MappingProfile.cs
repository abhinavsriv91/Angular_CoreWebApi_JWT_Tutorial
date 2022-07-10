using AutoMapper;
using Tutorial.Business.Models;
using Tutorial.Global.DTO;

namespace Tutorial.IOC
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Source -> Traget
            CreateMap<RoleDTO, Role>().ReverseMap();
            CreateMap<AuthenticatedUserDTO, User>().ReverseMap();

           // ForMember(d => d.Roles, o => o.MapFrom(src => src.Roles))
        }
    }
}
