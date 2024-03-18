﻿namespace LessonTool.API.Authentication.Models;

public class UserAccount
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public string PasswordResetToken { get; set; }
}
