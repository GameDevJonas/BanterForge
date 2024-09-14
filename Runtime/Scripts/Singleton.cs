using System;
using UnityEngine;

namespace GDPanda.Data
{
    [DefaultExecutionOrder(-100)]
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T _instance;
        
        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            // Debug.Log(gameObject, gameObject);
            _instance = this.GetComponent<T>();
        }

        public static T GetInstance()
        {
            return _instance;
        }

        public static bool IsInstance(T instance)
        {
            return _instance == instance;
        }
        
        private void OnDestroy()
        {
            if(!IsInstance(this as T))
                return;

            _instance = null;
        }
    }
}