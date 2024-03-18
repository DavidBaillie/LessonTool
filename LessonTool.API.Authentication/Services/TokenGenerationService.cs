using LessonTool.Common.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LessonTool.API.Authentication.Services;

public class TokenGenerationService(IConfiguration _configuration, IRepository<User>)
{

}
