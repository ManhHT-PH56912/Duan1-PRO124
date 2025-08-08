using UnityEngine;

public class TestFirebaseData : MonoBehaviour
{
    FirebaseManager firebaseManager;

    void Start()
    {
        if (firebaseManager == null)
        {
            firebaseManager = FindAnyObjectByType<FirebaseManager>();
        }

        // Tạo dữ liệu mẫu
        UserData newUser = new UserData
        {
            email = "lam@example.com",
            stats = new StatsData { level = 0, exp = 0, coins = 0, health = 0, mana = 80 },
            inventory = new InventoryData
            {
                items = new System.Collections.Generic.List<ItemData>
                {
                    new ItemData { id = "potion_hp", name = "Thuốc Hồi Máu", quantity = 2 },
                    new ItemData { id = "potion_mana", name = "Thuốc Hồi Mana", quantity = 1 },
                    new ItemData { id = "knife_fire", name = "Dao Cháy", quantity = 1 }
                }
            },
            settings = new SettingsData { musicVolume = 1f, sfxVolume = 0.5f },
            shop = new ShopData
            {
                availableItems = new System.Collections.Generic.List<ShopItem>
                {
                    new ShopItem { id = "potion_hp", name = "Thuốc Hồi Máu", heal = 50, cost = 2 },
                    new ShopItem { id = "potion_mana", name = "Thuốc Hồi Mana", manaRestore = 50, cost = 1 }
                }
            },
            lastLogin = System.DateTime.UtcNow.ToString("o")
        };

        // Lưu dữ liệu
        firebaseManager.CreateOrUpdateUserData(newUser);

        // Đọc dữ liệu
        firebaseManager.ReadUserData((data) =>
        {
            if (data != null)
                Debug.Log("Đọc thành công! Email: " + data.email);
        });

        // Update chỉ 1 field
        // firebaseManager.UpdateUserField("stats/coins", 100);

        // Xóa dữ liệu
        // firebaseManager.DeleteUserData();
    }
}
