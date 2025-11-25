using BookSpot.UI.DTOs;
using BookSpot.UI.Models;
using BookSpot.UI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BookSpot.UI.Services.Implementations;

public class LojaService : BaseApiService, ILojaService
{
    public LojaService(
        HttpClient httpClient,
        IOptions<ApiSettings> apiSettings,
        IHttpContextAccessor httpContextAccessor)
        : base(httpClient, apiSettings, httpContextAccessor)
    {
    }

    public async Task<List<CategoriaDto>> ObterCategoriasAtivasAsync()
    {
        return await GetAsync<List<CategoriaDto>>("categorias");
    }

    public async Task<List<ProdutoDto>> ObterProdutosDestaqueAsync()
    {
        return await GetAsync<List<ProdutoDto>>("produtos/destaque");
    }

    public async Task<List<ProdutoDto>> ObterTodosProdutosAsync()
    {
        return await GetAsync<List<ProdutoDto>>("produtos");
    }

    public async Task<List<ProdutoDto>> ObterProdutosPorCategoriaAsync(int categoriaId)
    {
        return await GetAsync<List<ProdutoDto>>($"produtos/categoria/{categoriaId}");
    }

    public async Task<ProdutoDto> ObterProdutoPorIdAsync(int id)
    {
        return await GetAsync<ProdutoDto>($"produtos/{id}");
    }
}
