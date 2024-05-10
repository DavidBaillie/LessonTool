using LessonTool.API.Authentication.Exceptions;
using LessonTool.API.Authentication.Interfaces;
using LessonTool.API.Domain.Interfaces;
using LessonTool.API.Infrastructure.Interfaces;
using LessonTool.Common.Domain.Constants;
using LessonTool.Common.Domain.Interfaces;
using LessonTool.Common.Domain.Models.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers;

[ApiController]
[Route("/api/account")]
public class AuthenticationController(IUserAccountRepository _userAccounts, ILoginRequestProcessor _loginRequestProcessor, ITokenGenerationService _tokenGenerator,
     IPasswordComplexityValidator _complexityValidator, IHashService _hashService) : ControllerBase
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
                return _loginRequestProcessor.ProcessAnonymousLoginRequest();
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

                return _loginRequestProcessor.ProcessUserAccountLoginRequest(user, loginRequest);
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

            //Process the token for a new session
            if (username == "Anonymous")
            {
                return _loginRequestProcessor.ProcessAnonymousRefreshRequest(tokens);
            }
            else
            {
                var user = await _userAccounts.GetAccountByUsernameAsync(username, cancellationToken);

                if (user == null) 
                    return Unauthorized();

                return _loginRequestProcessor.ProcessUserAccountRefreshRequest(user, tokens);
            }
        }
        catch (NotExpiredException)
        {
            return StatusCode(425);
        }
        catch
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

#if DEBUG

    //[HttpGet("test")]
    //public ActionResult<PasswordData> GetHashedData(string password)
    //{
    //    string firstHash = _hashService.HashString(password);
    //    byte[] salt = _hashService.CreateSalt();
    //    string dbHash = _hashService.HashStringWithSalt(firstHash, salt);

    //    return Ok(new PasswordData(firstHash, dbHash, Convert.ToBase64String(salt)));
    //}

    //[HttpGet("testCreate")]
    //public async Task<ActionResult<PasswordData>> TestCreate(string username, string password)
    //{
    //    string firstHash = _hashService.HashString(password);
    //    byte[] salt = _hashService.CreateSalt();
    //    string dbHash = _hashService.HashStringWithSalt(firstHash, salt);

    //    await _userAccounts.CreateAsync(new Authentication.Models.UserAccount()
    //    {
    //        AccountType = UserClaimConstants.Admin,
    //        Username = username,
    //        Password = dbHash,
    //        PasswordSalt = Convert.ToBase64String(salt),
    //        PasswordResetToken = ""
    //    });

    //    return Ok();
    //}

    //public record PasswordData(string sentHash, string dbHash, string salt);

#endif

}