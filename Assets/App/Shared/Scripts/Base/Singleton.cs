#nullable enable
using UnityEngine;

namespace App.Shared.Scripts.Base
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; } = null!;

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T ?? throw new System.InvalidOperationException();
        }
    }
}
