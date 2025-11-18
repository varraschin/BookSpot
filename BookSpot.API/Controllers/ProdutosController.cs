using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSpot.API.Data;
using BookSpot.API.Models;
using BookSpot.API.Services.Interfaces;
using BookSpot.API.DTOs;

namespace BookSpot.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IFileService _fileService;

    public ProdutosController(AppDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    // GET: api/Produtos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos()
    {
        var produtos = await _context.Produtos
            .Include(p => p.Categoria)
            .ToListAsync();

        var produtosDto = produtos.Select(p => MapToDto(p)).ToList();
        return Ok(produtosDto);
    }

    // GET: api/Produtos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoDto>> GetProduto(int id)
    {
        var produto = await _context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (produto == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(produto));
    }

    // GET: api/Produtos/categoria/5
    [HttpGet("categoria/{categoriaId}")]
    public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutosPorCategoria(int categoriaId)
    {
        var produtos = await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId)
            .ToListAsync();

        var produtosDto = produtos.Select(p => MapToDto(p)).ToList();
        return Ok(produtosDto);
    }

    // GET: api/Produtos/destaque
    [HttpGet("destaque")]
    public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutosDestaque()
    {
        var produtos = await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Destaque)
            .ToListAsync();

        var produtosDto = produtos.Select(p => MapToDto(p)).ToList();
        return Ok(produtosDto);
    }

    // PUT: api/Produtos/5
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> PutProduto(int id, [FromForm] ProdutoUpdateDto produtoDto)
    {
        if (id != produtoDto.Id)
        {
            return BadRequest();
        }

        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
        {
            return NotFound();
        }

        // Atualizar propriedades
        produto.CategoriaId = produtoDto.CategoriaId;
        produto.Nome = produtoDto.Nome;
        produto.Descricao = produtoDto.Descricao;
        produto.Qtde = produtoDto.Qtde;
        produto.ValorCusto = produtoDto.ValorCusto;
        produto.ValorVenda = produtoDto.ValorVenda;
        produto.Destaque = produtoDto.Destaque;

        // Processar nova foto se fornecida
        if (produtoDto.Foto != null && produtoDto.Foto.Length > 0)
        {
            // Deletar foto antiga se existir
            if (!string.IsNullOrEmpty(produto.Foto))
            {
                await _fileService.DeleteFileAsync(produto.Foto);
            }

            // Salvar nova foto
            produto.Foto = await _fileService.SaveFileAsync(produtoDto.Foto, "img\\produtos");
        }

        _context.Entry(produto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProdutoExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok(MapToDto(produto));
    }

    // POST: api/Produtos
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ProdutoDto>> PostProduto([FromForm] ProdutoCreateDto produtoDto)
    {
        var produto = new Produto
        {
            CategoriaId = produtoDto.CategoriaId,
            Nome = produtoDto.Nome,
            Descricao = produtoDto.Descricao,
            Qtde = produtoDto.Qtde,
            ValorCusto = produtoDto.ValorCusto,
            ValorVenda = produtoDto.ValorVenda,
            Destaque = produtoDto.Destaque
        };

        // Salvar foto se fornecida
        if (produtoDto.Foto != null && produtoDto.Foto.Length > 0)
        {
            produto.Foto = await _fileService.SaveFileAsync(produtoDto.Foto, "img\\produtos");
        }

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        // Carregar categoria para incluir no DTO
        await _context.Entry(produto)
            .Reference(p => p.Categoria)
            .LoadAsync();

        return CreatedAtAction("GetProduto", new { id = produto.Id }, MapToDto(produto));
    }

    // DELETE: api/Produtos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
        {
            return NotFound();
        }

        // Deletar foto associada se existir
        if (!string.IsNullOrEmpty(produto.Foto))
        {
            await _fileService.DeleteFileAsync(produto.Foto);
        }

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProdutoExists(int id)
    {
        return _context.Produtos.Any(e => e.Id == id);
    }

    private ProdutoDto MapToDto(Produto produto)
    {
        return new ProdutoDto
        {
            Id = produto.Id,
            CategoriaId = produto.CategoriaId,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Qtde = produto.Qtde,
            ValorCusto = produto.ValorCusto,
            ValorVenda = produto.ValorVenda,
            Destaque = produto.Destaque,
            Foto = !string.IsNullOrEmpty(produto.Foto) ? _fileService.GetFileUrl(produto.Foto) : null,
            CategoriaNome = produto.Categoria?.Nome
        };
    }
}

