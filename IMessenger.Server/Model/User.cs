namespace IMessenger.Server.Model;

public partial class User
{
    public User()
    {
        MessageReceivers = new HashSet<Message>();
        MessageSenders = new HashSet<Message>();
    }

    public int Id { get; set; }
    public string Userid { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? About { get; set; }
    public string? Photouri { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;

    public virtual ICollection<Message> MessageReceivers { get; set; }
    public virtual ICollection<Message> MessageSenders { get; set; }
}
