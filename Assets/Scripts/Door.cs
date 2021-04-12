using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Entity
{
    [Header("Destructible From: ")]
    [SerializeField] bool left = false;
    [SerializeField] bool right = false;
    [SerializeField] bool top = false;
    [SerializeField] bool bottom = false;

    [EventRef] public string breakSound = "Default";
    [SerializeField] GameObject leftSprite;
    [SerializeField] GameObject rightSprite;
    [SerializeField] GameObject topSprite;
    [SerializeField] GameObject bottomSprite;

    private void Start()
    {
        // in case of manuel enabled sprites
        if (leftSprite.activeSelf)
        {
            left = true;
        }
        if (rightSprite.activeSelf)
        {
            right = true;
        }
        if (topSprite.activeSelf)
        {
            top = true;
        }
        if (bottomSprite.activeSelf)
        {
            bottom = true;
        }


        //enable sprites based on bool from the editor
        if (left)
        {
            leftSprite.SetActive(true);
        }
        if (right)
        {
            rightSprite.SetActive(true);
        }
        if (top)
        {
            topSprite.SetActive(true);
        }
        if (bottom)
        {
            bottomSprite.SetActive(true);
        }
    }
    [EasyButtons.Button]
    public override void TakeDamage(float baseDamage, Vector2 direction)
    {
        if ((left && direction.x > 0)|| (right && direction.x < 0) || (bottom && direction.y < 0) || (top && direction.y > 0))
        {
            if(breakSound != "Default")
            {
                RuntimeManager.PlayOneShot(breakSound);
            }
            Destroy(gameObject);
        }

    }

    public override void TakeDamage(float baseDamage)
    {
        //do nothing with undirectional damge
    }


}
