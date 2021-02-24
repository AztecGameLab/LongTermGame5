using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Trigger))]
public class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    [SerializeField] private float interactRadius = 1f;
    
    [Header("Interactable Events")]
    public UnityEvent<GameObject> onInteract;


    public void Interact(GameObject caller)
    {
        onInteract.Invoke(caller);
    }

    private void Awake()
    {
        var interactCollider = gameObject.AddComponent<CircleCollider2D>();
        interactCollider.isTrigger = true;
        interactCollider.radius = interactRadius;
    }
}