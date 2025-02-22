namespace SampleCoreAPIApp.Authorization;

using SampleCoreAPIApp.Models;

public interface IJwtUtils
{
    public string GenerateToken(User user);
    public int? ValidateToken(string token);
}