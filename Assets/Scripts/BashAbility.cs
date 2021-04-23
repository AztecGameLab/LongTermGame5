using UnityEngine;
using UnityEngine.InputSystem;

public class BashAbility : Ability
{
    protected override string InputName => "Bash";
    public float bashDistance = 20;
    
    protected override void Started(InputAction.CallbackContext context)
    {
        var nearestBashable = Scanner.GetClosestObject<IBashable>(Player.transform.position);

        if (CanBash(nearestBashable))
        {
            GetComponent<Animator>().Play("bash");
            nearestBashable.Bash(Player, bashDistance);
        }
    }

    private bool CanBash(IBashable bashable)
    {
        return bashable != null  
               && Player.primaryStick.normalized != Vector2.zero  
               && bashable.CanBash(Player);
    }
}