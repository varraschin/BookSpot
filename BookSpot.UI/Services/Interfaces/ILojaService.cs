using BookSpot.UI.DTOs;

namespace BookSpot.UI.Services.Interfaces;

public interface ILojaService
{
    Task<List<CategoriaDto>> ObterCategoriasAtivasAsync();
    Task<List<ProdutoDto>> ObterProdutosDestaqueAsync();
    Task<List<ProdutoDto>> ObterTodosProdutosAsync();
    Task<List<ProdutoDto>> ObterProdutosPorCategoriaAsync(int categoriaId);
    Task<ProdutoDto> ObterProdutoPorIdAsync(int id);
}
