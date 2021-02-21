using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarUI : MonoBehaviour
{
    public Image ManaBarFill;
    
    public float lerpDuration;

    /* ChangeFillAmount() is a IEnumerator called externally whenever:
     *      an ability is used || mana is gained
     *      
     * This allows the fill amount to slightly adjust to the newMana the lerpDuration
     */

    public IEnumerator ChangeFillAmount(float manaStartPoint, float manaEndPoint, float maxMana)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            ManaBarFill.fillAmount = Mathf.Lerp(manaStartPoint, manaEndPoint, timeElapsed / lerpDuration) / maxMana;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
