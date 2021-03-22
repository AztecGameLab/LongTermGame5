using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    private static Texture2D backgroundTexture;
    private static GUIStyle textureStyle;

    // Start is called before the first frame update
    void Start()
    {
        backgroundTexture = Texture2D.whiteTexture;
        textureStyle = new GUIStyle { normal = new GUIStyleState { background = backgroundTexture } };
    }

    private void OnGUI()
    {
        Rect rect = new Rect(100,100,100,100);
        DrawRect(rect, Color.white);
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
