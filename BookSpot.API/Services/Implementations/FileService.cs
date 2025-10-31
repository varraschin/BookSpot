using BookSpot.API.Services.Interfaces;

namespace BookSpot.API.Services.Implementations;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FileService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string subDirectory)
    {
        if (file == null || file.Length == 0)
            return null;

        // Criar diretório se não existir
        var uploadsPath = Path.Combine(_environment.WebRootPath, subDirectory);
        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        // Gerar nome único para o arquivo
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(subDirectory, fileName); // Salva o path relativo
        var fullPath = Path.Combine(_environment.WebRootPath, filePath);

        // Salvar arquivo
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath; // Retorna o path relativo: "img/usuarios/arquivo.png"
    }

    public async Task<bool> DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return false;

        var fullPath = Path.Combine(_environment.WebRootPath, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }
        return false;
    }

    public string GetFileUrl(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return null;

        var request = _httpContextAccessor.HttpContext.Request;
        return $"{request.Scheme}://{request.Host}/{filePath.Replace("\\", "/")}";
    }
}
