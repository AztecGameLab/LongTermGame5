using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    private Vector2 center = new Vector2(0,0);

    private static List<Room> rooms;
    private static Texture2D backgroundTexture;
    private static GUIStyle textureStyle;

    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<Room>();
        getRooms();
        backgroundTexture = Texture2D.whiteTexture;
        textureStyle = new GUIStyle { normal = new GUIStyleState { background = backgroundTexture } };
    }

    private void getRooms()
    {
        rooms.Add(new Room(new Rect(100, 100, 50, 50), -1));
        rooms.Add(new Room(new Rect(150, 150, 50, 50), 0));
        rooms.Add(new Room(new Rect(200, 200, 50, 50), 1));
        //TODO: Get rooms from bounding boxes
    }

    private void OnGUI()
    {
        foreach (Room r in rooms)
        {
            if (r.getDiscovered() >= 0)
            {
                DrawRect(r.getRect(), (r.getDiscovered() == 1 ? Color.white : Color.yellow));
            }
            
        }
        
    }

    public static void DrawRect(Rect position, Color color, GUIContent content = null)
    {
        var backgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
        GUI.Box(position, content ?? GUIContent.none, textureStyle);
        GUI.backgroundColor = backgroundColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

class Room
{
    Rect roomRect;
    int discovered = -1;

    public Room(Rect rect, int discovered = -1)
    {
        this.roomRect = rect;
        this.discovered = discovered;
    }
    public Rect getRect()
    {
        return roomRect;
    }

    public int getDiscovered()
    {
        return discovered;
    }
}