using System.Diagnostics;
using BookSpot.UI.DTOs;
using BookSpot.UI.Models;
using BookSpot.UI.Services.Interfaces;
using BookSpot.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSpot.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILojaService _lojaService;

    public HomeController(ILogger<HomeController> logger, ILojaService lojaService)
    {
        _logger = logger;
        _lojaService = lojaService;
    }

    // GET: Home/Index - Página inicial
    public async Task<IActionResult> Index()
    {
        try
        {
            var categorias = await _lojaService.ObterCategoriasAtivasAsync();
            var produtosDestaque = await _lojaService.ObterProdutosDestaqueAsync();
            
            var viewModel = new HomeVM
            {
                Categorias = categorias,
                ProdutosDestaque = produtosDestaque
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar página inicial");
            return View(new HomeVM());
        }
    }

    // GET: Home/Produtos - Lista de produtos
    public async Task<IActionResult> Produtos(int? categoriaId)
    {
        try
        {
            ViewBag.Categorias = await _lojaService.ObterCategoriasAtivasAsync();
            
            List<ProdutoDto> produtos;
            if (categoriaId.HasValue && categoriaId > 0)
            {
                produtos = await _lojaService.ObterProdutosPorCategoriaAsync(categoriaId.Value);
                ViewBag.CategoriaSelecionada = categoriaId.Value;
            }
            else
            {
                produtos = await _lojaService.ObterTodosProdutosAsync();
                ViewBag.CategoriaSelecionada = 0;
            }

            return View(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar produtos da loja");
            TempData["Erro"] = "Erro ao carregar produtos. Tente novamente.";
            return View(new List<ProdutoDto>());
        }
    }

    // GET: Home/Detalhes/5 - Detalhes do produto
    public async Task<IActionResult> Detalhes(int id)
    {
        try
        {
            var produto = await _lojaService.ObterProdutoPorIdAsync(id);
            if (produto == null)
            {
                TempData["Erro"] = "Produto não encontrado.";
                return RedirectToAction("Produtos");
            }

            return View(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar detalhes do produto {Id}", id);
            TempData["Erro"] = "Erro ao carregar produto.";
            return RedirectToAction("Produtos");
        }
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

