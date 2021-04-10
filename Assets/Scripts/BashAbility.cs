using UnityEngine;
using UnityEngine.InputSystem;

public class BashAbility : Ability
{
    public float bashDistance;
    
    protected override void Started(InputAction.CallbackContext context)
    {
        var nearestBashable = Scanner.GetClosestObject<IBashable>(Player.transform.position);

        if (CanBash(nearestBashable))
            nearestBashable.Bash(Player, bashDistance);
    }

    private bool CanBash(IBashable bashable)
    {
        return bashable != null  
               && Player.primaryStick.normalized != Vector2.zero  
               && bashable.CanBash(Player);
    }
    
    protected override string InputName => "Bash";
}