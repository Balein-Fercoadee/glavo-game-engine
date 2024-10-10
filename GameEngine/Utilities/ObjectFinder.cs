
using System.Runtime.CompilerServices;
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
}
