#nullable enable
using UnityEngine;
using UnityEngine.InputSystem;
using App.Shared.Scripts.Interfaces;

namespace App.Features.Nexus.Scripts
{
    /// <summary>
    /// Temporary script to test IDamageable on the Nexus.
    /// Press the spacebar to deal 10 damage using the New Input System!
    /// </summary>
    [RequireComponent(typeof(IDamageable))]
    public class NexusDamageTester : MonoBehaviour
    {
        private IDamageable? _damageable;

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
        }

        private void Update()
        {
            // Checks for spacebar press directly from the New Input System
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                _damageable?.TakeDamage(10);
            }
        }
    }
}
