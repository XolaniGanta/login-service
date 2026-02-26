using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Login_Service.DTOs.Request;
using Login_Service.DTOs.Response;
using Login_Service.Entities;
using Login_Service.Repository;
using Login_Service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Login_Service.Service;

public class UserService : IUserService
{
    private readonly UserDb _db;
    private readonly IConfiguration _configuration;
    
    public UserService(UserDb db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }
    
    
    public async Task RegisterUser(RegisterRequest registerRequest)
    {
        var email = registerRequest.Email.Trim().ToLowerInvariant();
        
        bool exists = await _db.users.AnyAsync(u => u.Email.ToLower() == email);
        
        if (exists) throw new Exception("User with this email already exists.");
        
        var passwordHashed = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

        var user = new User
        {
            FirstName = registerRequest.FirstName.Trim(),
            LastName = registerRequest.LastName.Trim(),
            Email = email,
            PasswordHash = passwordHashed,
        };
        
        await _db.users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest)
    {
        var email = loginRequest.Email.Trim().ToLowerInvariant();
        var user = await _db.users
            .FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || 
            !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
        {
            throw new Exception("Invalid credentials");
        }

        var response = new LoginResponse {
            Token = GenerateJwtToken(user)
        };

        return response;
    }

    public async Task<User> GetUserDetails(string email)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        var user = await _db.users
            .FirstOrDefaultAsync(u => u.Email == normalizedEmail);
        return user ?? throw new Exception("User not found.");
    }
    
    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(jwtSettings["ExpiryMinutes"])),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}