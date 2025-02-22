using System;
namespace SampleCoreAPIApp.Models.ResponseModels
{
    public class CommonResponseModel
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public bool Status { get; set; }
        public object? Data { get; set; }
    }
}

