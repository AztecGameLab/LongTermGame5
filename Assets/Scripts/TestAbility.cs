using UnityEngine.InputSystem;

public class TestAbility : Ability
{
    // This is the name of the input which triggers the ability.
    // You can find the names of all the possible inputs in the GameInputs.inputactions file
    // (At the time of writing, GameInputs.inputactions is located under Scripts/BaseCode) 
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