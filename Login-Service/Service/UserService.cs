using Login_Service.DTOs.Request;
using Login_Service.DTOs.Response;
using Login_Service.Entities;
using Login_Service.Repository;
using Login_Service.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Login_Service.Service;

public class UserService : IUserService
{
    private readonly UserDb _db;
    
    public UserService(UserDb db)
    {
        _db = db;
    }
    
    public async Task RegisterUser(RegisterRequest registerRequest)
    {
        var email = registerRequest.Email.Trim().ToLowerInvariant();
        
        bool exists = await _db.Users.AnyAsync(u => u.Email.ToLower() == email);
        
        if (exists) throw new Exception("User with this email already exists.");
        
        var passwordHashed = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

        var user = new User
        {
            FirstName = registerRequest.FirstName.Trim(),
            LastName = registerRequest.LastName.Trim(),
            Email = email,
            PasswordHash = passwordHashed,
        };
        
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public Task<LoginResponse> Login(LoginRequest loginRequest)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserDetails(string email)
    {
        throw new NotImplementedException();
    }
}