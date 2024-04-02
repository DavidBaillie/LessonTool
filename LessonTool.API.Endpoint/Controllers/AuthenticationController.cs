using LessonTool.API.Authentication.Exceptions;
using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Domain.Interfaces;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers
{
    [ApiController]
    [Route("/api/account/")]
    public class AuthenticationController(IUserAccountRepository _userAccounts, ILoginRequestProcessor _loginRequestProcessor, ITokenGenerationService _tokenGenerator,
        ILoginSessionRepository _loginSessions, IPasswordComplexityValidator _complexityValidator, IHashService _hashService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<AccessTokensResponseModel>> AuthenticateAsync([FromBody] LoginRequestModel loginRequest, CancellationToken cancellationToken)
        {
            try
            {
                //Quick safety check to filter out bots scanning endpoints
                if (loginRequest.RequestToken != TokenConstants.AuthenticationRequestToken || 
                    string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.HashedPassword))
                    return Unauthorized();

                //Anonymous account for readonly
                if (loginRequest.Username == TokenConstants.AnonymousAccountToken.ToString() && loginRequest.HashedPassword == TokenConstants.AnonymousAccountToken.ToString())
                    return await _loginRequestProcessor.ProcessAnonymousLoginRequest(cancellationToken);
                //Actual account for read/write
                else
                {
                    //Check to see if we have a user
                    var user = await _userAccounts.GetAccountByUsernameAsync(loginRequest.Username, cancellationToken);
                    if (user == null)
                        return Unauthorized();

                    //User needs to reset password
                    if (string.IsNullOrWhiteSpace(user.Password))
                        return Conflict();

                    return await _loginRequestProcessor.ProcessUserAccountLoginRequest(user, loginRequest, cancellationToken);
                }
            }
            catch (AuthenticationFailureException authEx)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpPut("refresh")]
        public async Task<ActionResult<AccessTokensResponseModel>> RefreshTokensAsync([FromBody]RefreshTokensRequestModel tokens, CancellationToken cancellationToken)
        {
            if (tokens is null || tokens.RequestToken != TokenConstants.AuthenticationRequestToken)
                return Unauthorized();

            try
            {
                //Verify this API made the token and it's valid
                var principle = _tokenGenerator.GetPrincipalFromExpiredToken(tokens.Token);
                var username = principle.Identity.Name;

                await _loginSessions.DeleteExpiredSessionsAsync(cancellationToken);

                //Process the token for a new session
                if (username == "Anonymous")
                {
                    return await _loginRequestProcessor.ProcessAnonymousRefreshRequest(tokens, cancellationToken);
                }
                else
                {
                    var user = await _userAccounts.GetAccountByUsernameAsync(username, cancellationToken);
                    return await _loginRequestProcessor.ProcessUserAccountRefreshRequest(user, tokens, cancellationToken);
                }
            }
            catch (NotExpiredException)
            {
                return StatusCode(425);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }

        [HttpPost("reset")]
        public async Task<ActionResult> ResetPassword([FromBody]PasswordResetRequestModel request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.RequestToken != TokenConstants.AuthenticationRequestToken)
                    return Unauthorized();

                //Check user exists and can reset password
                var user = await _userAccounts.GetAccountByUsernameAsync(request.Username, cancellationToken);
                if (user is null || !string.IsNullOrWhiteSpace(user.Password))
                    return Unauthorized();

                //Invalid reset token
                if (string.IsNullOrWhiteSpace(user.PasswordResetToken) || user.PasswordResetToken != request.ResetToken)
                    return Unauthorized();

                //Make sure password is good
                if (!_complexityValidator.PasswordIsSufficient(request.Password))
                    return BadRequest("Password complexity insufficient!");

                var salt = _hashService.CreateSalt();
                var hashedPassword = _hashService.HashString(request.Password);

                user.PasswordResetToken = "";
                user.PasswordSalt = Convert.ToBase64String(salt);
                user.Password = _hashService.HashStringWithSalt(hashedPassword, salt);

                return Ok();
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }       
    }
}