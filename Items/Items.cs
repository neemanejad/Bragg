using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redwood.Items
{
    public delegate void ItemEffect(Player player);

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
        /// <summary>
        /// Function for item to affect the player in some way.
        /// </summary>
        public ItemEffect Effect { get; set; }

        public Item(string name, string descrition, ItemId id, ItemEffect effect)
        {
            Name = name;
            Description = descrition;
            ID = id;
            Effect = effect;
        }

        public static Dictionary<ItemId, ItemEffect> ItemEffects = new Dictionary<ItemId, ItemEffect>
        {
            { ItemId.SPEED_BOOST, (Player player) => { player.Speed *= 1.30; } },
        };

        public static Dictionary<ItemId, Item> ItemDictionary = new Dictionary<ItemId, Item>
        {
            { ItemId.SPEED_BOOST,  new Item(name: "Speed Boost", descrition: "Gotta go fast", id: ItemId.SPEED_BOOST, effect: ItemEffects[ItemId.SPEED_BOOST])},
        };

        public static Item GetItem(ItemId id)
        {
            return ItemDictionary[id];
        }

    };
}
