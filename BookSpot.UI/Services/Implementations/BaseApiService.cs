using System.Text;
using System.Text.Json;
using BookSpot.UI.Models;
using Microsoft.Extensions.Options;

namespace BookSpot.UI.Services.Implementations;

public abstract class BaseApiService
{
    protected readonly HttpClient _httpClient;
    protected readonly ApiSettings _apiSettings;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected BaseApiService(
        HttpClient httpClient,
        IOptions<ApiSettings> apiSettings,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings.Value;
        _httpContextAccessor = httpContextAccessor;

        _httpClient.BaseAddress = new Uri(_apiSettings.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds);
    }

    protected async Task<T> GetAsync<T>(string endpoint)
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync(endpoint);
        return await HandleResponse<T>(response);
    }

    protected async Task<T> PostAsync<T>(string endpoint, object data)
    {
        AddAuthHeader();
        var content = CreateJsonContent(data);
        var response = await _httpClient.PostAsync(endpoint, content);
        return await HandleResponse<T>(response);
    }

    protected async Task<T> PutAsync<T>(string endpoint, object data)
    {
        AddAuthHeader();
        var content = CreateJsonContent(data);
        var response = await _httpClient.PutAsync(endpoint, content);
        return await HandleResponse<T>(response);
    }

    protected async Task<bool> DeleteAsync(string endpoint)
    {
        AddAuthHeader();
        var response = await _httpClient.DeleteAsync(endpoint);
        return response.IsSuccessStatusCode;
    }

    private void AddAuthHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    private StringContent CreateJsonContent(object data)
    {
        var json = JsonSerializer.Serialize(data);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private async Task<T> HandleResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<T>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Tenta desserializar a resposta de erro da API
        try
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Se conseguiu desserializar, usa a mensagem do erro
            var errorMessage = errorResponse?.Message ?? $"Erro: {response.StatusCode}";

            // Para erros 401, faz logout automático
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _httpContextAccessor.HttpContext?.Session.Clear();
            }

            throw new HttpRequestException(errorMessage);
        }
        catch (JsonException)
        {
            // Se não conseguir desserializar, usa a mensagem padrão
            throw new HttpRequestException($"Erro {response.StatusCode}: {content}");
        }
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}