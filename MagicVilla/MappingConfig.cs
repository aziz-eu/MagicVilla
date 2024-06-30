using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.DTO;

namespace MagicVilla
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDto>().ReverseMap();
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        }

    }
}
