#nullable enable
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Shared.Scripts.Input
{
    /// <summary>
    /// Sample script demonstrating how to use the Unity Event System for 2D objects via IPointerDownHandler.
    /// Requires a Collider2D and a Main Camera with a Physics 2D Raycaster component.
    /// Do NOT use OnMouseDown as it is a legacy feature.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class SamplePointerDownHandler : MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"[{nameof(SamplePointerDownHandler)}] Pointer down detected on '{gameObject.name}' exactly at {eventData.position}");
        }
    }
}
