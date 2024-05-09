using LessonTool.API.Authentication.Constants;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers
{
    [Authorize(Policy = PolicyNameConstants.AdminPolicy)]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPost]
        public async Task<ActionResult<ResetUserDto>> CreateAsync(UserDto userDto, CancellationToken cancellationToken)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPost("resetPassword/{id}")]
        public async Task<ActionResult<string>> ResetPasswordAsync(Guid id, CancellationToken cancellationToken)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateAsync(UserDto userDto, CancellationToken cancellationToken)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
