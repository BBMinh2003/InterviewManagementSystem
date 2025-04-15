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

        while (password.Length < options.RequiredLength)
        {
            password.Append((char)random.Next(33, 126)); 
        }

        if (options.RequireUppercase)
        {
            password.Append((char)random.Next(65, 90)); 
        }

        if (options.RequireLowercase)
        {
            password.Append((char)random.Next(97, 122));
        }

        if (options.RequireDigit)
        {
            password.Append((char)random.Next(48, 57));
        }

        if (options.RequireNonAlphanumeric)
        {
            var specialChars = "!@#$%^&*()-_=+[]{}|;:'\",.<>?";
            password.Append(specialChars[random.Next(specialChars.Length)]);
        }

        return new string(password.ToString().ToCharArray().OrderBy(_ => random.Next()).ToArray());
    }

    public string GenerateUserName(string fullName, int number)
{
    if (string.IsNullOrWhiteSpace(fullName))
        throw new ArgumentException("Full name is required");

    var nameParts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

    if (nameParts.Length < 2)
        throw new ArgumentException("Full name must have at least two parts");

    string firstName = nameParts[^1]; 
    string initials = string.Concat(nameParts.Take(nameParts.Length - 1).Select(part => part[0]));

    return $"{firstName}{initials}{number}";
}

}
