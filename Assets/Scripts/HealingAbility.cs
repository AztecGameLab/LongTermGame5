using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealingAbility : Ability
{
    [Header("Heal Settings")]
    public float healAmount = 1;
    public float healZoomAmount = 1f;
    public float healChargeTime = 2f;
    public AnimationCurve healChargeCurve;
    public AnimationCurve healResetCurve;
    [EventRef] public string healSfx;
    
    [Header("Heal State")] 
    public float remainingHealTime = 0f;
    public float remainingHealTimeAnalog = 0f;
    
    private float MaxHealth => Player.parameters.MaxHealth;
    private EventInstance _healSfxEvent;
    private CameraController _cameraController;
    private float _zoomDefault;
    private Coroutine _healingCoroutine;
    
    protected override void Start()
    {
        base.Start();

        _cameraController = CameraController.Get();
        _healSfxEvent = RuntimeManager.CreateInstance(healSfx);
    }

    protected override void Started(InputAction.CallbackContext context)
    {
        _healingCoroutine = StartCoroutine(ChargeHeal());
    }

    protected override void Canceled(InputAction.CallbackContext context)
    {
        StopCoroutine(_healingCoroutine);
        StartCoroutine(Cleanup());
    }

    private IEnumerator ChargeHeal()
    {
        var shouldZoom = _cameraController.TryGetZoom(out _zoomDefault);
        var zoomStart = _zoomDefault - healZoomAmount;

        _healSfxEvent.start();
        remainingHealTime = healChargeTime;
        Player.lockControls = true;

        while (remainingHealTime > 0)
        {
            remainingHealTime -= Time.deltaTime;
            remainingHealTimeAnalog = 1 - remainingHealTime / healChargeTime;
            
            if (shouldZoom)
                _cameraController.SetZoom(Mathf.Lerp(_zoomDefault, zoomStart, healChargeCurve.Evaluate(remainingHealTimeAnalog)));
            
            yield return new WaitForEndOfFrame();
        }
        Player.health = Mathf.Min(Player.health + healAmount, MaxHealth);
        StartCoroutine(Cleanup());
    }

    private IEnumerator Cleanup()
    {
        var startTime = Time.time;
        _cameraController.TryGetZoom(out var currentZoom);
        Player.lockControls = false;
        remainingHealTime = 0;
        remainingHealTimeAnalog = 0;
        _healSfxEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
        while (Time.time - startTime < 1)
        {
            var t = Time.time - startTime; 
            _cameraController.SetZoom(Mathf.Lerp(currentZoom, _zoomDefault, healResetCurve.Evaluate(t)));
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnGUI()
    {
        GUILayout.Label($"Player HP: {Player.health}");
    }
    
    protected override string InputName => "ManaHeal";
}