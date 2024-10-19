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
                case "drop":
                    ProcessDrop(noun, gameState.GameData, gameState.PlayerData);
                    break;
                case "go":
                    ProcessGo(noun, gameState.GameData, gameState.PlayerData);
                    break;
                case "help":
                    Console.WriteLine(MessageFormatter.Help());
                    break;
                case "look":
                    ProcessLook(noun, gameState.GameData, gameState.PlayerData);
                    break;
                case "score":
                    quitGame = ProcessScore(gameState.GameData);
                    break;
                case "take":
                    ProcessTake(noun, gameState.GameData, gameState.PlayerData);
                    break;
                case "quit":
                    Console.Write("Such a quiter. BOOOOOOOO!\n");
                    ProcessScore(gameState.GameData);
                    quitGame = true;
                    break;
                default:
                    break;
            }
            // Process actions
            ProcessActions(verb, noun, gameState.GameData, gameState.PlayerData);
        }
        else
        {
            // A known verb wasn't found
            Console.WriteLine("You don't know how to do that!");
        }

        return quitGame;
    }

    private static void ProcessActions(Word verb, Word? noun, GameDatabase gameData, Player player)
    {
        foreach (var action in gameData.Actions)
        {
            // The action's verb and noun match what the player entered
            if (noun != null && action.TriggerVerbId == verb.Id && action.TriggerNounId == noun.Id)
            {
                // Check the conditions
                Room currentRoom = ObjectFinder.GetRoom(gameData.Rooms, player.CurrentRoomId);
                bool conditionsMet = true;

                foreach (var condition in action.Conditions)
                {
                    switch (condition.Condition)
                    {
                        case ActionConditions.PlayerHasItem:
                            break;
                        case ActionConditions.PlayerWithItem:
                            if (currentRoom.ContainedItemIds.Contains(condition.ObjectId))
                            {
                                conditionsMet &= true;
                            }
                            else
                                conditionsMet &= false;
                            break;
                    }
                }

                // Execute the commands
                if (conditionsMet)
                {
                    foreach (var command in action.Commands)
                    {
                        switch (command.Command)
                        {
                            case ActionCommands.DisplayMessage:
                                var msg = ObjectFinder.GetMessage(gameData.Messages, command.ObjectId);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                Console.WriteLine(msg.Text);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                break;
                            case ActionCommands.MoveToRoom:
                                player.CurrentRoomId = command.ObjectId;
                                ProcessLook(null, gameData, player);
                                break;
                        }
                    }
                }
            }
        }
    }

    private static void ProcessDrop(Word? noun, GameDatabase gameData, Player player)
    {
        if (noun == null)
        {
            Console.WriteLine("Drop what??");
        }
        else
        {
            var currentRoom = ObjectFinder.GetRoom(gameData.Rooms, player.CurrentRoomId);
            switch (noun.Name)
            {
                case "all":
                    List<int> carriedItemIds = new List<int>(player.ItemInventory);
                    foreach (int itemId in carriedItemIds)
                    {
                        currentRoom.ContainedItemIds.Add(itemId);
                        player.ItemInventory.Remove(itemId);
                        Console.WriteLine("You dropped all carried items into the room!");
                    }
                    break;
                default:
                    var carriedItems = ObjectFinder.GetItems(gameData.Items, player.ItemInventory);
                    var carriedItem = ObjectFinder.GetItem(carriedItems, noun.Name);
                    if (carriedItem != null)
                    {
                        currentRoom.ContainedItemIds.Add(carriedItem.Id);
                        player.ItemInventory.Remove(carriedItem.Id);
                        Console.WriteLine($"You dropped: {carriedItem.Name}");
                    }
                    else
                    {
                        Console.Write("You're not carrying that item!\n");
                    }
                    break;
            }
        }
    }

    private static void ProcessGo(Word? noun, GameDatabase gameData, Player player)
    {
        if (noun != null)
        {
            Room currentRoom = ObjectFinder.GetRoom(gameData.Rooms, player.CurrentRoomId);
            int roomId = Constants.ROOM_ID_UNSET;
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
                case "up":
                    roomId = currentRoom.ExitIdUp;
                    break;
                case "down":
                    roomId = currentRoom.ExitIdDown;
                    break;
            }

            if (roomId != Constants.ROOM_ID_UNSET)
            {
                Console.Write(MessageFormatter.Look(roomId, gameData, player) + "\n");
                player.CurrentRoomId = roomId;
            }
            else
            {
                Console.Write("You can't go in that direction!!\n");
            }
        }
        else
        {
            Console.Write("Go where??\n");
        }
    }

    private static void ProcessLook(Word? noun, GameDatabase gameData, Player player)
    {
        if (noun == null) // look with no noun means to look at the room.
            Console.Write(MessageFormatter.Look(player.CurrentRoomId, gameData, player, true) + "\n");
        else
        {
            switch (noun.Name)
            {
                case "inventory":
                    Console.Write(MessageFormatter.Inventory(player, gameData) + "\n");
                    break;
                default:
                    // assume that we're looking at an item either in inventory or in the room
                    Room currentRoom = ObjectFinder.GetRoom(gameData.Rooms, player.CurrentRoomId);
                    var playerItems = ObjectFinder.GetItems(gameData.Items, player.ItemInventory);
                    var roomItems = ObjectFinder.GetItems(gameData.Items, currentRoom.ContainedItemIds);

                    Item? lookedItem = ObjectFinder.GetItem(playerItems, noun.Name);
                    if (lookedItem == null)
                    {
                        lookedItem = ObjectFinder.GetItem(roomItems, noun.Name);
                    }

                    if (lookedItem != null)
                    {
                        Console.Write($"{lookedItem.Name}: {lookedItem.Description}\n");
                    }
                    else
                    {
                        Console.Write("You don't see that here!\n");
                    }

                    break;
            }
        }
    }

    private static bool ProcessScore(GameDatabase gameData)
    {
        bool quitGame = false;
        Room treasureRoom = ObjectFinder.GetRoom(gameData.Rooms, gameData.TreasureRoomId);
        int countTreasures = gameData.Items.Where(i => i.IsTreasure).Count();
        int storedTreasures = ObjectFinder.GetItems(gameData.Items, treasureRoom.ContainedItemIds).Count(i => i.IsTreasure);
        float percentComplete = storedTreasures / countTreasures;
        Console.Write($"You have stored {storedTreasures} out of {countTreasures} treasures in the treasure room.\nYou have a score of {percentComplete:P0}.\n");
        if (percentComplete == 1)
        {
            Console.Write("CONGRATULATIONS!!! YOU WIN!!!\n");
            quitGame = true;
        }

        return quitGame;
    }

    private static void ProcessTake(Word? noun, GameDatabase gameData, Player player)
    {
        if (noun == null)
        {
            // Need a noun to pick up
            Console.WriteLine("Take what??");
        }
        else
        {
            Room currentRoom = ObjectFinder.GetRoom(gameData.Rooms, player.CurrentRoomId);
            var itemsInRoom = ObjectFinder.GetItems(gameData.Items, currentRoom.ContainedItemIds);
            switch (noun.Name)
            {
                case "all":
                    // pick everything up from the room that can be taken
                    // and remove everything picked up from the room.
                    foreach (Item item in itemsInRoom.Where(i => i.CanTake))
                    {
                        player.ItemInventory.Add(item.Id);
                        currentRoom.ContainedItemIds.Remove(item.Id);
                    }
                    Console.WriteLine("You picked all the items from the room!");
                    break;
                default:
                    var takenItem = ObjectFinder.GetItem(itemsInRoom, noun.Name);
                    if (takenItem != null)
                    {
                        if (takenItem.CanTake)
                        {
                            player.ItemInventory.Add(takenItem.Id);
                            currentRoom.ContainedItemIds.Remove(takenItem.Id);
                            Console.WriteLine($"You picked up: {takenItem.Name}");
                        }
                        else
                        {
                            Console.WriteLine("You can't take that!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You don't see that here!");
                    }
                    break;
            }
        }
    }
}