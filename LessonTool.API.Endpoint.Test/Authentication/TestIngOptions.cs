namespace LessonTool.API.Endpoint.Test.Authentication;

internal class TestIngOptions
{
    public string UserType { get; set; }
    public bool UseInMemoryDatabase { get; set; } = true;

    public TestIngOptions(string userType, bool isInMemory = true)
    {
        UserType = userType;
        UseInMemoryDatabase = isInMemory;
    }
}
