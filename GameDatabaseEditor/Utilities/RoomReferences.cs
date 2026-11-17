using GameEngine.GameData;

namespace GameDatabaseEditor.Utilities;

internal static class RoomReferences
{
    public static List<string> Find(GameDatabase database, int roomId)
    {
        var references = new List<string>();
        if (database.StartingRoomId == roomId)
        {
            references.Add("Starting Room ID");
        }

        if (database.TreasureRoomId == roomId)
        {
            references.Add("Treasure Room ID");
        }

        foreach (var action in database.Actions)
        {
            if (action.Commands.Any(command =>
                    command.Command == ActionCommands.MoveToRoom && command.ObjectId == roomId))
            {
                references.Add($"Action {action.Id}: command");
            }
        }

        return references;
    }
}
