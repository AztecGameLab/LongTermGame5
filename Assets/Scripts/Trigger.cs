using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
// Events that provide information about a collision; these handled well by scripts
public struct CollisionEventsWithData 
{
    public UnityEvent<GameObject> collisionEnterWithData;
    public UnityEvent<GameObject> collisionExitWithData;
    public UnityEvent<GameObject> collisionStayWithData;
}

[Serializable]
// Events that don't provide collision information; these are handled well by the editor
public struct CollisionEvents 
{
    public UnityEvent collisionEnter;
    public UnityEvent collisionExit;
    public UnityEvent collisionStay;
}

// Wraps a trigger Collider and provides UnityEvents for OnTriggerEnter / Exit / Stay
public class Trigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private Collider2D colliderComponent = default;
    [SerializeField] private LayerMask layerMask = default;
    [SerializeField] private bool showDebug = true;
    [Header("Trigger Events")]
    [SerializeField] private CollisionEvents events = new CollisionEvents();
    [SerializeField] private CollisionEventsWithData eventsWithData = new CollisionEventsWithData();
    
    private readonly List<GameObject> _triggeringObjects = new List<GameObject>(); 
    private bool HasObjectsInRange => _triggeringObjects.Count > 0;

    public ReadOnlyCollection<GameObject> TriggeringObjects { get; private set; }
    
    private void Awake()
    {
        if (colliderComponent == null)
            colliderComponent = gameObject.AddComponent<Collider2D>();

        colliderComponent.isTrigger = true;
        TriggeringObjects = _triggeringObjects.AsReadOnly();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggeringObjects.Contains(other.gameObject) || (1 << other.gameObject.layer & layerMask) == 0)
            return;
        
        _triggeringObjects.Add(other.gameObject);
        TriggeringObjects = _triggeringObjects.AsReadOnly();
        
        events.collisionEnter?.Invoke();
        eventsWithData.collisionEnterWithData?.Invoke(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_triggeringObjects.Contains(other.gameObject) || (1 << other.gameObject.layer & layerMask) == 0)
            return;
        
        _triggeringObjects.Remove(other.gameObject);
        TriggeringObjects = _triggeringObjects.AsReadOnly();
        
        events.collisionExit.Invoke();
        eventsWithData.collisionExitWithData?.Invoke(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        events.collisionStay?.Invoke();
        eventsWithData.collisionStayWithData?.Invoke(other.gameObject);
    }

#if UNITY_EDITOR

    private readonly Color _red = new Color(1, 0, 0, 0.1f);
    private readonly Color _green = new Color(0, 1, 0, 0.1f);
    
    private void OnDrawGizmos()
    {
        if (!showDebug)
            return;

        var bounds = colliderComponent.bounds;
        var colliderRect = new Rect(bounds.min, bounds.size);
        var color = HasObjectsInRange ? _green : _red;

        var redStyle = new GUIStyle {normal = {textColor = Color.black}};

        UnityEditor.Handles.Label(Vector3.zero, $"Trigger: { gameObject.name }", redStyle);
        UnityEditor.Handles.DrawSolidRectangleWithOutline(colliderRect, color, color);
    }

#endif
}
