using Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private CinemachineBrain brain;

    private CinemachineVirtualCamera _activeVirtualCam;

    private void OnEnable()
    {
        brain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
    }

    private void OnDisable()
    {
        brain.m_CameraActivatedEvent.RemoveListener(OnCameraActivated);
    }

    private void OnCameraActivated(ICinemachineCamera newCam, ICinemachineCamera oldCam)
    {
        if (newCam is CinemachineVirtualCamera virtualCam)
            _activeVirtualCam = virtualCam;
    }

    public bool TryGetZoom(out float result)
    {
        result = _activeVirtualCam == null ? -1 : _activeVirtualCam.m_Lens.OrthographicSize;
        return _activeVirtualCam != null;
    }
    
    public void SetZoom(float zoom)
    {
        if (_activeVirtualCam == null)
            return;

        _activeVirtualCam.m_Lens.OrthographicSize = zoom;
    }
}