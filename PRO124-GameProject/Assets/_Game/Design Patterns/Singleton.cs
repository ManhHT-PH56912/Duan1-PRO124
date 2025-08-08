using UnityEngine;

namespace DesignPatterns.Singleton
{
    /// <summary>
    /// Generic Singleton base class for MonoBehaviour types.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        /// <summary>
        /// Global access to the singleton instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();

                    if (_instance == null)
                    {
                        CreateInstance();
                    }
                    else
                    {
                        Debug.Log($"[Singleton] Using existing instance of {typeof(T).Name} on {_instance.gameObject.name}");
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Ensures only one instance exists and persists between scenes.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Creates a new singleton instance if none exists in the scene.
        /// </summary>
        private static void CreateInstance()
        {
            GameObject singletonObj = new GameObject(typeof(T).Name);
            _instance = singletonObj.AddComponent<T>();
            DontDestroyOnLoad(singletonObj);
        }
    }
}
