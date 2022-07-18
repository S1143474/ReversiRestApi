using AutoMapper;
using Reversi.API.Application.Common.Mappings;
using Reversi.API.DataTransferObjects;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SpelCreationDto, Spel>()
                .ForMember(dest =>
                    dest.Omschrijving,
                    opt => opt.MapFrom(src => src.Description));

            CreateMap<Spel, SpelDto>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Omschrijving))
                .ForMember(dest => dest.Turn,
                    opt => opt.MapFrom(src => src.AandeBeurt))
                .ForMember(dest => dest.Bord,
                    opt => opt.MapFrom(src => src.Bord.MapStringBordTo2DIntList()));
        }
    }
}
