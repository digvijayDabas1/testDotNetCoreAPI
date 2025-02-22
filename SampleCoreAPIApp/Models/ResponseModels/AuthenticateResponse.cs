using System;
namespace SampleCoreAPIApp.Models.ResponseModels
{
	public class AuthenticateResponse
	{
        public string? Token { get; set; }
        public int UserId { get; set; }
    }
}

