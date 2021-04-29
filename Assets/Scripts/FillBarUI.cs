using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FillBarUI : MonoBehaviour
{
    [SerializeField] private Image barFill;
    [SerializeField] private float lerpDuration;

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
            barFill.fillAmount = Mathf.Lerp(fillStartPoint, fillEndPoint, timeElapsed / lerpDuration) / maxFill;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
