using System.Collections.ObjectModel;
using GameEngine.GameData;
using GameAction = GameEngine.GameData.Action;

namespace GameDatabaseEditor.Utilities;

internal sealed class ActionEditSession
{
    private readonly ObservableCollection<GameAction> _actions;
    private readonly GameAction _action;

    public GameAction Draft { get; }

    public ActionEditSession(ObservableCollection<GameAction> actions, GameAction action)
    {
        _actions = actions;
        _action = action;
        Draft = new GameAction
        {
            Id = action.Id,
            Description = action.Description,
            TriggerNounId = action.TriggerNounId,
            TriggerVerbId = action.TriggerVerbId,
            Commands = new ObservableCollection<ActionCommand>(action.Commands.Select(command =>
                new ActionCommand { Command = command.Command, ObjectId = command.ObjectId })),
            Conditions = new ObservableCollection<ActionCondition>(action.Conditions.Select(condition =>
                new ActionCondition { Condition = condition.Condition, ObjectId = condition.ObjectId }))
        };
    }

    public bool Save()
    {
        int index = _actions.IndexOf(_action);
        if (index < 0)
        {
            return false;
        }

        _actions[index] = Draft;
        return true;
    }
}
