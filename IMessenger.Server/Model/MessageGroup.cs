namespace IMessenger.Server.Model;

public class MessageGroup
{
    public string Senderid { get; set; } = null!;
    public string Receiverid { get; set; } = null!;
    public int StartingPoint { get; set; }
    public int EndingPoint { get; set; }
}
