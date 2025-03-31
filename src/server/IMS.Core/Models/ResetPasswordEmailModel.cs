using System;

namespace IMS.Core.Models;

public class ResetPasswordEmailModel
{
    public string Email { get; set; } = string.Empty;
    public string ResetLink { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; } = 0;
}