using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Dependencies")] 
    [SerializeField] private Trigger trigger = default;
    
    [Header("Interactable Events")]
    [SerializeField] public UnityEvent<GameObject> onInteract;
    
    private void Awake()
    {
        if (trigger == null)
            gameObject.AddComponent<Trigger>();
    }

    public void Interact(GameObject caller)
    {
        print(trigger.HasObjectInTrigger(caller));
        
        if (trigger.HasObjectInTrigger(caller))
        {
            print("interacted " + gameObject.name);
            onInteract.Invoke(caller);
        }
    }
}