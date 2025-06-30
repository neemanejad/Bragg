using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Items
{
    public enum ItemId
    {
        SPEED_BOOST,
        DAMAGE_BOOST,
        FASTER_FIRE_RATE
    }

    public interface IItemNode
    {
        ItemId ItemId { get; }
    }

    public partial class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemId ID { get; set; }

        public Item(string name, string descrition, ItemId id)
        {
            Name = name;
            Description = descrition;
            ID = id;
        }

        public static Dictionary<ItemId, Item> ItemDictionary = new Dictionary<ItemId, Item>
        {
            { ItemId.SPEED_BOOST,  new Item(name: "Speed Boost", descrition: "Gotta go fast", id: ItemId.SPEED_BOOST)},
            { ItemId.DAMAGE_BOOST, new Item(name: "Damage Boost", descrition: "Bigger number = better", id: ItemId.DAMAGE_BOOST) },
            { ItemId.FASTER_FIRE_RATE, new Item(name: "Faster Fire Rate", descrition: "More bullets = better", id: ItemId.FASTER_FIRE_RATE) },
        };

        public static Item GetItem(ItemId id)
        {
            return ItemDictionary[id];
        }

    };
}
