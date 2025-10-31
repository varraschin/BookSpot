using Microsoft.AspNetCore.Identity;
using BookSpot.API.DTOs;
using BookSpot.API.Services.Interfaces;
using BookSpot.API.Models;
using BookSpot.API.Helpers;

namespace BookSpot.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly IFileService _fileService;

    public AuthService(
        UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager,
        IJwtService jwtService,
        IFileService fileService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _fileService = fileService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new ArgumentException("Email já está em uso.");
        }

        // Salvar a foto se existir
        string fotoPath = null;
        if (registerDto.Foto != null)
        {
            fotoPath = await _fileService.SaveFileAsync(registerDto.Foto, "img/usuarios");
        }

        var user = new Usuario
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            Nome = registerDto.Nome,
            DataNascimento = registerDto.DataNascimento,
            Foto = fotoPath
        };

        var result = await _userManager.CreateAsync(user, registerDto.Senha);
        if (!result.Succeeded)
        {
            if (fotoPath != null)
                await _fileService.DeleteFileAsync(fotoPath);

            var errors = string.Join(", ", result.Errors.Select(e => TranslateIdentityErrors.TranslateErrorMessage(e.Code)));
            throw new ArgumentException($"Falha ao criar usuário: {errors}");
        }

        await _userManager.AddToRoleAsync(user, "Cliente");

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Nome = user.Nome,
            DataNascimento = user.DataNascimento,
            Foto = fotoPath != null ? _fileService.GetFileUrl(fotoPath) : null,
            Perfil = string.Join(", ", await _userManager.GetRolesAsync(user))
        };
        var token = _jwtService.GenerateToken(userDto);

        return new AuthResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = userDto
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Usuário e/ou Senha Inválidos.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Senha, false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Usuário e/ou Senha Inválidos.");
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Nome = user.Nome,
            DataNascimento = user.DataNascimento,
            Foto = !string.IsNullOrEmpty(user.Foto) ? _fileService.GetFileUrl(user.Foto) : null,
            Perfil = string.Join(", ", await _userManager.GetRolesAsync(user))
        };
        var token = _jwtService.GenerateToken(userDto);

        return new AuthResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = userDto
        };
    }

    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("Usuário não encontrado.");
        }

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Nome = user.Nome,
            DataNascimento = user.DataNascimento,
            Foto = !string.IsNullOrEmpty(user.Foto) ? _fileService.GetFileUrl(user.Foto) : null,
            Perfil = string.Join(", ", await _userManager.GetRolesAsync(user))
        };
    }
}
