using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    public float maxMana;
    public float currentMana;

    public ManaBarUI ManaBarUI;

    /* When the game starts set the currentMana to the max it can be
     * 
     */
    void Start()
    {
        currentMana = maxMana;
    }

    /* A method which returns the currentMana
     * 
     */
    public float GetCurrentMana()
    {
        return currentMana;
    }


    /* This method allows the player to gain mana when called elsewhere
     * 
     * If manaGained is positive:
     *      Set a temporary variable for the currentMana that is used in the ChangeFillAmount() Lerp function
     *      Set currentMana to the value of currentMana plus manaGained
     *      Clamp this value to the maxMana so that the player never has more than max mana
     *      ChangeFillAmount() based on the new mana level
     *      
     * Otherwise manaGained must be negative:
     *      As such, the programer is told via Debug.LogError that they likely intended to use ConsumeMana()
     *      
     */
    public void GainMana(float manaGain)
    {
        if (manaGain > 0)
        {
            float manaBefore = currentMana;
            currentMana += manaGain;
            Mathf.Clamp(currentMana, 0, maxMana);
            StartCoroutine(ManaBarUI.ChangeFillAmount(manaBefore,currentMana, maxMana));
        }
        else
        {
            Debug.LogError("The GainMana() function expects a positive manaGain not a negative one");
            Debug.LogError("Did you intend to use the ConsumeMana() function?");
        }
    }


    /* This method is executed when an ability is attempted to be used.
     * 
     * If the programer gave a negative manaCost:
     *      The programer is told via Debug.LogError that they likely intended to use GainMana() &&
     *      
     * Else if the player has enough mana to use the ability:
     *      Set a temporary variable for the currentMana that is used in the ChangeFillAmount() Lerp function &&
     *      The mana is consumed && 
     *      The UI updates based on the lower mana level &&
     *      True is returned indicating the ability can be used
     *      
     * False is returned indicating the player needs more mana or the function had a negative manaCost. 
     *      
     */
    public bool ConsumeMana(float manaCost)
    {

        if (manaCost < 0)
        {
            Debug.LogError("The ConsumeMana() function expects a positive manaCost not a negative one");
            Debug.LogError("Did you intend to use the GainMana() function?");
        }
        else if(currentMana >= manaCost)
        {
            float manaBefore = currentMana;
            currentMana -= manaCost;
            StartCoroutine(ManaBarUI.ChangeFillAmount(manaBefore, currentMana, maxMana));
            return true;
        }
        return false;
        
    }


    /* This method increases the maxMana by the manaIncrease parameter
     * 
     * If the manaIncrease is negative:
     *      The programer is told IncreaseMaxMana expects positive manaIncrease
     *      They are also told a negative manaIncrease has not been implemented
     *      
     * Otherwise manaIncrease is positive:
     *      maxMana is increase by manaIncrease &&
     *      GainMana(manaIncrease) is called to also update currentMana
     *
     */
    public void IncreaseMaxMana(float manaIncrease)
    {
        if (manaIncrease < 0)
        {
            Debug.LogError("The IncreaseMaxMana() function expects a positive manaIncrease not a negative one");
            Debug.LogError("A negative manaIncrease has not been implemented.");
        }
        else
        {
            maxMana += manaIncrease;
            GainMana(manaIncrease);
        }
    }
}
