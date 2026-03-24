#nullable enable
using UnityEngine;
using UnityEngine.EventSystems;
using App.Features.Managers.Scripts;

namespace App.Features.Nexus.Scripts
{
    /// <summary>
    /// Handles rapid click and touch interactions on the Nexus object.
    /// EventSystems guarantees IPointerDownHandler fires immediately upon touch/mouse down, preventing dropped rapid inputs.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class NexusInteraction : MonoBehaviour, IPointerDownHandler
    {
        [Tooltip("Amount of money granted per tap.")]
        [SerializeField] private int _moneyPerTap = 1;

        public void OnPointerDown(PointerEventData eventData)
        {
            EconomyManager.Instance.AddMoney(_moneyPerTap);
        }
    }
}
