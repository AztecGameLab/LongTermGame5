using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float lookAheadDistance = 3f;
    [SerializeField] private float lookAheadTime = 5f;
    
    [SerializeField] private float lookUpDistance = 3f;
    [SerializeField] private float lookUpTime = 0.1f;
    
    [SerializeField] private float timeHeldBeforeLookVertical = 1f;
    
    private CinemachineFramingTransposer _transposer;
    private CinemachineVirtualCamera _camera;
    private PlatformerController _player;
    private Vector2 _dampVelocity = Vector2.zero;
    private Vector2 Input => _player.primaryStick;
    private Vector3 _offset = Vector3.zero;
    private float _lookTime = 0;
    
    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _transposer = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _player = PlatformerController.instance;
    }

    private void Update()
    {
        if (Input.x != 0)
            LookHorizontal();

        if (Input.y != 0)
            LookVertical();
        else
        {
            _lookTime = 0;
            
            var current = _transposer.m_TrackedObjectOffset.y;
            _offset.y = Mathf.SmoothDamp(current, 0, ref _dampVelocity.y, lookUpTime);
        }

        _transposer.m_TrackedObjectOffset = _offset;
    }

    private void LookHorizontal()
    {
        var current = _transposer.m_TrackedObjectOffset.x;
        var target = (Input.x > 0 ? 1 : -1) * lookAheadDistance;
        _offset.x = Mathf.SmoothDamp(current, target, ref _dampVelocity.x, lookAheadTime);
    }

    
    private void LookVertical()
    {
        _lookTime += Time.deltaTime;

        if (_lookTime > timeHeldBeforeLookVertical)
        {
            var current = _transposer.m_TrackedObjectOffset.y;
            var target = (Input.y > 0 ? 1 : -1) * lookUpDistance;
            _offset.y = Mathf.SmoothDamp(current, target, ref _dampVelocity.y, lookUpTime);
        }
    }
}