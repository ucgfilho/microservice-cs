using Microsoft.EntityFrameworkCore;
using projetoAPI.Data;
using projetoAPI.DTOs;
using projetoAPI.Models;
using projetoAPI.Services.Interfaces;

namespace projetoAPI.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(AppDbContext context, ITokenService tokenService, IPasswordHasher passwordHasher)
    {
        _context = context;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResult> RegisterAsync(RegisterDTO dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return new AuthResult(false, ErrorMessage: "Este e-mail já está em uso.");

        var user = new User
        {
            Email = dto.Email,
            Password = _passwordHasher.Hash(dto.Password),
            Role = dto.Role.ToLower()
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new AuthResult(true);
    }

    public async Task<AuthResult> LoginAsync(LoginDTO dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return new AuthResult(false, ErrorMessage: "E-mail ou senha inválidos.");

        if (!_passwordHasher.Verify(dto.Password, user.Password))
            return new AuthResult(false, ErrorMessage: "E-mail ou senha inválidos.");

        var token = _tokenService.GenerateToken(user);
        return new AuthResult(true, Token: token);
    }
}
