using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Milo.Tools
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance { get; set; }

        public static T Instance => _instance;

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else if (_instance == null)
            {
                _instance = this as T;
            }
        }
    }
}