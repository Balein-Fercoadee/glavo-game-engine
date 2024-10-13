using GameEngine.GameData;

namespace GameEngine.Utilities;

public static class Processor
{
    public static bool ProcessWords(Word? verb, Word? noun, GameState gameState)
    {
        bool quitGame = false;

        if (verb != null)
        {
            switch (verb.Name)
            {
                case "go":
                    ProcessGo(noun, gameState.GameData, gameState.PlayerData);
                    break;
                case "help":
                    Console.WriteLine(MessageFormatter.Help());
                    break;
                case "look":
                    ProcessLook(noun, gameState.GameData, gameState.PlayerData);
                    break;
                case "take":
                    break;
                case "quit":
                    Console.WriteLine("Such a quiter. BOOOOOOOO!");
                    quitGame = true;
                    break;
            }
        }
        else
        {
            // A known verb wasn't found
            Console.WriteLine("You don't know how to do that!");
        }

        return quitGame;
    }

    private static void ProcessGo(Word? noun, GameDatabase gameData, Player player)
    {
        if (noun != null)
        {
            Room currentRoom = ObjectFinder.GetRoom(gameData.Rooms, player.CurrentRoomId);
            int roomId = -1;
            switch (noun.Name)
            {
                case "north":
                    roomId = currentRoom.ExitIdNorth;
                    break;
                case "south":
                    roomId = currentRoom.ExitIdSouth;
                    break;
                case "east":
                    roomId = currentRoom.ExitIdEast;
                    break;
                case "west":
                    roomId = currentRoom.ExitIdWest;
                    break;
            }

            if (roomId != Constants.ROOM_ID_UNSET)
            {
                Console.WriteLine(MessageFormatter.Look(roomId, gameData, player));
                player.CurrentRoomId = roomId;
            }
            else
            {
                Console.WriteLine("You can't go in that direction!!");
            }
        }
        else
        {
            Console.WriteLine("Go where??");
        }        
    }

    private static void ProcessLook(Word? noun, GameDatabase gameData, Player player)
    {
        if (noun == null) // look with no noun means to look at the room.
            Console.WriteLine(MessageFormatter.Look(player.CurrentRoomId, gameData, player, true));
        else
        {
            switch (noun.Name)
            {
                case "inventory":
                    Console.WriteLine(MessageFormatter.Inventory(player, gameData));
                break;
                default:
                    Room currentRoom = ObjectFinder.GetRoom(gameData.Rooms, player.CurrentRoomId);
                    // assume that we're looking at an item either in inventory or in the room
                break;
            }
        }
    }
}