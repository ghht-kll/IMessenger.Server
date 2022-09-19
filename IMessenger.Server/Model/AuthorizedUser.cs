namespace IMessenger.Server.Model;
public class AuthorizedUser
{
    public AuthorizedUser(string accessToken, string userName)
    {
        AccessToken = accessToken;
        UserName = userName;
    }

    public string AccessToken { get; set; } = null!;
    public string UserName { get; set; } = null!;
}