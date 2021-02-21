using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarUI : MonoBehaviour
{
    public Image ManaBarFill;


    /* ChangeFillAmount is a script called externally whenever:
     *      an ability is used 
     *      or mana is gained 
     * 
     */
    public void ChangeFillAmount(float mana, float maxMana)
    {
        ManaBarFill.fillAmount = mana / maxMana;
    }
}
