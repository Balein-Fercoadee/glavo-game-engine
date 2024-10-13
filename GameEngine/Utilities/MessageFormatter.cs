using GameEngine.GameData;

namespace GameEngine.Utilities;

public static class MessageFormatter
{
    /// <summary>
    /// Creates a formatted error message used during game startup.
    /// </summary>
    /// <param name="errorMessages">A collection of error messages from the startup process.</param>
    /// <returns>A nicely formatted string.</returns>
    public static string Error(IEnumerable<string> errorMessages)
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
    public static string GameLoaded(GameDatabase gameDatabase)
    {
        string message = string.Empty;

        message += "\n";
        message += new string('-', 20) + "\n";
        message += $"{gameDatabase.Name}\n";
        message += $"{gameDatabase.Description}\n";
        message += "\n";

        return message;
    }

    public static string Help()
    {
        string helpMessage = string.Empty;

        helpMessage += "Try these words:\n";
        helpMessage += Constants.HELP_STANDARD_VERBS + "\n";
        helpMessage += Constants.HELP_STANDARD_DIRECTIONS + "\n";
        helpMessage += Constants.HELP_STANDARD_MISC + "\n";
        helpMessage += "All items have some usefulness. You need to find out how to use them.\n";
        helpMessage += "Items enclosed in '*' are treasures (ex. *large diamond*). Collect them!\n";

        return helpMessage;
    }

    public static string Inventory(Player player, GameDatabase gameDb)
    {
        string inv = string.Empty;

        var items = ObjectFinder.GetItems(gameDb.Items, player.ItemInventory).Select(i=>i.Name).ToList();

        if (items.Count != 0)
        {
            inv = string.Join(", ", items);
        }
        else
        {
            inv = "You're not carrying anything!";
        }

        return inv;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentRoomId">The <c>Room.Id</c> of the room where the player is currently at.</param>
    /// <param name="gameState">The <c>GameState</c> object.</param>
    /// <param name="overrideToFullDescription">Should the full description be returned regardless if the <c>Player</c> has arleady visited the <c>Room</c>.</param>
    /// <returns></returns>
    public static string Look(int currentRoomId, GameDatabase gameDb, Player player, bool overrideToFullDescription = false)
    {
        Room currentRoom = ObjectFinder.GetRoom(gameDb.Rooms, currentRoomId);
        List<string> itemsInRoom = ObjectFinder.GetItems(gameDb.Items, currentRoom.ContainedItemIds).Select(i=> i.IsTreasure ? $"*{i.Name}*" : i.Name).ToList<string>();
        List<string> exitsFromRoom = currentRoom.AvailableExits();

        string description = (!player.HasAlreadyVisitedRoom(currentRoomId) || overrideToFullDescription) ? currentRoom.Description : string.Empty;
        string items = string.Join(", ", itemsInRoom);
        string exits = RoomAvailableExits(exitsFromRoom);
        string availableExits = exits.Length > 0 ? $"Available exits: {exits}" : "Available exits: none apparent";
        string visibleItems = items.Length > 0 ? $"Also visible: {items}" : string.Empty;

        string lookResponse = $"{currentRoom.Name}\n{description}\n{visibleItems}\n{availableExits}";

        return lookResponse;
    }

    /// <summary>
    /// Creates a formatted message with a Room's available exits.
    /// </summary>
    /// <param name="exits"></param>
    /// <returns></returns>
    public static string RoomAvailableExits(List<string> exits)
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

    private static string VisibleItems(IEnumerable<Item> items)
    {
        return  string.Empty;
    }
}