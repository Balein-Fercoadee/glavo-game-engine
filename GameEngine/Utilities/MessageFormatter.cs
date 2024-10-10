
using System.ComponentModel;
using GameEngine.GameData;

namespace GameEngine.Utilities;

public static class Formatter
{
    /// <summary>
    /// Creates a formatted error message used during game startup.
    /// </summary>
    /// <param name="errorMessages">A collection of error messages from the startup process.</param>
    /// <returns>A nicely formatted string.</returns>
    public static string ErrorMessage(IEnumerable<string> errorMessages)
    {
        string errorMessage = string.Join("\n", errorMessages);
        string fullErrorMessage = string.Empty;

        fullErrorMessage += "\n";
        fullErrorMessage += Constants.SETUP_ERROR_HEADER_TEXT + "\n";
        fullErrorMessage += errorMessage + "\n";
        fullErrorMessage += "\n";
        fullErrorMessage += Constants.LOADING_HELP_TEXT + "\n";

        return fullErrorMessage;
    }

    /// <summary>
    /// Creates a formatted message that contains the game database name and description.
    /// </summary>
    /// <param name="gameDatabase"></param>
    /// <returns></returns>
    public static string GameLoadedMessage(GameDatabase gameDatabase)
    {
        string message = string.Empty;

        message += "\n";
        message += new string('-', 20) + "\n";
        message += $"{gameDatabase.Name}\n";
        message += $"{gameDatabase.Description}\n";
        message += "\n";

        return message;
    }

    /// <summary>
    /// Creates a formatted message with a Room's available exits.
    /// </summary>
    /// <param name="exits"></param>
    /// <returns></returns>
    public static string RoomAvailableExitsMessage(List<string> exits)
    {
        string formattedExits = string.Empty;
        List<string> exitList = new List<string>();

        foreach (var exit in exits)
        {
            string formattedExit = string.Empty;

            switch (exit)
            {
                case "n": formattedExit = "(N)orth"; break;
                case "s": formattedExit = "(S)outh"; break;
                case "e": formattedExit = "(E)ast"; break;
                case "w": formattedExit = "(W)est"; break;
            }
            exitList.Add(formattedExit);
        }

        return string.Join(", ", exitList);
    }
}