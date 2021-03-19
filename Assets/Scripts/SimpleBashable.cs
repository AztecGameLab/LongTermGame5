using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleBashable : MonoBehaviour, IBashable
{
    public void Bash()
    {
        print($"Bash! { gameObject.name } : { gameObject.transform.position }");
    }
}