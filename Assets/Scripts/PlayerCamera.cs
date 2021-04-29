using Cinemachine;
using EasyButtons;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")] 
    [SerializeField] private float orthographicSize = 6f;
    [SerializeField] private Trigger activeZone;

    [Header("Horizontal Settings")]
    [SerializeField] private float horizontalDistance = 1.75f;
    [SerializeField] private float horizontalTime = 1f;

    [Header("Vertical Settings")]
    [SerializeField] private float verticalDistance = 3f;
    [SerializeField] private float verticalTime = 0.1f;
    [SerializeField] private float verticalHoldTime = 0.5f;
    
    private CinemachineFramingTransposer _transposer;
    private CinemachineVirtualCamera _camera;
    private PlatformerController _player;
    
    private Vector2 Input => _player.primaryStick;
    private Vector2 _dampVelocity = Vector2.zero;
    private Vector3 _offset = Vector3.zero;
    private float VerticalDirection => Input.y > 0 ? 1 : -1;
    private float HorizontalDirection => Input.x > 0 ? 1 : -1; 
    private float _holdTime = 0;
    private bool _hasTransposer = false;
    
    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _transposer = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _hasTransposer = _transposer != null;

        activeZone.events.collisionEnter.AddListener(TriggerEnter);
        activeZone.events.collisionExit.AddListener(TriggerExit);
        
        _camera.m_Lens.OrthographicSize = orthographicSize;
        _camera.enabled = false;
    }

    private void Start()
    {
        _player = PlatformerController.instance;
        _camera.Follow = _player.transform;
    }
    
    private void Update()
    {
        if (_hasTransposer)
        {
            LookHorizontal();
            LookVertical();

            _transposer.m_TrackedObjectOffset = _offset;    
        }
    }

    private void LookHorizontal()
    {
        if (Input.x == 0)
            return;
        
        var current = _transposer.m_TrackedObjectOffset.x;
        var target = HorizontalDirection * horizontalDistance;
        
        _offset.x = Mathf.SmoothDamp(current, target, ref _dampVelocity.x, horizontalTime);
    }
    
    private void LookVertical()
    {
        CalculateHoldTime();

        float current = _transposer.m_TrackedObjectOffset.y;
        float target = 0f;
        
        if (_player.isGrounded && _holdTime > verticalHoldTime)
            target = VerticalDirection * verticalDistance;
        
        _offset.y = Mathf.SmoothDamp(current, target, ref _dampVelocity.y, verticalTime);
    }

    private void CalculateHoldTime()
    {
        if (Input.y == 0)
        {
            _holdTime = 0;
        } 
        else _holdTime += Time.deltaTime;
    }
    
    private void TriggerEnter(GameObject obj)
    {
        _camera.enabled = true;
    }

    private void TriggerExit(GameObject obj)
    {
        _camera.enabled = false;
    }
}