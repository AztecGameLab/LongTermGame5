using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Serializable]
    public struct InteractEvents
    {
        // Passes the GameObject that triggered the event.
        
        public UnityEvent<GameObject> onInteract;
        public UnityEvent<GameObject> onEnterRange;
        public UnityEvent<GameObject> onExitRange;
    }
    
    [SerializeField] private float interactRadius;
    [SerializeField] private InteractEvents events;
    [SerializeField] private bool showDebug = true;
    
    [SerializeField, Tooltip("Specify layers that may interact with this object")] 
    private LayerMask interactionMask = default;
    
    private List<GameObject> _objectsInRange;
    private bool HasObjectsInRange => _objectsInRange.Count > 0;

    public void Interact(GameObject caller)
    {
        events.onInteract.Invoke(caller);
    }

    private void Awake()
    {
        var interactCollider = gameObject.AddComponent<CircleCollider2D>();
        interactCollider.isTrigger = true;
        interactCollider.radius = interactRadius;

        _objectsInRange = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_objectsInRange.Contains(other.gameObject) || (other.gameObject.layer & interactionMask) == 0)
            return;
        
        events.onEnterRange.Invoke(other.gameObject);
        _objectsInRange.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_objectsInRange.Contains(other.gameObject) || (other.gameObject.layer & interactionMask) == 0)
            return;
        
        events.onExitRange.Invoke(other.gameObject);
        _objectsInRange.Remove(other.gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!showDebug)
            return;
        
        Gizmos.color = HasObjectsInRange ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, interactRadius);
    }
}