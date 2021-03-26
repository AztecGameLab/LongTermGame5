using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleBashable : MonoBehaviour, IBashable
{
    public float shakeTime = 0.2f;
    public float speedBoost = 10f;
    public float bashRange = 1;
    
    private Rigidbody2D _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Bash(PlatformerController controller)
    {
        print($"Bash! { gameObject.name } : { gameObject.transform.position }");

        var bashOrigin = transform.position;
        var bashDirection = controller.primaryStick.normalized;

        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = -bashDirection * speedBoost;

        controller.isJumping = false;
        controller.transform.position = bashOrigin;
        controller.rigid.velocity = Vector2.zero;
        controller.KnockBack(bashDirection, speedBoost);
        
        // CameraShake.instance.StartShake(shakeTime);
        // Thread.Sleep(20);
    }

    public bool CanBash(PlatformerController controller)
    {
        return Vector2.Distance(controller.transform.position, transform.position) > bashRange;
    }

#if UNITY_EDITOR
    private Color bashRangeColor = new Color(0, 1, 1, 0.1f);
    
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = bashRangeColor;
        UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, bashRange);
    }
#endif
}