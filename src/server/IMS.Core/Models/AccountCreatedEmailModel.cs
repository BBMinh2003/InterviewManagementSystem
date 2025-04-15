using System;

namespace IMS.Core.Models;

public class AccountCreatedEmailModel
{
    public string Username { get; set; }  = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string RecruiterEmail { get; set; } = string.Empty;
}

