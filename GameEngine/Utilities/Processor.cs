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
                    ProcessGo(verb, noun, gameState.GameData, gameState.PlayerData);
                    break;
                case "help":
                    Console.WriteLine(MessageFormatter.Help());
                    break;
                case "look":
                    if (noun == null) // look with no noun means to look at the room.
                        Console.WriteLine(MessageFormatter.Look(gameState.PlayerData.CurrentRoomId, gameState.GameData, gameState.PlayerData, true));
                    else
                    {
                        switch (noun.Name)
                        {
                            case "inventory":
                                Console.WriteLine("You don't have an inventory yet!");
                            break;
                            default:
                                // assume that we're looking at an item either in inventory or in the room
                                
                            break;
                        }
                    }
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

    private static void ProcessGo(Word verb, Word? noun, GameDatabase gameData, Player player)
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
}