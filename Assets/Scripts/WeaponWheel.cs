using System.Collections;
using UnityEngine;

public class WeaponWheel : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float showTimeSeconds = 0.25f;
    [SerializeField] private float hideTimeSeconds = 0.25f;

    [Header("References")]
    [SerializeField] private GameObject wheelParent;
    [SerializeField] private GameObject fireballImage;
    [SerializeField] private GameObject grappleImage;
    [SerializeField] private GameObject ricochetImage;
    [SerializeField] private GameObject freezeImage;

    private Vector3 CurrentScale => wheelParent.transform.localScale;

    private void OnEnable()  { AbilityUnlocks.AbilityUnlocked += UnlockWeapon; }
    private void OnDisable() { AbilityUnlocks.AbilityUnlocked -= UnlockWeapon; }

    private void UnlockWeapon(AbilityUnlocks.Abilities ability)
    {
        switch (ability)
        {
            case AbilityUnlocks.Abilities.ReflectingProjectile:
                ricochetImage.SetActive(true);
                break;
            case AbilityUnlocks.Abilities.FireBall:
                fireballImage.SetActive(true);
                break;
            case AbilityUnlocks.Abilities.FreezeProjectile:
                freezeImage.SetActive(true);
                break;
            case AbilityUnlocks.Abilities.Grapple:
                grappleImage.SetActive(true);
                break;
        }
    }

    public IEnumerator Show()
    {
        while (CurrentScale != Vector3.one)
        {
            float delta = showTimeSeconds * Time.deltaTime;
            wheelParent.transform.localScale = 
                Vector3.MoveTowards(CurrentScale, Vector3.one, delta);
            yield return null;
        }
    }
    
    public IEnumerator Hide()
    {
        while (CurrentScale != Vector3.zero)
        {
            float delta = hideTimeSeconds * Time.deltaTime;
            wheelParent.transform.localScale = Vector3.MoveTowards(CurrentScale, Vector3.zero, delta);
            yield return null;
        }
    }
}