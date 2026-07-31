using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Entities.CMS;
using PortfolioApi.EntityValueObject;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers.Profile;

[ApiVersion(1)]
[ApiController]
[Route("api/v{version:apiVersion}/profiles/{profileId:guid}/learnings")]
public class ProfileLearningsController : ControllerBase
{
    private readonly IProfileLearning _profileLearning;

    public ProfileLearningsController(IProfileLearning profileLearning)
    {
        _profileLearning = profileLearning;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProducesEntity<CollectionType<Learn>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProfileLearnings(Guid profileId,
        [FromQuery] Pagination pagination)
    {
        var articles = await _profileLearning.GetProfileLearnings(profileId, pagination);

        if (articles is ProducesEntityFail<CollectionType<Learn>> fail)
        {
            return Problem(statusCode: fail.StatusCode, title: fail.Error, detail: fail.Description);
        }

        return Ok(articles.Entity);
    }
}