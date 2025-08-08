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
    public string userid;
    public StatsData stats;
    public InventoryData inventory;
    public SettingsData settings;
}
