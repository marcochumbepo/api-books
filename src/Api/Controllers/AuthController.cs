using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsBooks.Application.DTOs;
using MsBooks.Application.Services;

namespace MsBooks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var response = _authService.Execute(request);
        return Ok(response);
    }
}
