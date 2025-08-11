using UnityEngine;
using System.Collections.Generic;

namespace Utils
{
    public static class ResourceLoader
    {
        /// <summary>
        /// Load 1 asset từ Resources (generic)
        /// </summary>
        public static T Load<T>(string path) where T : Object
        {
            T asset = Resources.Load<T>(path);
            if (asset == null)
                Debug.LogError($"[ResourceLoader] Không tìm thấy asset tại path: {path}");
            return asset;
        }

        /// <summary>
        /// Load tất cả asset từ thư mục trong Resources
        /// </summary>
        public static T[] LoadAll<T>(string path) where T : Object
        {
            T[] assets = Resources.LoadAll<T>(path);
            if (assets == null || assets.Length == 0)
                Debug.LogWarning($"[ResourceLoader] Không tìm thấy asset nào tại folder: {path}");
            return assets;
        }

        /// <summary>
        /// Load prefab và tạo instance
        /// </summary>
        public static GameObject InstantiatePrefab(string path, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = Load<GameObject>(path);
            if (prefab != null)
                return Object.Instantiate(prefab, position, rotation);
            return null;
        }

        /// <summary>
        /// Load sprite theo tên file trong folder Resources/Sprites
        /// </summary>
        public static Sprite LoadSprite(string spriteName)
        {
            return Load<Sprite>($"Sprites/{spriteName}");
        }

        /// <summary>
        /// Load âm thanh từ folder Resources/Audio
        /// </summary>
        public static AudioClip LoadAudio(string audioName)
        {
            return Load<AudioClip>($"Audio/{audioName}");
        }
    }
}
