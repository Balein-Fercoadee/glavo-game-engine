using GameEngine.GameData;
using GameAction = GameEngine.GameData.Action;

namespace GameDatabaseEditor.Utilities;

internal static class ActionEditorValidation
{
    public static IEnumerable<IIdentifiable> CommandTargets(GameDatabase database, ActionCommands command) =>
        command switch
        {
            ActionCommands.DisplayMessage => database.Messages,
            ActionCommands.PutItemInRoom or ActionCommands.RemoveItemFromRoom => database.Items,
            ActionCommands.MoveToRoom => database.Rooms,
            _ => Array.Empty<IIdentifiable>()
        };

    public static string? Validate(GameDatabase database, GameAction action)
    {
        if (action.TriggerVerbId < 0 ||
            !database.Words.Any(word => word.Id == action.TriggerVerbId && word.WordType == WordTypes.Verb))
        {
            return "Select an existing trigger verb.";
        }

        if (action.TriggerNounId < 0 ||
            !database.Words.Any(word => word.Id == action.TriggerNounId && word.WordType == WordTypes.Noun))
        {
            return "Select an existing trigger noun.";
        }

        foreach (var condition in action.Conditions)
        {
            if (!Enum.IsDefined(condition.Condition) || !database.Items.Any(item => item.Id == condition.ObjectId))
            {
                return "Each condition must have a valid type and an existing item.";
            }
        }

        foreach (var command in action.Commands)
        {
            if (!Enum.IsDefined(command.Command) ||
                !CommandTargets(database, command.Command).Any(target => target.Id == command.ObjectId))
            {
                return "Each command must have a valid type and an existing target.";
            }
        }

        return null;
    }
}
