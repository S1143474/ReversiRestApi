using AutoMapper;
using Reversi.API.Application.Common.Mappings;
using Reversi.API.Application.Spellen.Commands.InProcessSpelMove.MoveModels;
using Reversi.API.DataTransferObjects;
using Reversi.API.DataTransferObjects.Move;
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


            CreateMap<FinishedModel, FinishedDto>()
                .IncludeBase<BaseMoveModel ,BaseDto>()
                .ForMember(dest => dest.IsDraw,
                    opt => opt.MapFrom(src => src.IsSpelDraw));

            CreateMap<CoordsModel, CoordsDto>();

            CreateMap<MoveExecutedModel, MoveExecutedDto>()
                .IncludeBase<BaseMoveModel, BaseDto>()
                .ForMember(dest => dest.AanDeBeurt,
                    opt => opt.MapFrom(src => src.PlayerTurn))
                .ForMember(dest => dest.IsPlaceExecuted,
                    opt => opt.MapFrom(src => src.IsMovementExecuted))
                .ForMember(dest => dest.FichesToTurnAround,
                    opt => opt.MapFrom(src => src.CoordsToTurnAround));

            CreateMap<BaseMoveModel, BaseDto>()
                .IncludeAllDerived();

            CreateMap<Spel, SpelFinishedDto>()
                .IncludeBase<Spel, SpelDto>()
                .ForMember(dest => dest.GameStartedAt,
                    opt => opt.MapFrom(src => src.StartedAt))
                .ForMember(dest => dest.GameFinishedAt,
                    opt => opt.MapFrom(src => src.FinishedAt))
                .ForMember(dest => dest.GameWonBy,
                    opt => opt.MapFrom(src => src.WonBy))
                .ForMember(dest => dest.GameLostBy,
                    opt => opt.MapFrom(src => src.LostBy))
                .ForMember(dest => dest.AmountOfFichesFlippedByPlayer1,
                    opt => opt.MapFrom(src => src.AOFFBySpeler1))
                .ForMember(dest => dest.AmountOfFichesFlippedByPlayer2,
                    opt => opt.MapFrom(src => src.AOFFBySpeler2));
        }
    }
}
