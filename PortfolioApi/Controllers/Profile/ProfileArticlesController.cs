using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Entities.CMS;
using PortfolioApi.EntityValueObject;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers.Profile;

[ApiVersion(1)]
[ApiController]
[Route("api/v{version:apiVersion}/profiles/{profileId:guid}/articles")]
public class ProfileArticlesController : ControllerBase
{
    private readonly IProfileArticle _profileArticle;

    public ProfileArticlesController(IProfileArticle profileArticle)
    {
        _profileArticle = profileArticle;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProducesEntity<CollectionType<Article>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProfileArticles(Guid profileId,
        [FromQuery] Pagination pagination)
    {
        var articles = await _profileArticle.GetProfileArticles(profileId, pagination);

        if (articles is ProducesEntityFail<CollectionType<Article>> fail)
        {
            return Problem(statusCode: fail.StatusCode, title: fail.Error, detail: fail.Description);
        }

        return Ok(articles.Entity);
    }
}