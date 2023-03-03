using UnityEngine;

namespace _Projects.Scripts
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = FindObjectOfType<T>();
                if (instance != null) return instance;
                GameObject go = new GameObject
                {
                    name = typeof(T).Name
                };
                instance = go.AddComponent<T>();
                return instance;
            }
        }
    }
}