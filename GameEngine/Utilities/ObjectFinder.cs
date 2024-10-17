using GameEngine.GameData;

namespace GameEngine.Utilities;

public static class ObjectFinder
{
    /// <summary>
    /// Gets a Room object from its Room.Id.
    /// </summary>
    /// <param name="rooms"></param>
    /// <param name="roomId"></param>
    /// <returns></returns>
    public static Room GetRoom(IEnumerable<Room> rooms, int roomId)
    {
        return rooms.Where(r => r.Id == roomId).First();
    }

    public static List<Item> GetItems(IEnumerable<Item> items, List<int> itemIds)
    {
        return items.Where(i => itemIds.Contains(i.Id)).ToList<Item>();
    }

    public static Item? GetItem(IEnumerable<Item> items, int itemId)
    {
        return items.Where(i => i.Id == itemId).FirstOrDefault();
    }

    public static Item? GetItem(IEnumerable<Item> items, string itemShortName)
    {
        return items.Where(i => i.ShortName == itemShortName).FirstOrDefault();
    }

    public static Message? GetMessage(IEnumerable<Message> messages, int messageId)
    {
        return messages.Where(m=>m.Id==messageId).FirstOrDefault();
    }

    /// <summary>
    /// Gets the <c>Word object from a string.
    /// </summary>
    /// <param name="words">A collection of Words</param>
    /// <param name="command">The string command the translates to a Word.</param>
    /// <returns>The translated Word. If a Word wasn't found, null is returned.</returns>
    public static Word? GetWord(IEnumerable<Word> words, string command)
    {
        Word? word = null;

        word = words.Where(w => w.Name == command).FirstOrDefault();
        if (word == null)
            word = words.Where(w => w.Aliases.Contains(command)).FirstOrDefault();

        return word;
    }
}
