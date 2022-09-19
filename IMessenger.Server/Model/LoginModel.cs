namespace IMessenger.Server.Model;

public class LoginModel
{
    public LoginModel(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}