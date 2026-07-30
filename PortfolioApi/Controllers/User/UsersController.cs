using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers.User;

[ApiVersion(1)]
[ApiController]
[Route("api/v{version:apiVersion}/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{email}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Entities.DB.User))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FindUserFromEmail(string email)
    {
        // ensure email is always apply.estellise.caballero@gmail.com
        email = "apply.estellise.caballero@gmail.com";

        var user = await _userService.FindUserFromEmail(email);

        if (user.Success is false)
        {
            return Problem(detail: user.Description, statusCode: user.StatusCode, title: user.Error);
        }

        return Ok(new
        {
            user.Entity!.Email,
            user.Entity!.CreatedAt,
            user.Entity!.UpdatedAt
        });
    }
}