using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : Singleton<Minimap>
{
    [SerializeField]
    Image air;
    [SerializeField]
    Image bog;
    [SerializeField]
    Image lava;
    [SerializeField]
    Image water;
    [SerializeField]
    Image boss;
    public enum Areas {Air, Lava, Bog, Water, Boss }

    // Start is called before the first frame update
    void Start()
    {
        getRooms();
    }

    public void enableArea(Areas area)
    {
        switch (area)
        {
            case Areas.Air:
                air.gameObject.SetActive(true);
                break;
            case Areas.Water:
                water.gameObject.SetActive(true);
                break;
            case Areas.Lava:
                lava.gameObject.SetActive(true);
                break;
            case Areas.Bog:
                bog.gameObject.SetActive(true);
                break;
            case Areas.Boss:
                boss.gameObject.SetActive(true);
                break;
        }
    }

    private void getRooms()
    {
        enableArea(Areas.Air);
        enableArea(Areas.Lava);

        //TODO: Get rooms from bounding boxes
    }

    public void toggleMinimap(bool isOn)
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}