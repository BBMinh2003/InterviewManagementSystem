namespace IMS.Business.Services;

public interface IPasswordService
{
    string GenerateUserName(string fullName, int number);
    string GenerateValidPassword();
}
