using System;
using SampleCoreAPIApp.Models;
using SampleCoreAPIApp.Models.ResponseModels;

namespace SampleCoreAPIApp.IServices
{
	public interface IArticleServices
	{
        Task<CommonResponseModel> GetArticlesDetails();
        Task<CommonResponseModel> GetArticlebyId(int id);
        Task<CommonResponseModel> CreateArticleAsync(Article article);
        Task<CommonResponseModel> UpdateArticleById(int id, Article updatedArticle,string? userName);
        Task<CommonResponseModel> DeleteArticleById(int id, string? userName);
    }
}

