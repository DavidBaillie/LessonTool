using LessonTool.Common.Domain.Models.Authentication;

namespace LessonTool.Common.Domain.Extensions;

public static class AccessTokensResponseModelExtensions
{
    public static string TokensToString(this AccessTokensResponseModel model)
    {
        return $"{model.AccessToken};{model.RefreshToken};{model.Expires}";
    }

    public static AccessTokensResponseModel StringtoTokens(this string content)
    {
        var parts = content.Split(';');

        if (parts.Length != 3)
            throw new ArgumentException("Provided string cannot be parsed because too many elements were extracted!");

        return new AccessTokensResponseModel()
        {
            AccessToken = parts[0],
            RefreshToken = parts[1],
            Expires = DateTime.Parse(parts[2]),
        };
    }
}
