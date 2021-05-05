using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem;

public class Door : Entity, ISaveableComponent
{
    [Header("Destructible From: ")]
    [SerializeField] bool left = false;
    [SerializeField] bool right = false;
    [SerializeField] bool top = false;
    [SerializeField] bool bottom = false;

    [EventRef] public string breakSound = "Default";
    [EventRef] public string dudSound = "Default";
    [SerializeField] GameObject leftSprite;
    [SerializeField] GameObject rightSprite;
    [SerializeField] GameObject topSprite;
    [SerializeField] GameObject bottomSprite;

    private bool collapsed;

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
                RuntimeManager.PlayOneShot(breakSound, transform.position);
            }
            Collapse();
        }
        else
        {
            RuntimeManager.PlayOneShot(dudSound, transform.position);
        }
        
    }

    public override void TakeDamage(float baseDamage)
    {
        //do nothing with undirectional damge
    }

    [EasyButtons.Button]
    public void Collapse()
    {
        collapsed = true;
        GetComponent<Collider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
    
    [EasyButtons.Button]
    public void Rebuild()
    {
        collapsed = false;
        GetComponent<Collider2D>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
    
    
    #region SAVE SYSTEM
    [System.Serializable]
    protected class TestEntitySaveData : ISaveData //class that is a container for data that will be saved
    {
        public bool destroyed;

        public override string ToString()
        {
            return "destroyed: " + destroyed;
        }
    }

    public ISaveData GatherSaveData() //store current state into the SaveData class
    {
        return new TestEntitySaveData { destroyed = collapsed};
    }
    public void RestoreSaveData(ISaveData state) //receive SaveData class and set variables
    {
        var saveData = (TestEntitySaveData)state;
        collapsed = saveData.destroyed;
        
        if(collapsed)
            Collapse();
        else
            Rebuild();
    }
    #endregion

}
