using UnityEngine;

public class RotateTest : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 1f;
    [SerializeField] private float targetDegrees = 90f;
    [SerializeField] private Transform rotationOrigin;
    
    private void Update()
    {
        var axis = Vector3.forward;
        var origin = rotationOrigin.position;
        var degreeDelta = degreesPerSecond * Time.deltaTime;

        var curDegrees = transform.rotation.eulerAngles.z;
        var targetA = targetDegrees;
        var targetB = curDegrees > targetA ? targetA + 360: targetA - 360;
        var targetADistance = Mathf.Abs(curDegrees - targetA);
        var targetBDistance = Mathf.Abs(curDegrees - targetB);
        var closestTarget = targetADistance < targetBDistance ? targetA : targetB;
        
        var direction = Mathf.Sign(closestTarget - curDegrees);
        
        if (Mathf.Abs(closestTarget - curDegrees) < degreeDelta)
        {
            transform.RotateAround(origin, axis, closestTarget - curDegrees);        
        }
        else
        {
            transform.RotateAround(origin, axis, direction * degreeDelta);
        }
    }
}