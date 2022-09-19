namespace IMessenger.Server.Model;
public class AuthorizedUserModel
{
    public AuthorizedUserModel(string accessToken, string userName, string userId)
    {
        AccessToken = accessToken;
        UserName = userName;
        UserId = userId;
    }

    public string AccessToken { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string UserId { get; set; } = null!;
}