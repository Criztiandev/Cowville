#nullable enable
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

namespace App.Shared.Scripts.UI
{
    /// <summary>
    /// Handles the upward drift and fade of a floating text element.
    /// Operates on a simple flat Update loop for zero GC allocations.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class FloatingText : MonoBehaviour
    {
        private TMP_Text _textComponent = null!;
        private ObjectPool<FloatingText>? _pool;

        private float _timer;
        private const float Lifetime = 0.6f;
        private const float MoveSpeed = 1.0f / 0.6f; // Exactly 1 unit over 0.6 seconds

        private void Awake()
        {
            _textComponent = GetComponent<TMP_Text>();
        }

        public void Initialize(Vector2 startPos, string text, ObjectPool<FloatingText> pool)
        {
            transform.position = startPos;
            
            // Using compile-time string literally or .SetText() is necessary to avoid GC allocation.
            _textComponent.text = text;
            _textComponent.alpha = 1f;
            
            _pool = pool;
            _timer = 0f;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            
            // Drift upward
            transform.position += Vector3.up * (MoveSpeed * Time.deltaTime);

            // Fade alpha (1 to 0) over Lifetime
            float currentAlpha = 1f - (_timer / Lifetime);
            _textComponent.alpha = Mathf.Clamp01(currentAlpha);

            // Return to pool after lifetime
            if (_timer >= Lifetime)
            {
                _pool?.Release(this);
            }
        }
    }
}
