using GameEngine.GameData;

namespace GameDatabaseEditor.Utilities;

internal static class WordReferences
{
    public static List<string> Find(GameDatabase database, int wordId)
    {
        var references = new List<string>();
        foreach (var action in database.Actions)
        {
            if (action.TriggerNounId == wordId)
            {
                references.Add($"Action {action.Id}: noun trigger");
            }

            if (action.TriggerVerbId == wordId)
            {
                references.Add($"Action {action.Id}: verb trigger");
            }
        }

        return references;
    }
}
