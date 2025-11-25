using AutoMapper;
using BookSpot.UI.DTOs;
using BookSpot.UI.ViewModels;

namespace BookSpot.UI;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        // Mapeamento para Categorias 
        CreateMap<CategoriaDto, CategoriaVM>()
            .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.Foto))
            .ForMember(dest => dest.Foto, opt => opt.Ignore());

        CreateMap<CategoriaVM, CategoriaDto>()
            .ForMember(dest => dest.Foto, opt => opt.Ignore());

        // Mapeamento para Produtos
        CreateMap<ProdutoDto, ProdutoVM>()
            .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.Foto))
            .ForMember(dest => dest.Foto, opt => opt.Ignore());

        CreateMap<ProdutoVM, ProdutoDto>()
            .ForMember(dest => dest.Foto, opt => opt.Ignore());
    }
}
