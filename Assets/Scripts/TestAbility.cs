using UnityEngine.InputSystem;

public class TestAbility : Ability
{
    protected override string InputName => "Move";
    private int _counter = 0;
    
    protected override void Started(InputAction.CallbackContext context)
    {
        // Add logic for executing the ability.
        print($"Ability Activated { ++_counter } times!");
        
        // If a reference to the Player is needed, access the protected Player field as shown below:
        Player.lockControls = false;
    }
}