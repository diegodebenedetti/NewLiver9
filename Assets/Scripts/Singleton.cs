using UnityEngine;

namespace Core
{
    public class Singleton<T> : MonoBehaviour
        where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var _objs = FindObjectsOfType(typeof(T)) as T[];
                    if (_objs.Length > 0)
                        _instance = _objs[0];
                    if (_objs.Length > 1)
                    {
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    }
                    if (_instance == null)
                    {
                        var _obj = new GameObject(string.Format("_{0}", typeof(T).Name));
                        _instance = _obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
}