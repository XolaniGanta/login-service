using System.Security.Claims;
using Login_Service.DTOs.Request;
using Login_Service.DTOs.Response;
using Login_Service.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Login_Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            await _userService.RegisterUser(request);
            return Ok(new { message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [Authorize]
    [HttpGet("userDetails")]
    public async Task<IActionResult> GetUserDetails()
    {
        var email = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(email))
            return Unauthorized();
        var user = await _userService.GetUserDetails(email);
        var response = new UserResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
        return Ok(response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _userService.Login(request);
            var response = new LoginResponse
            {
                Token = token.Token
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Return 401 for invalid credentials
            return Unauthorized(new { error = ex.Message });
        }
    }
}
