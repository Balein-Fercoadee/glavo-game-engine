using GameDatabaseEditor.Utilities;
using GameEngine.GameData;
using GameAction = GameEngine.GameData.Action;

namespace GameDatabaseEditor.Views;

public sealed record ActionTarget(int Id, string Display);

public partial class ActionEditorView : ContentView
{
    private readonly GameDatabase _database;
    private readonly GameAction _action;

    public ActionEditorView(GameDatabase database, GameAction action)
    {
        _database = database;
        _action = action;
        InitializeComponent();

        pckVerb.ItemsSource = database.Words.Where(word => word.WordType == WordTypes.Verb).ToList();
        pckNoun.ItemsSource = database.Words.Where(word => word.WordType == WordTypes.Noun).ToList();
        pckVerb.SelectedItem = database.Words.FirstOrDefault(word =>
            word.WordType == WordTypes.Verb && word.Id == action.TriggerVerbId);
        pckNoun.SelectedItem = database.Words.FirstOrDefault(word =>
            word.WordType == WordTypes.Noun && word.Id == action.TriggerNounId);
        pckConditionType.ItemsSource = Enum.GetValues<ActionConditions>();
        pckConditionItem.ItemsSource = CreateTargets(database.Items);
        pckCommandType.ItemsSource = Enum.GetValues<ActionCommands>();
        BindingContext = action;
    }

    public bool TryApplyChanges()
    {
        _action.TriggerVerbId = (pckVerb.SelectedItem as Word)?.Id ?? -1;
        _action.TriggerNounId = (pckNoun.SelectedItem as Word)?.Id ?? -1;
        string? error = ActionEditorValidation.Validate(_database, _action);
        lblError.Text = error ?? string.Empty;
        return error is null;
    }

    private void AddCondition_Clicked(object sender, EventArgs e)
    {
        if (pckConditionType.SelectedItem is ActionConditions type && pckConditionItem.SelectedItem is ActionTarget target)
        {
            _action.Conditions.Add(new ActionCondition { Condition = type, ObjectId = target.Id });
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Text = "Select a condition type and item.";
        }
    }

    private void UpdateCondition_Clicked(object sender, EventArgs e)
    {
        if (lstConditions.SelectedItem is ActionCondition selected &&
            pckConditionType.SelectedItem is ActionConditions type &&
            pckConditionItem.SelectedItem is ActionTarget target)
        {
            int index = _action.Conditions.IndexOf(selected);
            _action.Conditions[index] = new ActionCondition { Condition = type, ObjectId = target.Id };
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Text = "Select a condition, type and item to update.";
        }
    }

    private void RemoveCondition_Clicked(object sender, EventArgs e)
    {
        if (lstConditions.SelectedItem is ActionCondition selected)
        {
            lstConditions.SelectedItem = null;
            _action.Conditions.Remove(selected);
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Text = "Select a condition to remove.";
        }
    }

    private void Condition_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lstConditions.SelectedItem is ActionCondition condition)
        {
            pckConditionType.SelectedItem = condition.Condition;
            SelectTarget(pckConditionItem, condition.ObjectId);
        }
    }

    private void CommandType_Changed(object sender, EventArgs e)
    {
        pckCommandTarget.ItemsSource = pckCommandType.SelectedItem is ActionCommands command
            ? CreateTargets(ActionEditorValidation.CommandTargets(_database, command))
            : new List<ActionTarget>();
    }

    private void AddCommand_Clicked(object sender, EventArgs e)
    {
        if (pckCommandType.SelectedItem is ActionCommands type && pckCommandTarget.SelectedItem is ActionTarget target)
        {
            _action.Commands.Add(new ActionCommand { Command = type, ObjectId = target.Id });
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Text = "Select a command type and target.";
        }
    }

    private void UpdateCommand_Clicked(object sender, EventArgs e)
    {
        if (lstCommands.SelectedItem is ActionCommand selected &&
            pckCommandType.SelectedItem is ActionCommands type &&
            pckCommandTarget.SelectedItem is ActionTarget target)
        {
            int index = _action.Commands.IndexOf(selected);
            _action.Commands[index] = new ActionCommand { Command = type, ObjectId = target.Id };
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Text = "Select a command, type and target to update.";
        }
    }

    private void RemoveCommand_Clicked(object sender, EventArgs e)
    {
        if (lstCommands.SelectedItem is ActionCommand selected)
        {
            lstCommands.SelectedItem = null;
            _action.Commands.Remove(selected);
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Text = "Select a command to remove.";
        }
    }

    private void Command_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lstCommands.SelectedItem is ActionCommand command)
        {
            pckCommandType.SelectedItem = command.Command;
            SelectTarget(pckCommandTarget, command.ObjectId);
        }
    }

    private void MoveCommandUp_Clicked(object sender, EventArgs e) => MoveCommand(-1);
    private void MoveCommandDown_Clicked(object sender, EventArgs e) => MoveCommand(1);

    private void MoveCommand(int direction)
    {
        if (lstCommands.SelectedItem is not ActionCommand selected)
        {
            lblError.Text = "Select a command to move.";
            return;
        }

        int index = _action.Commands.IndexOf(selected);
        int destination = index + direction;
        if (destination >= 0 && destination < _action.Commands.Count)
        {
            _action.Commands.Move(index, destination);
            lstCommands.SelectedItem = selected;
        }
        lblError.Text = string.Empty;
    }

    private static List<ActionTarget> CreateTargets(IEnumerable<IIdentifiable> targets) =>
        targets.Select(target => new ActionTarget(target.Id, target switch
        {
            Item item => $"{item.Id} - {item.Name}",
            Room room => $"{room.Id} - {room.Name}",
            Message message => $"{message.Id} - {message.Text}",
            _ => throw new InvalidOperationException("Unsupported action target.")
        })).ToList();

    private static void SelectTarget(Picker picker, int id)
    {
        picker.SelectedItem = (picker.ItemsSource as IEnumerable<ActionTarget>)?.FirstOrDefault(target => target.Id == id);
    }
}
