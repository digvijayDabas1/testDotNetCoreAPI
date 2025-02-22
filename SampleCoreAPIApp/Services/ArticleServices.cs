using Microsoft.EntityFrameworkCore;
using SampleCoreAPIApp.DBContext;
using SampleCoreAPIApp.IServices;
using SampleCoreAPIApp.Models;
using SampleCoreAPIApp.Models.ResponseModels;

namespace SampleCoreAPIApp.Services
{
    public class ArticleServices:IArticleServices
	{
        private readonly SampleTempDBContext _sampleTempDBContext;
        private readonly ILogger<UserServices> _logger;

        public ArticleServices(
            SampleTempDBContext sampleTempDBContext,
            ILogger<UserServices> logger)
        {
            _sampleTempDBContext = sampleTempDBContext;
            _logger = logger;
        }

        public async Task<CommonResponseModel> GetArticlesDetails()
        {
            CommonResponseModel commonResponseModel = new();
            try
            {
                var articles = await _sampleTempDBContext.Articles.Include(a => a.User).ToListAsync();
                if(articles != null)
                {
                    commonResponseModel.Data = articles;
                    commonResponseModel.Message = "Article get successfully";
                    commonResponseModel.StatusCode = StatusCodes.Status200OK;
                    commonResponseModel.Status = true;
                }
                else
                {
                    commonResponseModel.Data = null;
                    commonResponseModel.Message = "Data not found!";
                    commonResponseModel.StatusCode = StatusCodes.Status404NotFound;
                    commonResponseModel.Status = false;
                }
                return commonResponseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return commonResponseModel;
            }
        }

        public async Task<CommonResponseModel> GetArticlebyId(int id)
        {
            CommonResponseModel commonResponseModel = new();
            try
            {
                var article = await _sampleTempDBContext.Articles.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
                if (article == null)
                {
                    commonResponseModel.Data = null;
                    commonResponseModel.Message = "Data not found!";
                    commonResponseModel.StatusCode = StatusCodes.Status404NotFound;
                    commonResponseModel.Status = false;
                }
                else
                {
                    commonResponseModel.Data = article;
                    commonResponseModel.Message = "Article get successfully";
                    commonResponseModel.StatusCode = StatusCodes.Status200OK;
                    commonResponseModel.Status = true;
                }
                return commonResponseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return commonResponseModel;
            }
        }


        public async Task<CommonResponseModel> CreateArticleAsync(Article article)
        {
            CommonResponseModel commonResponseModel = new();
            try
            {
                var emailId = article.User.Email;
                var user = await _sampleTempDBContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(emailId));
                if (user == null)
                {
                    commonResponseModel.Data = null;
                    commonResponseModel.Message = "User not found!";
                    commonResponseModel.StatusCode = StatusCodes.Status404NotFound;
                    commonResponseModel.Status = false;
                }
                else
                {
                    article.UserId = user.Id;
                    article.CreatedAt = DateTime.UtcNow;
                    commonResponseModel.Message = "Arcticle added successfully!";
                    commonResponseModel.Status = true;
                    commonResponseModel.StatusCode = StatusCodes.Status200OK;
                    commonResponseModel.Data = new { id = article.Id, article };
                }
                return commonResponseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return commonResponseModel;
            }
        }

        public async Task<CommonResponseModel> UpdateArticleById(int id, Article updatedArticle,string? userName)
        {
            CommonResponseModel commonResponseModel = new();
            try
            {
                var article = await _sampleTempDBContext.Articles.FirstOrDefaultAsync(a => a.Id == id);
                if (article == null)
                {
                    commonResponseModel.Data = null;
                    commonResponseModel.Message = "Article not found!";
                    commonResponseModel.StatusCode = StatusCodes.Status404NotFound;
                    commonResponseModel.Status = false;
                    return commonResponseModel;
                }
                    var user = await _sampleTempDBContext.Users.FirstOrDefaultAsync(u => u.Username == userName);
                    if (article.UserId != user.Id)
                    {
                        commonResponseModel.Message = "You are not authorized to update this article.";
                        commonResponseModel.Status = false;
                        commonResponseModel.StatusCode = StatusCodes.Status401Unauthorized;
                        commonResponseModel.Data = null;
                        return commonResponseModel;
                    }
                    article.Title = updatedArticle.Title;
                    article.Content = updatedArticle.Content;
                    article.UpdatedAt = DateTime.UtcNow;
                    await _sampleTempDBContext.SaveChangesAsync();
                    commonResponseModel.Data = article;
                    commonResponseModel.Message = "Article updated successfully!";
                    commonResponseModel.StatusCode = StatusCodes.Status200OK;
                    commonResponseModel.Status = true;
                return commonResponseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return commonResponseModel;
            }
        }


        public async Task<CommonResponseModel> DeleteArticleById(int id,string? userName)
        {
            CommonResponseModel commonResponseModel = new();
            try
            {
                var article = await _sampleTempDBContext.Articles.FirstOrDefaultAsync(a => a.Id == id);
                if (article == null)
                {
                    commonResponseModel.Data = null;
                    commonResponseModel.Message = "Article not found!";
                    commonResponseModel.StatusCode = StatusCodes.Status404NotFound;
                    commonResponseModel.Status = false;
                    return commonResponseModel;
                }
                var user = await _sampleTempDBContext.Users.FirstOrDefaultAsync(u => u.Username == userName);
                if (article.UserId != user.Id)
                {
                        commonResponseModel.Message = "You are not authorized to delete this article.";
                        commonResponseModel.Status = false;
                        commonResponseModel.StatusCode = StatusCodes.Status401Unauthorized;
                        commonResponseModel.Data = null;
                    return commonResponseModel;
                }
                _sampleTempDBContext.Articles.Remove(article);
                await _sampleTempDBContext.SaveChangesAsync();
                return commonResponseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return commonResponseModel;
            }
        }
    }
}

