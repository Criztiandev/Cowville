#nullable enable
using UnityEngine;
using App.Shared.Scripts.Base;

namespace App.Features.Managers.Scripts
{
    public class EconomyManager : Singleton<EconomyManager>
    {
        [Header("Economy Settings")]
        [Tooltip("The initial balance of coins.")]
        public int StartingCoins = 100;
        
        [Tooltip("The cost of basic items.")]
        public int BasicItemCost = 10;
        
        [Tooltip("The time delay before economy updates.")]
        public float UpdateDelayTimer = 1.5f;

        [Tooltip("Current amount of coins.")]
        public int CurrentCoins = 0;

        public void AddMoney(int amount)
        {
            CurrentCoins += amount;
            // Optionally dispatch an SO event here in the future
            Debug.Log($"[EconomyManager] Added {amount} money. Total Coins: {CurrentCoins}");
        }
    }
}
