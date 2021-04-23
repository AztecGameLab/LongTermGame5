using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : Singleton<Minimap>
{
    public CanvasGroup canvasGroup;
    private Vector2 center = new Vector2(150, 150);

    private static Texture2D backgroundTexture;
    private static GUIStyle textureStyle;

    // Start is called before the first frame update
    void Start()
    {
        getRooms();
        backgroundTexture = Texture2D.whiteTexture;
        textureStyle = new GUIStyle { normal = new GUIStyleState { background = backgroundTexture } };
    }

    private void getRooms()
    {
        GameObject[] boundingBoxes = GameObject.FindGameObjectsWithTag("BoundingBox");
        
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