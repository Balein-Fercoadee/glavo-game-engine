using GameEngine.GameData;

namespace GameDatabaseEditor.Utilities;

internal static class ItemReferences
{
    public static List<string> Find(GameDatabase database, int itemId)
    {
        var references = new List<string>();
        foreach (var room in database.Rooms)
        {
            if (room.ContainedItemIds.Contains(itemId))
            {
                references.Add($"Room {room.Id}: {room.Name}");
            }
        }

        foreach (var action in database.Actions)
        {
            if (action.Conditions.Any(condition =>
                    (condition.Condition == ActionConditions.PlayerHasItem ||
                     condition.Condition == ActionConditions.PlayerInRoomWithItem) &&
                    condition.ObjectId == itemId))
            {
                references.Add($"Action {action.Id}: condition");
            }

            if (action.Commands.Any(command =>
                    (command.Command == ActionCommands.PutItemInRoom ||
                     command.Command == ActionCommands.RemoveItemFromRoom) &&
                    command.ObjectId == itemId))
            {
                references.Add($"Action {action.Id}: command");
            }
        }

        return references;
    }
}
