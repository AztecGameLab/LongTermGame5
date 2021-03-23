using Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private CinemachineBrain cinemachineBrain;

    private void Start()
    {
        
    }

    private void OnGUI()
    {
        GUILayout.Label(cinemachineBrain.ActiveVirtualCamera.Name);
    }
    
    public void SetCameraCollider(Collider2D col)
    {
        var activeCamera = cinemachineBrain.ActiveVirtualCamera;
        var confinerComponent = activeCamera?.VirtualCameraGameObject.GetComponent<CinemachineConfiner>();

        if (confinerComponent != null)
        {
            confinerComponent.m_BoundingShape2D = col;
            confinerComponent.m_ConfineScreenEdges = true;
        }
    }
}