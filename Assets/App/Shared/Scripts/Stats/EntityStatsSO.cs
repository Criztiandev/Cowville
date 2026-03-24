#nullable enable
using UnityEngine;

namespace App.Shared.Scripts.Stats
{
    /// <summary>
    /// Shared data container for entity attributes, ensuring data is tunable without editing prefabs.
    /// </summary>
    [CreateAssetMenu(menuName = "Game/Entity Stats")]
    public class EntityStatsSO : ScriptableObject
    {
        [Tooltip("The maximum base health points.")]
        public int MaxHealth = 100;

        [Tooltip("The movement speed scale.")]
        public float MoveSpeed = 5f;

        [Tooltip("Base attack damage dealt upon combat.")]
        public int AttackDamage = 10;
    }
}
