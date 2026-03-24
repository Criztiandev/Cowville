#nullable enable

namespace App.Shared.Scripts.Interfaces
{
    /// <summary>
    /// Represents any entity that can take damage.
    /// Useful for decoupling collision logic from health mechanics.
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(int amount);
    }
}
