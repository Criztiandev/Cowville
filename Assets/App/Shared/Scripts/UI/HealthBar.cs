#nullable enable
using UnityEngine;
using UnityEngine.UI;

namespace App.Shared.Scripts.UI
{
    /// <summary>
    /// A decoupled UI component intended to listen to Health.OnHealthChanged via UnityEvent.
    /// Supports either a Slider or an Image (Fill Amount) for maximum UI flexibility.
    /// Utilizes a CanvasGroup to smoothly auto-hide when out of combat.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class HealthBar : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("The UI Slider to display health.")]
        [SerializeField] private Slider? _healthSlider;
        
        [Tooltip("Optional: Image to use fillAmount instead of a Slider.")]
        [SerializeField] private Image? _healthFill;

        [Header("Visibility Modes")]
        [Tooltip("Should the health bar auto-hide when not taking damage?")]
        [SerializeField] private bool _autoHide = true;
        
        [Tooltip("Time in seconds to wait before fading out the health bar.")]
        [SerializeField] private float _visibleDuration = 2.0f;

        private CanvasGroup _canvasGroup = null!;
        private float _hideTimer;
        private const float FadeDuration = 0.5f;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            // Start completely invisible if autoHide is enabled
            if (_autoHide)
            {
                _canvasGroup.alpha = 0f;
            }
        }

        private void Update()
        {
            if (!_autoHide || _hideTimer <= -FadeDuration) return;

            _hideTimer -= Time.deltaTime;

            if (_hideTimer <= 0f)
            {
                // Fade from 1 down to 0 over 0.5 seconds smoothly using CanvasGroup
                float fadeRatio = 1f - (Mathf.Abs(_hideTimer) / FadeDuration);
                _canvasGroup.alpha = Mathf.Clamp01(fadeRatio);
            }
        }

        /// <summary>
        /// Call this directly from the Health component's OnHealthChanged UnityEvent in the Inspector!
        /// </summary>
        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            if (maxHealth <= 0) return;

            float fillRatio = (float)currentHealth / maxHealth;

            if (_healthSlider != null)
            {
                _healthSlider.value = fillRatio;
            }

            if (_healthFill != null)
            {
                _healthFill.fillAmount = fillRatio;
            }

            // Immediately display bar at full opacity and restart the hide timer
            if (_autoHide)
            {
                _hideTimer = _visibleDuration;
                _canvasGroup.alpha = 1f;
            }
        }
    }
}
