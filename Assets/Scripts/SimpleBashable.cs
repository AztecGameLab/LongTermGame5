using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleBashable : MonoBehaviour, IBashable
{
    [EventRef] public string bashSound = "Default";
    public float shakeIntensity = 1f;
    public float speedBoost = 10f;
    public float bashRange = 1;
    
    private Rigidbody2D _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public bool CanBash(PlatformerController controller)
    {
        var playerDistance = Vector2.Distance(controller.transform.position, transform.position);
        return playerDistance <= bashRange;
    }

    public void Bash(PlatformerController controller, float distance)
    {
        if (bashSound != "Default")
            RuntimeManager.PlayOneShot(bashSound);
        
        var bashDirection = controller.primaryStick.normalized;
        var bashOrigin = transform.position;
        
        _rigidbody.velocity = -bashDirection * speedBoost;

        controller.isJumping = false;
        controller.rigid.velocity = Vector2.zero;
        controller.transform.position = bashOrigin;
        controller.KnockBack(bashDirection, distance);
        controller.playerImpulseSource.GenerateImpulse(shakeIntensity);
    }
    
#if UNITY_EDITOR
    private readonly Color _bashRangeColor = new Color(0, 1, 1, 0.1f);
    
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = _bashRangeColor;
        UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, bashRange);
    }
#endif
}