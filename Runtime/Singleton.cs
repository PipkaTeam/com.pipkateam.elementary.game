using UnityEngine;

namespace Elementary.Game
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Elementary Game][Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (FindObjectsOfType<T>().Length > 1)
                        {
                            Debug.LogError($"[Elementary Game][Singleton] Something went really wrong - there should never be more than 1 singleton of type '{typeof(T)}'! Reopening the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            Debug.LogError($"[Elementary Game][Singleton] Instance of '{typeof(T)}' is needed in the scene but does not exist. Make sure you have added an instance of '{typeof(T)}' to the scene.");
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _applicationIsQuitting = true;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        protected virtual void Awake()
        {
            lock (_lock)
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
        }

        protected virtual void Start()
        {
            Debug.Log($"[Elementary Game][Singleton] Instance of '{typeof(T)}' started.");
        }
    }
}