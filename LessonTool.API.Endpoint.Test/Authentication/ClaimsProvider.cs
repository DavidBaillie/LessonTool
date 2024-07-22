using LessonTool.API.Authentication.Constants;
using LessonTool.Common.Domain.Constants;
using System.Security.Claims;

namespace LessonTool.API.Endpoint.Test.Authentication
{
    internal class ClaimsProvider
    {
        public IList<Claim> Claims { get; set; } = new List<Claim>();

        public ClaimsProvider() { }

        public ClaimsProvider(IList<Claim> claims)
        {
            this.Claims = claims;
        }

        public static ClaimsProvider CreateTestClaims(string user) => user switch
        {
            PolicyNameConstants.AdminPolicy => WithAdminClaims(),
            PolicyNameConstants.TeacherPolicy => WithTeacherClaims(),
            PolicyNameConstants.ParentPolicy => WithParentClaims(),
            PolicyNameConstants.StudentPolicy => WithStudentClaims(),
            PolicyNameConstants.ReaderPolicy => WithReaderClaims(),
            _ => throw new InvalidOperationException()
        };

        public static ClaimsProvider WithAdminClaims()
        {
            return new ClaimsProvider(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "Admin User"),
                new Claim(ClaimTypes.Role, UserClaimConstants.Admin),
            });
        }

        public static ClaimsProvider WithTeacherClaims()
        {
            return new ClaimsProvider(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "Teacher User"),
                new Claim(ClaimTypes.Role, UserClaimConstants.Teacher),
            });
        }

        public static ClaimsProvider WithParentClaims()
        {
            return new ClaimsProvider(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "Parent User"),
                new Claim(ClaimTypes.Role, UserClaimConstants.Parent),
            });
        }

        public static ClaimsProvider WithStudentClaims()
        {
            return new ClaimsProvider(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "Student User"),
                new Claim(ClaimTypes.Role, UserClaimConstants.Student),
            });
        }

        public static ClaimsProvider WithReaderClaims()
        {
            return new ClaimsProvider(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "Reader User"),
                new Claim(ClaimTypes.Role, UserClaimConstants.Reader),
            });
        }
    }
}
