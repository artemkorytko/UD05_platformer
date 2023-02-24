using UnityEngine;

namespace DefaultNamespace.Patterns.Singleton
{
    public abstract class BaseSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance // паттерн Singleton позволяет обращаться к публичним методам(или полям) без создания экземпляра класса  
        {
            get // ленивая(Lazy) ссылка, позволяет найти объект когда нам нужно(разгрузка старта)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        var obj = new GameObject("Singleton");
                        _instance = obj.AddComponent<T>();
                        DontDestroyOnLoad(obj);
                    }
                }
                
                return _instance;
            }
        }
    }
}