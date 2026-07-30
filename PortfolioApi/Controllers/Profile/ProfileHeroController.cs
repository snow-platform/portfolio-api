using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Entities.DB;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers.Profile;

[ApiVersion(1)]
[ApiController]
[Route("api/v{version:apiVersion}/profiles/{profileId:guid}/hero")]
public class ProfileHeroController : ControllerBase
{
    private readonly IProfileHeroService _profileHeroService;

    public ProfileHeroController(IProfileHeroService profileHeroService)
    {
        _profileHeroService = profileHeroService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProfileHero), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileHero(Guid profileId)
    {
        var profileHero = await _profileHeroService.GetProfileHero(profileId);

        if (profileHero is ProducesEntityFail<ProfileHero> fail)
        {
            return Problem(statusCode: fail.StatusCode, title: fail.Error, detail: fail.Description);
        }

        return Ok(profileHero.Entity);
    }
}