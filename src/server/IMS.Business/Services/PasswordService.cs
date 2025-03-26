using System;
using System.Text;
using IMS.Models.Security;
using Microsoft.AspNetCore.Identity;

namespace IMS.Business.Services;

public class PasswordService(UserManager<User> userManager) : IPasswordService
{
    private readonly UserManager<User> _userManager = userManager;

    public string GenerateValidPassword()
    {
        var options = _userManager.Options.Password;

        var random = new Random();
        var password = new StringBuilder();

        // Ensure required length
        while (password.Length < options.RequiredLength)
        {
            password.Append((char)random.Next(33, 126)); // Random ASCII characters
        }

        // Ensure at least one uppercase letter
        if (options.RequireUppercase)
        {
            password.Append((char)random.Next(65, 90)); // A-Z
        }

        // Ensure at least one lowercase letter
        if (options.RequireLowercase)
        {
            password.Append((char)random.Next(97, 122)); // a-z
        }

        // Ensure at least one digit
        if (options.RequireDigit)
        {
            password.Append((char)random.Next(48, 57)); // 0-9
        }

        // Ensure at least one special character
        if (options.RequireNonAlphanumeric)
        {
            var specialChars = "!@#$%^&*()-_=+[]{}|;:'\",.<>?";
            password.Append(specialChars[random.Next(specialChars.Length)]);
        }

        // Shuffle the password to avoid predictable patterns
        return new string(password.ToString().ToCharArray().OrderBy(_ => random.Next()).ToArray());
    }
}
