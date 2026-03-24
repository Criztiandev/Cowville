#nullable enable
using UnityEngine;
using App.Shared.Scripts.Base;

namespace App.Features.Managers.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        public void TriggerBrokenState()
        {
            Debug.Log("[GameManager] Nexus has reached 0 HP! Triggering Broken State.");
            // Broken state logic implementation goes here
        }
    }
}
