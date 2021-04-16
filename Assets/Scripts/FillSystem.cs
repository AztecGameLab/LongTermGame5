using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillSystem : MonoBehaviour
{
    public float maxFill;
    public float currentFill;

    public FillBarUI FillBarUI;

    /* When the game starts set the currentMana to the max it can be
     * 
     */
    public virtual void Start()
    {
        currentFill = maxFill;
    }

    /* A method which returns the currentFill
     * 
     */
    public float GetCurrentFill()
    {
        return currentFill;
    }


    /* This method allows the player to gain mana when called elsewhere
     * 
     * If valueGained is negative:
     *      The programer is told via Debug.LogError that they likely intended to use Consume()
     * 
     * Otherwise:
     *      Set a temporary variable for the currentFill that is used in the ChangeFillAmount() Lerp function
     *      Set currentFill to the value of currentFill plus valueGained
     *      Clamp this value to the maxFill so that the player never has more than maxFill
     *      ChangeFillAmount() based on the new fill level
     *      
     */
    public void Gain(float valueGained)
    {
        if (valueGained < 0)
        {
            Debug.LogError("The Gain() function expects a positive \"valueGained\" not a negative one. \n " +
                           "Did you intend to use the Consume() function?");
        }
        else
        {
            float fillBefore = currentFill;
            currentFill += valueGained;
            currentFill = Mathf.Clamp(currentFill, 0, maxFill);
            StartCoroutine(FillBarUI.ChangeFillAmount(fillBefore, currentFill, maxFill));
        }
    }

    //These two scripts allow the buttons to work
    public void CanConsumePastZero(float cost)
    {
       Consume(cost, true);
    }

    public void CanNotConsumePastZero(float cost)
    {
        Consume(cost, false);
    }

    /* This method is executed whenever a fillbar is attempted to be reduced
     * 
     * First the fillBefore is grabbed as it is used many places within this function
     * 
     * If the programer gave a negative cost:
     *      The programer is told via Debug.LogError that they likely intended to use Gain() &&
     *      False is returned indicating the function had a negative "cost." 
     * 
     * Else if the player has enough fill to consume:
     *      The cost is consumed && 
     *      The UI updates based on the lower level &&
     *      True is returned indicating the value was successfully reduced
     * 
     * Else if the first check failed but the fill can be reduced below Zero
     *      The fill is set to 0 && 
     *      The UI updates based on the lower level &&
     *      True is returned indicating the value was successfully reduced
     *      
     * Otherwise the cost was too high and false is returned as the fill couldn't be reduced past zero
     *      
     */
    public bool Consume(float cost, bool canBeReducedPastZero)
    {
        float fillBefore = currentFill;
        
        if (cost < 0)
        {
            Debug.LogError("The Consume() function expects a positive \"cost\" not a negative one");
            Debug.LogError("Did you intend to use the Gain() function?");
            return false;
        }
        else if (currentFill >= cost)
        {
            currentFill -= cost;
            StartCoroutine(FillBarUI.ChangeFillAmount(fillBefore, currentFill, maxFill));
            return true;
        }
        else if (canBeReducedPastZero)
        {
            currentFill = 0;
            StartCoroutine(FillBarUI.ChangeFillAmount(fillBefore, currentFill, maxFill));
            return true;
        }
        else
        {
            //Not enough fill to Consume because cost is too high
            return false;
        }

    }


    /* This method increases the maxValue of a fill bar by the valueIncrease parameter
     * 
     * If the valueIncrease is negative:
     *      The programer is told IncreaseMax expects positive valueIncrease
     *      They are also told a negative valueIncrease has not been implemented
     *      
     * Otherwise valueIncrease is positive:
     *      maxFill is increased by valueIncrease &&
     *      Gain(valueIncrease) is called to also update currentFill
     *
     */
    public void IncreaseMax(float valueIncrease)
    {
        if (valueIncrease < 0)
        {
            Debug.LogError("The IncreaseMax() function expects a positive valueIncrease not a negative one");
            Debug.LogError("A negative valueIncrease has not been implemented.");
        }
        else
        {
            maxFill += valueIncrease;
            Gain(valueIncrease);
        }
    }
}
