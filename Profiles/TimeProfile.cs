using AutoMapper;
using JogadoresApi.Dtos;
using JogadoresApi.Model;

namespace JogadoresApi.Profiles
{
    public class TimeProfile : Profile
    {
        public TimeProfile()
        {
            CreateMap<CriarTimeDto, Time>();
            CreateMap<AtualizarTimeDto, Time>();
            CreateMap<Time, ReadTimeDto>();
            CreateMap<Time, TimeResumoDto>();
            CreateMap<Jogador, JogadorResumoDto>();
        }
    }
}
