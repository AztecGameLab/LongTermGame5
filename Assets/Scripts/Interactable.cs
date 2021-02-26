using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] public UnityEvent<GameObject> onInteract;
    
    [Header("Interactable Settings")] 
    [SerializeField] private Trigger trigger = default;
    
    public void Interact(GameObject caller)
    {
        if (ObjectCanInteract(caller))
            onInteract.Invoke(caller);
    }

    private bool ObjectCanInteract(GameObject obj)
    {
        return 
            trigger != null && 
            trigger.HasObjectInTrigger(obj) && 
            gameObject.activeInHierarchy;
    }
}