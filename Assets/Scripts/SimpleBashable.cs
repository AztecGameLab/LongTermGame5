using FMODUnity;
using UnityEngine;

public class SimpleBashable : MonoBehaviour, IBashable
{
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private SpriteRenderer sprite;
    
    [EventRef] public string bashSound = "Default";
    public float shakeIntensity = 1f;
    public float speedBoost = 10f;
    public float bashRange = 1;
    
    private Vector3 BashOrigin => sprite.bounds.center;
    
    public bool CanBash(PlatformerController controller)
    {
        var playerDistance = Vector2.Distance(controller.transform.position, BashOrigin);
        return playerDistance <= bashRange;
    }

    public void Bash(PlatformerController controller, float distance)
    {
        if (bashSound != "Default")
            RuntimeManager.PlayOneShot(bashSound);
        
        var bashDirection = controller.primaryStick.normalized;
        rigidbody2d.velocity = -bashDirection * speedBoost;

        controller.isJumping = false;
        controller.rigid.velocity = Vector2.zero;
        controller.transform.position = BashOrigin + (Vector3) bashDirection * 0.1f;
        controller.KnockBack(bashDirection, distance);
        controller.playerImpulseSource.GenerateImpulse(shakeIntensity);
    }
    
#if UNITY_EDITOR
    private readonly Color _bashRangeColor = new Color(0, 1, 1, 0.1f);
    
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = _bashRangeColor;
        UnityEditor.Handles.DrawSolidDisc(BashOrigin, Vector3.forward, bashRange);
    }
#endif
}