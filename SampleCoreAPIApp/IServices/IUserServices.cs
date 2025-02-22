using System;
using Microsoft.AspNetCore.Identity.Data;
using SampleCoreAPIApp.Models;
using SampleCoreAPIApp.Models.RequestModels;
using SampleCoreAPIApp.Models.ResponseModels;

namespace SampleCoreAPIApp.IServices
{
	public interface IUserServices
	{
        User GetById(int id);
        CommonResponseModel Authenticate(AuthenticateRequest model);
        CommonResponseModel Register(RegisterRequest model);
        CommonResponseModel GetProfileDetails(int id);
    }
}

