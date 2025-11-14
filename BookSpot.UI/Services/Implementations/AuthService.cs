using System.Text;
using System.Text.Json;
using BookSpot.UI.DTOs;
using BookSpot.UI.Models;
using BookSpot.UI.Services.Interfaces;
using BookSpot.UI.ViewModels;
using Microsoft.Extensions.Options;

namespace BookSpot.UI.Services.Implementations;

public class AuthService : BaseApiService, IAuthService
{
    public AuthService(
        HttpClient httpClient,
        IOptions<ApiSettings> apiSettings,
        IHttpContextAccessor httpContextAccessor)
        : base(httpClient, apiSettings, httpContextAccessor)
    {
    }

    public async Task<(bool Success, string Message)> LoginAsync(LoginVM loginVM)
    {
        try
        {
            // Remove o header de auth se existir (para login)
            _httpClient.DefaultRequestHeaders.Authorization = null;

            var response = await PostAsync<AuthResponseDto>("auth/login", loginVM);

            if (response != null && !string.IsNullOrEmpty(response.Token))
            {
                // Salva o token na sessão
                _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", response.Token);
                _httpContextAccessor.HttpContext?.Session.SetString("UserEmail", response.User.Email);
                _httpContextAccessor.HttpContext?.Session.SetString("UserName", response.User.Nome);
                _httpContextAccessor.HttpContext?.Session.SetString("UserId", response.User.Id);
                if (response.User.DataNascimento.HasValue)
                {
                    _httpContextAccessor.HttpContext?.Session.SetString("UserDataNascimento",
                        response.User.DataNascimento.Value.ToString("yyyy-MM-dd"));
                }
                _httpContextAccessor.HttpContext?.Session.SetString("UserFoto", response.User.Foto ?? "");
                _httpContextAccessor.HttpContext?.Session.SetString("UserPerfil", response.User.Perfil);
                return (true, "Login realizado com sucesso!");
            }
            return (false, "Credenciais inválidas.");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro no login: {ex.Message}");
            return (false, ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado no login: {ex.Message}");
            return (false, "Erro interno. Tente novamente.");
        }
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegistroVM registroVM)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            var formData = new MultipartFormDataContent();

            // Campos obrigatórios
            formData.Add(new StringContent(registroVM.Nome ?? ""), "Nome");
            formData.Add(new StringContent(registroVM.Email ?? ""), "Email");
            formData.Add(new StringContent(registroVM.Senha ?? ""), "Senha");

            // Campo opcional
            if (registroVM.DataNascimento.HasValue)
            {
                formData.Add(new StringContent(registroVM.DataNascimento.Value.ToString("yyyy-MM-dd")), "DataNascimento");
            }

            // Foto (opcional) - se tiver arquivo, envia, senão não
            if (registroVM.Foto != null && registroVM.Foto.Length > 0)
            {
                var fileContent = new StreamContent(registroVM.Foto.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(registroVM.Foto.ContentType);
                formData.Add(fileContent, "Foto", registroVM.Foto.FileName);
            }

            var response = await _httpClient.PostAsync("auth/register", formData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
                {
                    // Salva o token e dados do usuário na sessão
                    _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", authResponse.Token);
                    _httpContextAccessor.HttpContext?.Session.SetString("UserEmail", authResponse.User.Email);
                    _httpContextAccessor.HttpContext?.Session.SetString("UserName", authResponse.User.Nome);
                    _httpContextAccessor.HttpContext?.Session.SetString("UserId", authResponse.User.Id);

                    if (authResponse.User.DataNascimento.HasValue)
                    {
                        _httpContextAccessor.HttpContext?.Session.SetString("UserDataNascimento",
                            authResponse.User.DataNascimento.Value.ToString("yyyy-MM-dd"));
                    }
                    _httpContextAccessor.HttpContext?.Session.SetString("UserFoto", authResponse.User.Foto ?? "");
                    _httpContextAccessor.HttpContext?.Session.SetString("UserPerfil", authResponse.User.Perfil);

                    return (true, "Registro realizado com sucesso!");
                }
            }

            // Tratamento de erro
            var errorContent = await response.Content.ReadAsStringAsync();
            string errorMessage;

            try
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                errorMessage = errorResponse?.Message ?? "Falha no registro.";
            }
            catch
            {
                errorMessage = $"Erro {response.StatusCode}: {errorContent}";
            }

            return (false, errorMessage);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro no registro: {ex.Message}");
            return (false, ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado no registro: {ex.Message}");
            return (false, "Erro interno. Tente novamente.");
        }
    }

    public void Logout()
    {
        _httpContextAccessor.HttpContext?.Session.Clear();
    }

    public bool IsAuthenticated()
    {
        return !string.IsNullOrEmpty(GetUserToken());
    }

    public string GetUserToken()
    {
        return _httpContextAccessor.HttpContext?.Session.GetString("JWTToken") ?? string.Empty;
    }

    public string GetUserName()
    {
        return _httpContextAccessor.HttpContext?.Session.GetString("UserName") ?? string.Empty;
    }

    public string GetUserEmail()
    {
        return _httpContextAccessor.HttpContext?.Session.GetString("UserEmail") ?? string.Empty;
    }

    public string GetUserFoto()
    {
        return _httpContextAccessor.HttpContext?.Session.GetString("UserFoto") ?? string.Empty;
    }

    public string GetUserPerfil()
    {
        return _httpContextAccessor.HttpContext?.Session.GetString("UserPerfil") ?? string.Empty;
    }
}