using UnityEngine;

public class DisableIfWebGL : MonoBehaviour
{
#if UNITY_WEBGL
    private void Start()
    {
        gameObject.SetActive(false);
    }
#endif
}