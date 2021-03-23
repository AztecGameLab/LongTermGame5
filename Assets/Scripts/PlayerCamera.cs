using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerCamera : MonoBehaviour
{
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
    private AudioListener _audioListener;
    
    private Vector2 Input => _player.primaryStick;
    private Vector2 _dampVelocity = Vector2.zero;
    private Vector3 _offset = Vector3.zero;
    private float VerticalDirection => Input.y > 0 ? 1 : -1;
    private float HorizontalDirection => Input.x > 0 ? 1 : -1; 
    private float _holdTime = 0;
    
    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _transposer = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _audioListener = GetComponent<AudioListener>();
    }

    private void Start()
    {
        _player = PlatformerController.instance;
        _camera.Follow = _player.transform;
    }

    public void SetActive(bool active)
    {
        _camera.Priority = active ? 1 : 10;
        _audioListener.gameObject.SetActive(active);
    }

    private void Update()
    {
        LookHorizontal();
        LookVertical();

        _transposer.m_TrackedObjectOffset = _offset;
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
}