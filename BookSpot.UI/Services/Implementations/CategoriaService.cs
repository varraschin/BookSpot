using BookSpot.UI.DTOs;
using BookSpot.UI.Models;
using BookSpot.UI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BookSpot.UI.Services.Implementations;

public class CategoriaService : BaseApiService, ICategoriaService
{
    public CategoriaService(
        HttpClient httpClient,
        IOptions<ApiSettings> apiSettings,
        IHttpContextAccessor httpContextAccessor)
        : base(httpClient, apiSettings, httpContextAccessor)
    {
    }

    public async Task<List<CategoriaDto>> ObterTodasAsync()
    {
        return await GetAsync<List<CategoriaDto>>("categorias");
    }

    public async Task<CategoriaDto> ObterPorIdAsync(int id)
    {
        return await GetAsync<CategoriaDto>($"categorias/{id}");
    }

    public async Task<bool> CriarAsync(CategoriaDto categoria)
    {
        try
        {
            // Converter para FormData para enviar arquivo
            var formData = new MultipartFormDataContent
            {
                { new StringContent(categoria.Nome), "Nome" },
                { new StringContent(categoria.Cor), "Cor" }
            };

            var response = await _httpClient.PostAsync("categorias", formData);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> CriarComFotoAsync(CategoriaDto categoria, IFormFile foto)
    {
        try
        {
            var formData = new MultipartFormDataContent
            {
                { new StringContent(categoria.Nome), "Nome" },
                { new StringContent(categoria.Cor), "Cor" }
            };
            
            if (foto != null)
            {
                var fileContent = new StreamContent(foto.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(foto.ContentType);
                formData.Add(fileContent, "Foto", foto.FileName);
            }

            var response = await _httpClient.PostAsync("categorias", formData);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AtualizarAsync(int id, CategoriaDto categoria)
    {
        try
        {
            var formData = new MultipartFormDataContent
            {
                { new StringContent(id.ToString()), "Id" },
                { new StringContent(categoria.Nome), "Nome" },
                { new StringContent(categoria.Cor), "Cor" }
            };

            var response = await _httpClient.PutAsync($"categorias/{id}", formData);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AtualizarComFotoAsync(int id, CategoriaDto categoria, IFormFile foto)
    {
        try
        {
            var formData = new MultipartFormDataContent
            {
                { new StringContent(id.ToString()), "Id" },
                { new StringContent(categoria.Nome), "Nome" },
                { new StringContent(categoria.Cor), "Cor" }
            };
            
            if (foto != null)
            {
                var fileContent = new StreamContent(foto.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(foto.ContentType);
                formData.Add(fileContent, "Foto", foto.FileName);
            }

            var response = await _httpClient.PutAsync($"categorias/{id}", formData);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        return await DeleteAsync($"categorias/{id}");
    }
}
