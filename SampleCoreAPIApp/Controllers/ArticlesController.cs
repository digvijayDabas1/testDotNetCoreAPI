using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleCoreAPIApp.Models;
using SampleCoreAPIApp.Services;

[ApiController]
[Route("api/v1/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly ArticleServices _articleService;

    public ArticlesController(ArticleServices articleServices)
    {
        _articleService = articleServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetArticles()
    {
        var response = await _articleService.GetArticlesDetails();
        return Ok(response);
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetArticle(int id)
    {
        var response = await _articleService.GetArticlebyId(id);
        return Ok(response);
    }



    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateArticle(Article article)
    {
        var response = await _articleService.CreateArticleAsync(article);
        return Ok(response);
    }



    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateArticle(int id, Article updatedArticle)
    {
        var response = await _articleService.UpdateArticleById(id,updatedArticle, User.Identity.Name);
        return Ok(response);
    }



    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var response = await _articleService.DeleteArticleById(id, User.Identity.Name);
        return Ok(response);
    }
}