using Login_Service.DTOs.Request;
using Login_Service.DTOs.Response;
using Login_Service.Entities;

namespace Login_Service.Service.Interfaces;

public interface IUserService
{
    Task RegisterUser(RegisterRequest registerRequest);
    Task<LoginResponse> Login(LoginRequest loginRequest);
    Task<User> GetUserDetails(String email);
}