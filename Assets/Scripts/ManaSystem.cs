using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem : MonoBehaviour
{
    public float maxMana;
    public float currentMana;

    public ManaBarUI ManaBarUI;

    /* Start is called before the first frame update
     * 
     */
    void Start()
    {
        //When the game starts set the currentMana to the max it can be
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
     * If manaGained would put the player over maxMana:
     *      Set currentMana to maxMana
     * Otherwise:
     *      Set currentMana to the value of currentMana plus manaGained
     *      
     * Update the UI FillAmount to be (new currentMana)/maxMana
     */
    public void GainMana(float manaGained)
    {
        if (currentMana + manaGained >= maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += manaGained;
        }

        ManaBarUI.ChangeFillAmount(currentMana, maxMana);
    }


    /* This method is executed when an ability is attempted to be used.
     * 
     * If the player has enough mana to use the ability:
     *      The mana is consumed && 
     *      The UI updates to the based on the lower mana level &&
     *      True is returned indicating the ability can be used
     * Otherwise:
     *      False is returned indicating the player needs more mana. 
     */
    public bool ConsumeMana(float manaCost)
    {

        if (currentMana >= manaCost)
        {
            currentMana -= manaCost;
            ManaBarUI.ChangeFillAmount(currentMana, maxMana);
            return true;
        }
        else
        {
            return false;
        }
    }


    /* This method increases the maxMana by the manaIncrease parameter
     * Following this, the GainMana(manaIncrease) is called to also update currentMana
     * Update the UI FillAmount to be (new currentMana)/(new maxMana)
     *
     */
    public void IncreaseMaxMana(float manaIncrease)
    {
        maxMana += manaIncrease;
        GainMana(manaIncrease);
        ManaBarUI.ChangeFillAmount(currentMana, maxMana);
    }   
}
