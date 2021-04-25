using System.Collections;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealingAbility : Ability
{
    [Header("Heal Settings")] 
    [SerializeField] private float manaCost = 0.25f;
    [SerializeField] private float healAmount = 1;
    [SerializeField] private float healZoomAmount = 1f;
    [SerializeField] private float healChargeTime = 2f;
    [SerializeField] private AnimationCurve healChargeCurve;
    [SerializeField] private AnimationCurve healResetCurve;
    [SerializeField, EventRef] private string healSfx;
    
    [Header("Heal State")] 
    [SerializeField] private float remainingHealTime = 0f;
    [SerializeField] private float remainingHealTimeAnalog = 0f;
    
    private float MaxHealth => Player.parameters.MaxHealth;
    private EventInstance _healSfxEvent;
    private CameraController _cameraController;
    private float _zoomDefault;
    private Coroutine _healingCoroutine;
    private bool _canHeal = true;
    private ManaController _manaController;
    
    protected override void Start()
    {
        _manaController = ManaController.Get();
        _cameraController = CameraController.Get();
        _healSfxEvent = RuntimeManager.CreateInstance(healSfx);
        
        base.Start();
    }

    protected override void Started(InputAction.CallbackContext context)
    {
        if (_canHeal && _manaController.GetCurrentFill() >= manaCost)
            _healingCoroutine = StartCoroutine(ChargeHeal());
    }

    protected override void Canceled(InputAction.CallbackContext context)
    {
        if (_healingCoroutine != null)
        {
            StopCoroutine(_healingCoroutine);
            StartCoroutine(Cleanup());

            _healingCoroutine = null;
        }
    }

    private IEnumerator ChargeHeal()
    {
        GetComponent<Animator>().SetBool("healing", true);
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
        _manaController.Consume(manaCost, false);
        StartCoroutine(Cleanup());
    }

    private IEnumerator Cleanup()
    {
        GetComponent<Animator>().SetBool("healing", false);
        var startTime = Time.time;
        _cameraController.TryGetZoom(out var currentZoom);
        Player.lockControls = false;
        remainingHealTime = 0;
        remainingHealTimeAnalog = 0;
        _healSfxEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _canHeal = false;
        
        while (Time.time - startTime < 1)
        {
            var t = Time.time - startTime; 
            _cameraController.SetZoom(Mathf.Lerp(currentZoom, _zoomDefault, healResetCurve.Evaluate(t)));
            yield return new WaitForEndOfFrame();
        }

        _canHeal = true;
    }

    protected override string InputName => "ManaHeal";
}