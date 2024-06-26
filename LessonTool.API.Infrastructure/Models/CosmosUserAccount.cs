﻿namespace LessonTool.API.Infrastructure.Models;

public class CosmosUserAccount : CosmosEntityBase
{
    public string Username { get; set; }
    public string AccountType { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public string PasswordResetToken { get; set; }
}