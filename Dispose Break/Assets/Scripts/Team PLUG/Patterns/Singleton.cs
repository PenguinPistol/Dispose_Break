using UnityEngine;

namespace com.TeamPlug.Patterns
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    }
                }

                return instance;
            }
        }
    }
}
