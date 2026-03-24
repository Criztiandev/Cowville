#nullable enable
using UnityEngine;
using UnityEngine.Pool;
using App.Shared.Scripts.Base;

namespace App.Shared.Scripts.UI
{
    /// <summary>
    /// Manages an ObjectPool of FloatingText components.
    /// Allocates 20 instances precisely at startup to prevent frame hitches or runtime Instantiation.
    /// </summary>
    public class FloatingTextPool : Singleton<FloatingTextPool>
    {
        [Header("Pool Setup")]
        [Tooltip("The FloatingText prefab to spawn.")]
        [SerializeField] private FloatingText _prefab = null!;

        private ObjectPool<FloatingText> _pool = null!;

        protected override void Awake()
        {
            base.Awake();
            
            _pool = new ObjectPool<FloatingText>(
                createFunc: () => Instantiate(_prefab),
                actionOnGet: ft => ft.gameObject.SetActive(true),
                actionOnRelease: ft => ft.gameObject.SetActive(false),
                actionOnDestroy: ft => Destroy(ft.gameObject),
                collectionCheck: false,
                defaultCapacity: 20,
                maxSize: 100
            );

            // Pre-allocate 20 objects at startup to guarantee no hitches during early gameplay
            var preallocated = new FloatingText[20];
            for (int i = 0; i < 20; i++)
            {
                preallocated[i] = _pool.Get();
            }
            for (int i = 0; i < 20; i++)
            {
                _pool.Release(preallocated[i]);
            }
        }

        public void Spawn(Vector2 position, string text)
        {
            var floatingText = _pool.Get();
            floatingText.Initialize(position, text, _pool);
        }
    }
}
