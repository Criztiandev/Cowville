#nullable enable
using UnityEngine;
using UnityEngine.Events;
using App.Shared.Scripts.Interfaces;
using App.Shared.Scripts.Stats;

namespace App.Shared.Scripts.Combat
{
    /// <summary>
    /// Universal health component. Combines base EntityStatsSO values with instance health tracking.
    /// </summary>
    public class Health : MonoBehaviour, IDamageable
    {
        [Tooltip("Optional scriptable object to initialize base stats.")]
        [SerializeField] private EntityStatsSO? _baseStats;

        [Tooltip("Default Max Health if no SO is provided.")]
        [SerializeField] private int _maxHealthFallback = 100;

        private int _currentHealth;
        private int _maxHealth;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public UnityEvent<int, int> OnHealthChanged = new UnityEvent<int, int>();
        public UnityEvent<int> OnDamageTaken = new UnityEvent<int>();
        public UnityEvent OnDeath = new UnityEvent();

        private void Awake()
        {
            _maxHealth = _baseStats != null ? _baseStats.MaxHealth : _maxHealthFallback;
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0 || _currentHealth <= 0) return;

            // Clamp health strictly between 0 and MaxHealth
            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
            
            OnDamageTaken.Invoke(amount);
            OnHealthChanged.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                OnDeath.Invoke();
            }
        }
    }
}
