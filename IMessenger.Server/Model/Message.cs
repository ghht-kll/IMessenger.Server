using System.Text.Json.Serialization;

namespace IMessenger.Server.Model;

public partial class Message
{
    public int Id { get; set; }
    public string? Senderid { get; set; }
    public string? Receiverid { get; set; }
    public string Text { get; set; } = null!;
    public DateTime Sendingtime { get; set; }

    [JsonIgnore]
    public virtual User? Receiver { get; set; }
    [JsonIgnore]
    public virtual User? Sender { get; set; }
}
