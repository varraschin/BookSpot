using AutoMapper;
using BookSpot.UI.DTOs;
using BookSpot.UI.Services.Interfaces;
using BookSpot.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSpot.UI.Controllers;

[Authorize(Roles = "Administrador")]
public class ProdutosController : Controller
{
    private readonly IProdutoService _produtoService;
    private readonly ICategoriaService _categoriaService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(
        IProdutoService produtoService,
        ICategoriaService categoriaService,
        IMapper mapper,
        ILogger<ProdutosController> logger)
    {
        _produtoService = produtoService;
        _categoriaService = categoriaService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET: Produtos
    public async Task<IActionResult> Index()
    {
        try
        {
            var produtos = await _produtoService.ObterTodosAsync();
            var produtosVM = _mapper.Map<List<ProdutoVM>>(produtos);
            return View(produtosVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar produtos");
            TempData["Erro"] = "Erro ao carregar produtos. Tente novamente.";
            return View(new List<ProdutoVM>());
        }
    }

    // GET: Produtos/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null)
            {
                TempData["Erro"] = "Produto não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var produtoVM = _mapper.Map<ProdutoVM>(produto);
            return View(produtoVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar produto {Id}", id);
            TempData["Erro"] = "Erro ao carregar produto.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Produtos/Create
    public async Task<IActionResult> Create()
    {
        await CarregarCategoriasViewBag();
        return View();
    }

    // POST: Produtos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProdutoVM produtoVM)
    {
        if (!ModelState.IsValid)
        {
            await CarregarCategoriasViewBag();
            return View(produtoVM);
        }

        try
        {
            var produtoDto = new ProdutoDto
            {
                CategoriaId = produtoVM.CategoriaId,
                Nome = produtoVM.Nome,
                Descricao = produtoVM.Descricao,
                Qtde = produtoVM.Qtde,
                ValorCusto = produtoVM.ValorCusto,
                ValorVenda = produtoVM.ValorVenda,
                Destaque = produtoVM.Destaque
            };

            bool success;

            if (produtoVM.Foto != null && produtoVM.Foto.Length > 0)
            {
                success = await _produtoService.CriarComFotoAsync(produtoDto, produtoVM.Foto);
            }
            else
            {
                success = await _produtoService.CriarAsync(produtoDto);
            }

            if (success)
            {
                TempData["Sucesso"] = "Produto criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Erro ao criar produto.");
            await CarregarCategoriasViewBag();
            return View(produtoVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar produto");
            ModelState.AddModelError("", "Erro interno ao criar produto.");
            await CarregarCategoriasViewBag();
            return View(produtoVM);
        }
    }

    // GET: Produtos/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null)
            {
                TempData["Erro"] = "Produto não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var produtoVM = _mapper.Map<ProdutoVM>(produto);
            await CarregarCategoriasViewBag();
            return View(produtoVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar produto para edição {Id}", id);
            TempData["Erro"] = "Erro ao carregar produto.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Produtos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProdutoVM produtoVM)
    {
        if (id != produtoVM.Id)
        {
            TempData["Erro"] = "Produto inválido.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            await CarregarCategoriasViewBag();
            return View(produtoVM);
        }

        try
        {
            var produtoDto = new ProdutoDto
            {
                Id = produtoVM.Id,
                CategoriaId = produtoVM.CategoriaId,
                Nome = produtoVM.Nome,
                Descricao = produtoVM.Descricao,
                Qtde = produtoVM.Qtde,
                ValorCusto = produtoVM.ValorCusto,
                ValorVenda = produtoVM.ValorVenda,
                Destaque = produtoVM.Destaque
            };

            bool success;

            if (produtoVM.Foto != null && produtoVM.Foto.Length > 0)
            {
                success = await _produtoService.AtualizarComFotoAsync(id, produtoDto, produtoVM.Foto);
            }
            else
            {
                success = await _produtoService.AtualizarAsync(id, produtoDto);
            }

            if (success)
            {
                TempData["Sucesso"] = "Produto atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Erro ao atualizar produto.");
            await CarregarCategoriasViewBag();
            return View(produtoVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar produto {Id}", id);
            ModelState.AddModelError("", "Erro interno ao atualizar produto.");
            await CarregarCategoriasViewBag();
            return View(produtoVM);
        }
    }

    // GET: Produtos/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null)
            {
                TempData["Erro"] = "Produto não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var produtoVM = _mapper.Map<ProdutoVM>(produto);
            return View(produtoVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar produto para exclusão {Id}", id);
            TempData["Erro"] = "Erro ao carregar produto.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Produtos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var success = await _produtoService.ExcluirAsync(id);

            if (success)
            {
                TempData["Sucesso"] = "Produto excluído com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao excluir produto.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir produto {Id}", id);
            TempData["Erro"] = "Erro interno ao excluir produto.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CarregarCategoriasViewBag()
    {
        try
        {
            var categorias = await _categoriaService.ObterTodasAsync();
            ViewBag.Categorias = categorias;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar categorias para ViewBag");
            ViewBag.Categorias = new List<CategoriaDto>();
        }
    }
}
