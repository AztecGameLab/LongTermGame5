using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealingAbility : Ability
{
    [Header("Heal Settings")]
    public float healAmount = 1;
    public float healChargeTime = 2f;

    [Header("Heal State")] 
    public float remainingHealTime = 0f;
    public float remainingHealTimeAnalog = 0f;
    
    private float MaxHealth => Player.parameters.MaxHealth;

    protected override void Started(InputAction.CallbackContext context)
    {
        StartCoroutine(ChargeHeal());
    }

    protected override void Canceled(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
    }

    private IEnumerator ChargeHeal()
    {
        remainingHealTime = healChargeTime;
        Player.lockControls = true;

        while (remainingHealTime > 0)
        {
            remainingHealTime -= Time.deltaTime;
            remainingHealTimeAnalog = 1 - remainingHealTime / healChargeTime;
            yield return new WaitForEndOfFrame();
        }
        Player.lockControls = false;
        remainingHealTime = 0;
        remainingHealTimeAnalog = 0;
        Player.health = Mathf.Min(Player.health + healAmount, MaxHealth);
    }

    private void OnGUI()
    {
        GUILayout.Label($"Player HP: {Player.health}");
    }
    
    protected override string InputName => "ManaHeal";
}