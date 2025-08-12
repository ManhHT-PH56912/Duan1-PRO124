using System;
using System.Collections.Generic;

[Serializable]
public class PlayerDataModel
{
    public string userId;
    public string email;
    public Stats stats = new Stats();
    public Inventory inventory = new Inventory();
    public Settings settings = new Settings();

    [Serializable]
    public class Stats
    {
        public int level;
        public int exp;
        public int coins;
        public int health;
        public int mana;
        public int checkpoint;
    }

    [Serializable]
    public class Item
    {
        public string id;
        public string name;
        public int quantity;
    }

    [Serializable]
    public class Inventory
    {
        public List<Item> items = new List<Item>();
    }

    [Serializable]
    public class Settings
    {
        public float musicVolume;
        public float sfxVolume;
        public bool isMusicOn;
        public bool isSfxOn;
    }

    /// <summary>
    /// Resets the player data to default values.
    /// </summary>
    public void ResetDefaultData(string userId, string email)
    {
        this.userId = userId;
        this.email = email;

        stats = new Stats
        {
            level = 0,
            exp = 0,
            coins = 0,
            health = 0,
            mana = 0,
            checkpoint = 0
        };

        inventory = new Inventory
        {
            items = new List<Item>
            {
                new Item { id = "potion_hp", name = "Thuốc Hồi Máu", quantity = 0 },
                new Item { id = "potion_mana", name = "Thuốc Hồi Mana", quantity = 0 }
            }
        };

        settings = new Settings
        {
            musicVolume = 1f,
            sfxVolume = 0.5f,
            isMusicOn = true,
            isSfxOn = true
        };
    }
    /// <summary>
    /// Ensure default items are present in the inventory.
    /// </summary>
    public void EnsureDefaultItems()
    {
        var defaultItems = new List<Item>
    {
        new Item { id = "potion_hp", name = "Thuốc Hồi Máu", quantity = 0 },
        new Item { id = "potion_mana", name = "Thuốc Hồi Mana", quantity = 0 }
    };

        foreach (var def in defaultItems)
        {
            var existing = inventory.items.Find(i => i.id == def.id);
            if (existing == null)
            {
                inventory.items.Add(def);
            }
        }
    }
}
