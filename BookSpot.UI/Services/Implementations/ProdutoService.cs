using BookSpot.UI.DTOs;
using BookSpot.UI.Models;
using BookSpot.UI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace BookSpot.UI.Services.Implementations;

public class ProdutoService : BaseApiService, IProdutoService
{
    public ProdutoService(
        HttpClient httpClient,
        IOptions<ApiSettings> apiSettings,
        IHttpContextAccessor httpContextAccessor)
        : base(httpClient, apiSettings, httpContextAccessor)
    {
    }

    public async Task<List<ProdutoDto>> ObterTodosAsync()
    {
        return await GetAsync<List<ProdutoDto>>("produtos");
    }

    public async Task<ProdutoDto> ObterPorIdAsync(int id)
    {
        return await GetAsync<ProdutoDto>($"produtos/{id}");
    }

    public async Task<bool> CriarAsync(ProdutoDto produto)
    {
        try
        {
            var formData = new MultipartFormDataContent();
            AddProdutoToFormData(formData, produto);
            
            var response = await _httpClient.PostAsync("produtos", formData);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> CriarComFotoAsync(ProdutoDto produto, IFormFile foto)
    {
        try
        {
            var formData = new MultipartFormDataContent();
            AddProdutoToFormData(formData, produto);
            
            if (foto != null && foto.Length > 0)
            {
                var fileContent = new StreamContent(foto.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(foto.ContentType);
                formData.Add(fileContent, "Foto", foto.FileName);
            }

            var response = await _httpClient.PostAsync("produtos", formData);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AtualizarAsync(int id, ProdutoDto produto)
    {
        try
        {
            var formData = new MultipartFormDataContent();
            AddProdutoToFormData(formData, produto);
            formData.Add(new StringContent(id.ToString()), "Id");

            var response = await _httpClient.PutAsync($"produtos/{id}", formData);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AtualizarComFotoAsync(int id, ProdutoDto produto, IFormFile foto)
    {
        try
        {
            var formData = new MultipartFormDataContent();
            AddProdutoToFormData(formData, produto);
            formData.Add(new StringContent(id.ToString()), "Id");
            
            if (foto != null && foto.Length > 0)
            {
                var fileContent = new StreamContent(foto.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(foto.ContentType);
                formData.Add(fileContent, "Foto", foto.FileName);
            }

            var response = await _httpClient.PutAsync($"produtos/{id}", formData);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        return await DeleteAsync($"produtos/{id}");
    }

    private void AddProdutoToFormData(MultipartFormDataContent formData, ProdutoDto produto)
    {
        formData.Add(new StringContent(produto.CategoriaId.ToString()), "CategoriaId");
        formData.Add(new StringContent(produto.Nome ?? ""), "Nome");
        formData.Add(new StringContent(produto.Descricao ?? ""), "Descricao");
        formData.Add(new StringContent(produto.Qtde.ToString()), "Qtde");
        formData.Add(new StringContent(produto.ValorCusto.ToString()), "ValorCusto");
        formData.Add(new StringContent(produto.ValorVenda.ToString()), "ValorVenda");
        formData.Add(new StringContent(produto.Destaque.ToString()), "Destaque");
    }
}

