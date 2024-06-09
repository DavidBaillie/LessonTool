using LessonTool.API.Authentication.Constants;
using LessonTool.API.Authentication.Models;
using LessonTool.API.Infrastructure.Extensions;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers
{
    [Authorize(Policy = PolicyNameConstants.AdminPolicy)]
    [ApiController]
    [Route("api/users")]
    public class UserController(IUserAccountRepository _userAccounts, IHashService _hashService)
        : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var account =  await _userAccounts.GetAsync(id, cancellationToken);
            return Ok(account.ToUserDto());
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var accounts = await _userAccounts.GetAllAsync(cancellationToken);
            return Ok(accounts.Select(x => x.ToUserDto()));
        }

        [HttpPost]
        public async Task<ActionResult<ResetUserDto>> CreateAsync(UserDto userDto, CancellationToken cancellationToken)
        {
            var existing = await _userAccounts.GetAccountByUsernameAsync(userDto.Username);
            if (existing != null)
                return BadRequest($"This username is already taken.");

            var resetToken = Guid.NewGuid();
            var newAccount = new UserAccount()
            {
                Id = Guid.Empty,
                AccountType = userDto.AccountType,
                Username = userDto.Username,
                Password = "",
                PasswordSalt = "",
                PasswordResetToken = resetToken.ToString(),
            };

            var createdAccount = await _userAccounts.CreateAsync(newAccount, cancellationToken);

            return CreatedAtAction(nameof(GetAsync), new { createdAccount.Id }, new ResetUserDto()
            {
                Id = createdAccount.Id,
                AccountType = userDto.AccountType,
                Username = userDto.Username,
                PasswordResetToken = resetToken.ToString(),
            });
        }

        [HttpPost("resetPassword/{id}")]
        public async Task<ActionResult<string>> ResetPasswordAsync(Guid id, CancellationToken cancellationToken)
        {
            var resetToken = Guid.NewGuid().ToString();

            var account = await _userAccounts.GetAsync(id, cancellationToken);

            if (account == null) 
                return BadRequest($"No such account with id [{id}]");

            account.Password = "";
            account.PasswordSalt = "";
            account.PasswordResetToken = resetToken;
            await _userAccounts.UpdateAsync(account, cancellationToken);

            return Ok(resetToken);
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateAsync(UserDto userDto, CancellationToken cancellationToken)
        {
            var account = await _userAccounts.GetAsync(userDto.Id, cancellationToken);

            if (account == null)
                return BadRequest("Cannot update a user that does not exist!");

            account.Username = userDto.Username;
            account.AccountType = userDto.AccountType;
            var updated = await _userAccounts.UpdateAsync(account, cancellationToken);

            return Ok(updated.ToUserDto());
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _userAccounts.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
