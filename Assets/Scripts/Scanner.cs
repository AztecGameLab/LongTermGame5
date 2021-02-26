using System.Collections.Generic;
using UnityEngine;

public static class Scanner
{
    private static readonly Collider2D[] NearbyColliderBuffer = new Collider2D[100];
    
    public static T GetClosestObject<T>(Vector2 origin, float range = 10) where T : MonoBehaviour
    {
        int resultSize = PopulateColliderBuffer(range, origin);
        
        Collider2D closest = FindFirstColliderWithComponent<T>(NearbyColliderBuffer, resultSize);
        float closestDistance = (origin - (Vector2) closest.bounds.center).sqrMagnitude;
        
        for (int i = 0; i < resultSize; i++)
        {
            Collider2D element = NearbyColliderBuffer[i];
            float elementDistance = (origin - (Vector2) element.bounds.center).sqrMagnitude;
            
            if (elementDistance < closestDistance)
            {
                if (element.TryGetComponent<T>(out _))
                {
                    closest = element;
                    closestDistance = elementDistance;
                }
            }
        }
        return closest.GetComponent<T>();
    }
    
    // Returns the amount of colliders that were populated into the buffer
    private static int PopulateColliderBuffer(float range, Vector2 origin)
    {
        var resultSize = Physics2D.OverlapCircleNonAlloc(origin, range, NearbyColliderBuffer);
        
        if (resultSize > NearbyColliderBuffer.Length)
            Debug.LogWarning($"Scanner buffer [{ NearbyColliderBuffer.Length }] was smaller than raycast results [{ resultSize }] ");

        return resultSize;
    }
    
    private static Collider2D FindFirstColliderWithComponent<T>
        (Collider2D[] arrayToSearch, int elementsToSearch) where T : MonoBehaviour
    {
        for (int i = 0; i < elementsToSearch; i++)
        {
            if (arrayToSearch[i].TryGetComponent<T>(out _))
                return arrayToSearch[i];
        }

        return null;
    }
    
    public static T[] GetObjectsInRange<T>(Vector2 origin, float range = 10) where T : MonoBehaviour
    {
        var resultSize = PopulateColliderBuffer(range, origin);
        var result = new List<T>();

        for (int i = 0; i < resultSize; i++)
        {
            if (NearbyColliderBuffer[i].TryGetComponent<T>(out var component))
                result.Add(component);
        }
        
        return result.ToArray();
    }
}