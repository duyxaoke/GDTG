using AutoMapper;
using Core.Data;
using Shared.Models;

namespace Web.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // suport
            CreateMap<Menu, MenuViewModel>();
            CreateMap<MenuViewModel, Menu>().ForMember(d => d.Id, o => o.Ignore());
        }
    }

}