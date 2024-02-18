using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace LessonTool.UI.WebApp.Extensions
{
    public static class QueryHelperExtensions
    {
        public delegate bool QueryParameterParserDelegate<U>(string parameterValue, out U value);

        public static bool TryGetQueryString<T>(this NavigationManager navigationManager, string queryKey, out T value, QueryParameterParserDelegate<T> parser)
        {
            var absoluteUri = navigationManager.ToAbsoluteUri(navigationManager.Uri);

            if (QueryHelpers.ParseQuery(absoluteUri.Query).TryGetValue(queryKey, out var valueFromQueryString))
            {
                return parser(valueFromQueryString, out value);
            }

            value = default;
            return false;
        }
    }
}
