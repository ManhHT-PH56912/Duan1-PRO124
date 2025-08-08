using System;
using System.Collections.Generic;

[Serializable]
public class ItemData
{
    public string id;
    public string name;
    public int quantity;
}

[Serializable]
public class InventoryData
{
    public List<ItemData> items;
}

[Serializable]
public class ShopItem
{
    public string id;
    public string name;
    public int heal; // có thể 0 nếu không dùng
    public int manaRestore; // có thể 0 nếu không dùng
    public int cost;
}

[Serializable]
public class ShopData
{
    public List<ShopItem> availableItems;
}

[Serializable]
public class StatsData
{
    public int level;
    public int exp;
    public int coins;
    public int health;
    public int mana;
}

[Serializable]
public class SettingsData
{
    public float musicVolume;
    public float sfxVolume;
}

[Serializable]
public class UserData
{
    public string email;
    public StatsData stats;
    public InventoryData inventory;
    public SettingsData settings;
    public ShopData shop;
    public string lastLogin;
}
