using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct CollisionEvents 
{
    public UnityEvent<GameObject> collisionEnter;
    public UnityEvent<GameObject> collisionExit;
}

// Wraps a Collider and exposes UnityEvents for OnTriggerEnter / Exit 
public class Trigger : MonoBehaviour
{
    [Header("Trigger Events")]
    [SerializeField] public CollisionEvents events;
    
    [Header("Trigger Settings")]
    [SerializeField] public Collider2D colliderComponent;
    [SerializeField] public LayerMask layersThatCanTrigger;
    [SerializeField] private bool showDebug = true;
    
    private readonly List<GameObject> _objectsInTrigger = new List<GameObject>(); 
    private GameObject _collidedObject;

    public bool HasObjectInTrigger(GameObject obj)
    {
        return _objectsInTrigger.Contains(obj);
    }
    
    private void Awake()
    {
        if (colliderComponent == null && !TryGetComponent(out colliderComponent))
        {
            Debug.LogWarning($"[Trigger] { gameObject.name } is missing a collider.");
        }
        else
        {
            colliderComponent.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _collidedObject = other.gameObject;

        if (ObjectCanTrigger())
            AddObjectAndFireEvent();    
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _collidedObject = other.gameObject;

        if (ObjectCanTrigger())
            RemoveObjectAndFireEvent();
    }

    private bool ObjectCanTrigger()
    {
        // Make sure the collided object falls within our layer mask.
        return (1 << _collidedObject.layer & layersThatCanTrigger) != 0;
    }

    private void AddObjectAndFireEvent()
    {
        _objectsInTrigger.Add(_collidedObject);
        events.collisionEnter?.Invoke(_collidedObject);
    }
    
    private void RemoveObjectAndFireEvent()
    {
        _objectsInTrigger.Remove(_collidedObject);
        events.collisionExit?.Invoke(_collidedObject);
    }

    #region DEBUG_CODE
    #if UNITY_EDITOR

        private readonly Color _red = new Color(1, 0, 0, 0.05f);
        private readonly Color _green = new Color(0, 1, 0, 0.05f);
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
    #endregion
}