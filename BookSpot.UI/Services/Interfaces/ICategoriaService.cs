using BookSpot.UI.DTOs;

namespace BookSpot.UI.Services.Interfaces;

public interface ICategoriaService
{
    Task<List<CategoriaDto>> ObterTodasAsync();
    Task<CategoriaDto> ObterPorIdAsync(int id);
    Task<bool> CriarAsync(CategoriaDto categoria);
    Task<bool> CriarComFotoAsync(CategoriaDto categoria, IFormFile foto);
    Task<bool> AtualizarAsync(int id, CategoriaDto categoria);
    Task<bool> AtualizarComFotoAsync(int id, CategoriaDto categoria, IFormFile foto);
    Task<bool> ExcluirAsync(int id);
}

