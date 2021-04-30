using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelUI : MonoBehaviour
{

    public Image[] images;
    public Sprite LockedIcon;

    [Range(0, 1)]
    public float unselectedOpacity, selectedOpacity;
    

    void Start()
    {
        images = this.gameObject.GetComponentsInChildren<Image>();
        for(int i = 0; i < images.Length; i++){
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, unselectedOpacity);
        }

        this.gameObject.SetActive(false);
    }

    public void Appear(){
        for(int i = 0; i < images.Length; i++){
            if(i < PlatformerController.instance.weapons.Count)
                images[i].sprite = PlatformerController.instance.weapons[i].RuneSprite;
            else
                images[i].sprite = LockedIcon;
        }

        this.gameObject.SetActive(true);
    }

    public void Disappear(){
        this.gameObject.SetActive(false);
    }

    public void SelectWeapon(int index){
        //TODO :: Make the icon grow or something to indicate which weapon was picked
    }

    public void HighlightWeapon(int index){

        for(int i = 0; i < images.Length; i++){
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, unselectedOpacity);
        }

        images[index].color = new Color(images[index].color.r, images[index].color.g, images[index].color.b, selectedOpacity);
    }
}
