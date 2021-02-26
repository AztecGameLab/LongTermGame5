using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
// Events that provide information about a collision; these handled well by scripts
public struct CollisionEventsWithData 
{
    public UnityEvent<GameObject> collisionEnterWithData;
    public UnityEvent<GameObject> collisionExitWithData;
}

[Serializable]
// Events that don't provide collision information; these are handled well by the editor
public struct CollisionEvents 
{
    public UnityEvent collisionEnter;
    public UnityEvent collisionExit;
}

// Wraps a trigger Collider and provides UnityEvents for OnTriggerEnter / Exit / Stay
public class Trigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private Collider2D colliderComponent;
    [SerializeField] private LayerMask layersThatCanTrigger;
    [SerializeField] private bool showDebug = true;
    
    [Header("Trigger Events")]
    [SerializeField] private CollisionEvents events;
    [SerializeField] private CollisionEventsWithData eventsWithData;

    private GameObject _collidedObject;
    private readonly List<GameObject> _objectsInTrigger = new List<GameObject>(); 

    public bool HasObjectInTrigger(GameObject obj)
    {
        return _objectsInTrigger.Contains(obj);
    }
    
    private void Awake()
    {
        if (colliderComponent == null)
            colliderComponent = gameObject.AddComponent<CircleCollider2D>();

        colliderComponent.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _collidedObject = other.gameObject;

        if (ObjectIsInLayerMask())
            AddObjectAndFireEvents();    
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _collidedObject = other.gameObject;

        if (ObjectIsInLayerMask())
            RemoveObjectAndFireEvents();
    }

    private bool ObjectIsInLayerMask()
    {
        return (1 << _collidedObject.layer & layersThatCanTrigger) != 0;
    }

    private void AddObjectAndFireEvents()
    {
        _objectsInTrigger.Add(_collidedObject);
        
        events.collisionEnter?.Invoke();
        eventsWithData.collisionEnterWithData?.Invoke(_collidedObject);
    }
    
    private void RemoveObjectAndFireEvents()
    {
        _objectsInTrigger.Remove(_collidedObject);
        
        events.collisionExit.Invoke();
        eventsWithData.collisionExitWithData?.Invoke(_collidedObject);
    }

#if UNITY_EDITOR

    private readonly Color _red = new Color(1, 0, 0, 0.1f);
    private readonly Color _green = new Color(0, 1, 0, 0.1f);
    private bool HasObjectsInRange => _objectsInTrigger.Count > 0;

    private void OnDrawGizmos()
    {
        if (!showDebug)
            return;

        var bounds = colliderComponent.bounds;
        var colliderRect = new Rect(bounds.min, bounds.size);
        var color = HasObjectsInRange ? _green : _red;

        UnityEditor.Handles.DrawSolidRectangleWithOutline(colliderRect, color, color);
    }

#endif
}