using AutoMapper;
using JogadoresApi.Dtos;
using JogadoresApi.Model;

namespace JogadoresApi.Profiles
{
    public class LigaProfile : Profile
    {
        public LigaProfile()
        {
            CreateMap<CriarLigaDto, Liga>();
            CreateMap<AtualizarLigaDto, Liga>();
        }
    }
}
