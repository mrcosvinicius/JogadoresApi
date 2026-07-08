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
        }
    }
}
