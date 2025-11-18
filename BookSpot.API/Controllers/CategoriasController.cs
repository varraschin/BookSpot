using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSpot.API.Data;
using BookSpot.API.DTOs;
using BookSpot.API.Models;
using BookSpot.API.Services.Interfaces;

namespace BookSpot.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IFileService _fileService;

    public CategoriasController(AppDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    // GET: api/Categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
    {
        var categorias = await _context.Categorias.ToListAsync();
        
        // Converter caminhos relativos em URLs completas
        foreach (var categoria in categorias)
        {
            if (!string.IsNullOrEmpty(categoria.Foto))
            {
                categoria.Foto = _fileService.GetFileUrl(categoria.Foto);
            }
        }
        
        return categorias;
    }

    // GET: api/Categorias/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetCategoria(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null)
        {
            return NotFound();
        }

        // Converter caminho relativo em URL completa
        if (!string.IsNullOrEmpty(categoria.Foto))
        {
            categoria.Foto = _fileService.GetFileUrl(categoria.Foto);
        }

        return categoria;
    }

    // PUT: api/Categorias/5
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> PutCategoria(int id, [FromForm] CategoriaUpdateDto categoriaDto)
    {
        if (id != categoriaDto.Id)
        {
            return BadRequest();
        }

        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }

        // Atualizar propriedades básicas
        categoria.Nome = categoriaDto.Nome;
        categoria.Cor = categoriaDto.Cor;

        // Processar nova foto se fornecida
        if (categoriaDto.Foto != null && categoriaDto.Foto.Length > 0)
        {
            // Deletar foto antiga se existir
            if (!string.IsNullOrEmpty(categoria.Foto))
            {
                await _fileService.DeleteFileAsync(categoria.Foto);
            }

            // Salvar nova foto
            categoria.Foto = await _fileService.SaveFileAsync(categoriaDto.Foto, "img\\categorias");
        }

        _context.Entry(categoria).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoriaExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        // Retornar categoria com URL completa
        if (!string.IsNullOrEmpty(categoria.Foto))
        {
            categoria.Foto = _fileService.GetFileUrl(categoria.Foto);
        }

        return Ok(categoria);
    }

    // POST: api/Categorias
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<Categoria>> PostCategoria([FromForm] CategoriaCreateDto categoriaDto)
    {
        var categoria = new Categoria
        {
            Nome = categoriaDto.Nome,
            Cor = categoriaDto.Cor ?? "rgba(0,0,0,1)"
        };

        // Salvar foto se fornecida
        if (categoriaDto.Foto != null && categoriaDto.Foto.Length > 0)
        {
            categoria.Foto = await _fileService.SaveFileAsync(categoriaDto.Foto, "img\\categorias");
        }

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        // Retornar categoria com URL completa
        if (!string.IsNullOrEmpty(categoria.Foto))
        {
            categoria.Foto = _fileService.GetFileUrl(categoria.Foto);
        }

        return CreatedAtAction("GetCategoria", new { id = categoria.Id }, categoria);
    }

    // DELETE: api/Categorias/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }

        // Deletar foto associada se existir
        if (!string.IsNullOrEmpty(categoria.Foto))
        {
            await _fileService.DeleteFileAsync(categoria.Foto);
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoriaExists(int id)
    {
        return _context.Categorias.Any(e => e.Id == id);
    }
}

