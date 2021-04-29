using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelUI : MonoBehaviour
{

    public Image[] images;

    [Range(0, 1)]
    public float unselectedOpacity, selectedOpacity;
    

    void Start()
    {
        images = this.gameObject.GetComponentsInChildren<Image>();
        for(int i = 0; i < images.Length; i++){
            images[i].gameObject.SetActive(false);
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, unselectedOpacity);
        }
    }

    public void Appear(){
        for(int i = 0; i < PlatformerController.instance.weapons.Count; i++){
            images[i].sprite = PlatformerController.instance.weapons[i].RuneSprite;
            images[i].gameObject.SetActive(true);
        }

        this.gameObject.SetActive(true);
    }

    public void Disappear(){
        this.gameObject.SetActive(false);
    }

    public void SelectWeapon(int index){
        //TODO :: Make the icon grow or something to indicate which weapon was picked
    }

    int lastHighlighted = 0;

    public void HighlightWeapon(int index){
        if(index != lastHighlighted)
            images[lastHighlighted].color = new Color(images[lastHighlighted].color.r, images[lastHighlighted].color.g, images[lastHighlighted].color.b, unselectedOpacity);
        images[index].color = new Color(images[index].color.r, images[index].color.g, images[index].color.b, selectedOpacity);
    }
}
