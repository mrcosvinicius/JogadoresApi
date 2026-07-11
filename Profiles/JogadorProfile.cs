using AutoMapper;
using JogadoresApi.Dtos;
using JogadoresApi.Model;

namespace JogadoresApi.Profiles
{
    public class JogadorProfile : Profile
    {
        public JogadorProfile()
        {
            CreateMap<CriarJogadorDto, Jogador>();
            CreateMap<AtualizarJogadorDto, Jogador>();
            CreateMap<Jogador, ReadJogadorDto>()
                .ForMember(destino => destino.Time, opt => opt.MapFrom(origem => origem.Time != null ? origem.Time.Nome : null));
            CreateMap<Jogador, JogadorResumoDto>();
        }
    }
}
