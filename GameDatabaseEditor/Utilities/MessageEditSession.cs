using System.Collections.ObjectModel;
using GameEngine.GameData;

namespace GameDatabaseEditor.Utilities;

internal sealed class MessageEditSession
{
    private readonly ObservableCollection<Message> _messages;
    private readonly Message _message;

    public Message Draft { get; }

    public MessageEditSession(ObservableCollection<Message> messages, Message message)
    {
        _messages = messages;
        _message = message;
        Draft = new Message
        {
            Id = message.Id,
            Text = message.Text
        };
    }

    public bool Save()
    {
        int index = _messages.IndexOf(_message);
        if (index < 0)
        {
            return false;
        }

        _messages[index] = Draft;
        return true;
    }
}
