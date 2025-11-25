using BookSpot.UI.DTOs;

namespace BookSpot.UI.Services.Interfaces;

public interface IProdutoService
{
    Task<List<ProdutoDto>> ObterTodosAsync();
    Task<ProdutoDto> ObterPorIdAsync(int id);
    Task<bool> CriarAsync(ProdutoDto produto);
    Task<bool> CriarComFotoAsync(ProdutoDto produto, IFormFile foto);
    Task<bool> AtualizarAsync(int id, ProdutoDto produto);
    Task<bool> AtualizarComFotoAsync(int id, ProdutoDto produto, IFormFile foto);
    Task<bool> ExcluirAsync(int id);
}

