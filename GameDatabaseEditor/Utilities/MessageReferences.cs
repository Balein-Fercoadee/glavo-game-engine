using GameEngine.GameData;

namespace GameDatabaseEditor.Utilities;

internal static class MessageReferences
{
    public static List<string> Find(GameDatabase database, int messageId)
    {
        var references = new List<string>();
        foreach (var action in database.Actions)
        {
            if (action.Commands.Any(command =>
                    command.Command == ActionCommands.DisplayMessage && command.ObjectId == messageId))
            {
                references.Add($"Action {action.Id}: command");
            }
        }

        return references;
    }
}
