namespace GameEngine.GameData;

public class Message : IIdentifiable
{
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the text of the Message.
    /// </summary>
    public string Text { get; set; }

    public Message()
    {
        Text = string.Empty;
    }
}