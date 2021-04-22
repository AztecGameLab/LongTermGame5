using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneUIManager : MonoBehaviour
{
    public static RuneUIManager _instance;
    [SerializeField]
    Material runeOffMaterial;
    [SerializeField]
    Material runeOnMaterial;
    
    [SerializeField]
    GameObject Rune1, Rune2, Rune3, Rune4, Rune5, Rune6, Rune7, Rune8, Rune9;
    

    public static RuneUIManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<RuneUIManager>();
            }
            return _instance;
        }
    }

    public void turnOffRune1()
    {

        Rune1.GetComponent<Image>().material = runeOffMaterial;
        
    }

    public void turnOnRune1()
    {
        Rune1.GetComponent<Image>().material = runeOnMaterial;
    }

    public void turnOffRune2()
    {

        Rune2.GetComponent<Image>().material = runeOffMaterial;

    }

    public void turnOnRune2()
    {
        Rune2.GetComponent<Image>().material = runeOnMaterial;
    }

    public void turnOffRune3()
    {

        Rune3.GetComponent<Image>().material = runeOffMaterial;

    }

    public void turnOnRune3()
    {
        Rune3.GetComponent<Image>().material = runeOnMaterial;
    }
    public void turnOffRune4()
    {

        Rune4.GetComponent<Image>().material = runeOffMaterial;

    }

    public void turnOnRune4()
    {
        Rune4.GetComponent<Image>().material = runeOnMaterial;
    }

    public void turnOffRune5()
    {

        Rune5.GetComponent<Image>().material = runeOffMaterial;

    }

    public void turnOnRune5()
    {
        Rune5.GetComponent<Image>().material = runeOnMaterial;
    }

    public void turnOffRune6()
    {

        Rune6.GetComponent<Image>().material = runeOffMaterial;

    }

    public void turnOnRune6()
    {
        Rune6.GetComponent<Image>().material = runeOnMaterial;
    }

    public void turnOffRune7()
    {

        Rune7.GetComponent<Image>().material = runeOffMaterial;

    }

    public void turnOnRune7()
    {
        Rune7.GetComponent<Image>().material = runeOnMaterial;
    }

    public void turnOffRune8()
    {

        Rune8.GetComponent<Image>().material = runeOffMaterial;

    }

    public void turnOnRune8()
    {
        Rune8.GetComponent<Image>().material = runeOnMaterial;
    }

    public void turnOffRune9()
    {

        Rune9.GetComponent<Image>().material = runeOffMaterial;

    }

    public void turnOnRune9()
    {
        Rune9.GetComponent<Image>().material = runeOnMaterial;
    }
}
