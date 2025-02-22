
using SampleCoreAPIApp.Authorization;
using SampleCoreAPIApp.DBContext;
using SampleCoreAPIApp.IServices;
using SampleCoreAPIApp.Models;
using AutoMapper;
using SampleCoreAPIApp.Models.ResponseModels;
using SampleCoreAPIApp.Models.RequestModels;
using Microsoft.AspNetCore.Identity.Data;

namespace SampleCoreAPIApp.Services
{
	public class UserServices:IUserServices
	{
        private readonly SampleTempDBContext _sampleTempDBContext;
        private readonly ILogger<UserServices> _logger;
        private readonly IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public UserServices(
            SampleTempDBContext sampleTempDBContext,
            ILogger<UserServices> logger,
            IJwtUtils jwtUtils,
            IMapper mapper)
		{
            _sampleTempDBContext = sampleTempDBContext;
            _logger = logger;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
		}

        public User GetById(int id)
        {
            var user = _sampleTempDBContext.Users.Find(id);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", id);
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }


        public CommonResponseModel Authenticate(AuthenticateRequest model)
        {
            CommonResponseModel commonResponseModel = new CommonResponseModel();
            try
            {
                var user = _sampleTempDBContext.Users.SingleOrDefault(x => x.Email == model.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    commonResponseModel.Status = true;
                    commonResponseModel.Message = "Invalid username or password";
                    commonResponseModel.StatusCode = StatusCodes.Status203NonAuthoritative;
                    return commonResponseModel;
                }
                var data = _mapper.Map<AuthenticateResponse>(user);
                data.Token = _jwtUtils.GenerateToken(user);
                data.UserId = user.Id;
                commonResponseModel.StatusCode = StatusCodes.Status200OK;
                commonResponseModel.Message = "Authenticate Successfully";
                commonResponseModel.Status = true;
                commonResponseModel.Data = data;
                return commonResponseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return commonResponseModel;
            }

        }

        public CommonResponseModel Register(RegisterRequest model)
        {
            CommonResponseModel commonResponseModel = new CommonResponseModel();
            try
            {
                if (_sampleTempDBContext.Users.Any(x => x.Email == model.Email))
                {
                    commonResponseModel.Message = $"Email '{model.Email}' is already taken";
                    commonResponseModel.Status = true;
                    commonResponseModel.StatusCode = StatusCodes.Status208AlreadyReported;
                    return commonResponseModel;
                }

                var user = _mapper.Map<User>(model);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                var token = Guid.NewGuid().ToString();
                _sampleTempDBContext.Users.Add(user);
                _sampleTempDBContext.SaveChanges();
                commonResponseModel.Message = "Registration successful";
                commonResponseModel.Status = true;
                commonResponseModel.StatusCode = StatusCodes.Status200OK;
                return commonResponseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return commonResponseModel;
            }

        }

        public CommonResponseModel GetProfileDetails(int id)
        {
            CommonResponseModel commonResponseModel = new CommonResponseModel();
            try
            {
                var user = _sampleTempDBContext.Users.Find(id);
                if (user == null)
                {
                    commonResponseModel.Status = false;
                    commonResponseModel.Message = "User not found!";
                    commonResponseModel.StatusCode = StatusCodes.Status204NoContent;
                    commonResponseModel.Data = null;
                    return commonResponseModel;
                }
                commonResponseModel.Data = user;
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

