using UnityEngine;
using UnityEngine.InputSystem;

// TODO: Implement actual healing, change input from "A", change debug button ui to new rect (easier debugging)
public class HealingAbility : Ability
{
    protected override string InputName => "ManaHeal";

    protected override void Started(InputAction.CallbackContext context)
    {
        print("Heal player!");
    }

    private void OnGUI()
    {
        Rect position = new Rect(Screen.width - 200, 50, 200, 100);
        GUI.Label(position, $"Player HP: {Player.health}");
    }
}