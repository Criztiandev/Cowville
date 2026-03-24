#nullable enable
using UnityEngine;
using App.Shared.Scripts.Combat;
using App.Features.Managers.Scripts;
using App.Shared.Scripts.UI;

namespace App.Features.Nexus.Scripts
{
    /// <summary>
    /// Bridges the low-level Health component of the Nexus to the high-level GameManager states.
    /// Acts as the dedicated controller so the Health component remains decoupled and strictly about combat math.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class NexusController : MonoBehaviour
    {
        private Health _health = null!;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            _health.OnDeath.AddListener(HandleNexusDeath);
            _health.OnDamageTaken.AddListener(HandleDamageTaken);
        }

        private void OnDisable()
        {
            _health.OnDeath.RemoveListener(HandleNexusDeath);
            _health.OnDamageTaken.RemoveListener(HandleDamageTaken);
        }

        private void HandleNexusDeath()
        {
            GameManager.Instance.TriggerBrokenState();
        }

        private void HandleDamageTaken(int damageAmount)
        {
            // Spawn float text at the Nexus origin + an offset so it doesn't overlap perfectly with money
            FloatingTextPool.Instance.SpawnDamageText(transform.position + Vector3.up * 0.5f, damageAmount);
        }
    }
}
