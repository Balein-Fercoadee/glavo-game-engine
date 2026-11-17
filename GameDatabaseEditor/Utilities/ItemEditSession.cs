using System.Collections.ObjectModel;
using GameEngine.GameData;

namespace GameDatabaseEditor.Utilities;

internal sealed class ItemEditSession
{
    private readonly ObservableCollection<Item> _items;
    private readonly Item _item;

    public Item Draft { get; }

    public ItemEditSession(ObservableCollection<Item> items, Item item)
    {
        _items = items;
        _item = item;
        Draft = new Item
        {
            Id = item.Id,
            Name = item.Name,
            ShortName = item.ShortName,
            Description = item.Description,
            CanTake = item.CanTake,
            IsTreasure = item.IsTreasure
        };
    }

    public bool Save()
    {
        int index = _items.IndexOf(_item);
        if (index < 0)
        {
            return false;
        }

        _items[index] = Draft;
        return true;
    }
}
