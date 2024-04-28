using LessonTool.API.Authentication.Constants;
using LessonTool.API.Infrastructure.Models;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers
{
    [ApiController]
    [Authorize(Policy = PolicyNameConstants.AdminPolicy)]
    [Route("/users")]
    public class UsersController(IRepository<CosmosUserAccount> userRepository) 
        : ControllerBase
    {
        //[HttpGet]
        //public async Task<ActionResult<UserDto>> GetUsers(CancellationToken cancellationToken)
        //{
            
        //}
    }
}
