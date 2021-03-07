using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillBarUI : MonoBehaviour
{
    public Image BarFill;
    
    public float lerpDuration;

    void Start()
    {
        BarFill = gameObject.GetComponent(typeof(Image)) as Image;
    }

    /* ChangeFillAmount() is a IEnumerator called externally whenever:
     *      an ability is used || mana is gained || health is reduced etc.
     *      
     * This allows the fill amount to slightly adjust to the newMana the lerpDuration
     */

    public IEnumerator ChangeFillAmount(float fillStartPoint, float fillEndPoint, float maxFill)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            BarFill.fillAmount = Mathf.Lerp(fillStartPoint, fillEndPoint, timeElapsed / lerpDuration) / maxFill;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
