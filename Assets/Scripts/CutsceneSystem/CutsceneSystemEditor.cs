using UnityEngine;
using System.Collections;
using CutsceneSystem;
using UnityEditor;

[CustomEditor(typeof(Element))]
public class CutsceneSystemEditor : Editor
{
    private float gap = 10;

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        Element element = (Element) target;

        Element myScript = (Element) target;

        GUILayout.Label("Timing");
        GUILayout.BeginHorizontal();
        element.elementStartTime = EditorGUILayout.FloatField("Element Start Time", element.elementStartTime);
        element.elementDuration = EditorGUILayout.FloatField("Element Duration", element.elementDuration);
        GUILayout.EndHorizontal();

        GUILayout.Space(gap);

        GUILayout.Label("Transform");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Go To Start Transform"))
            myScript.GoToStartTransform();
        if (GUILayout.Button("Go To End Transform"))
            myScript.GoToEndTransform();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Start Transform"))
            myScript.SetStartTransform();
        if (GUILayout.Button("Set End Transform"))
            myScript.SetEndTransform();
        GUILayout.EndHorizontal();

        GUILayout.Space(gap);

        GUILayout.Label("Position");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Start Position"))
            myScript.SetStartPosition();
        if (GUILayout.Button("Set End Position"))
            myScript.SetEndPosition();
        GUILayout.EndHorizontal();
        element.positionAnimationCurve =
            EditorGUILayout.CurveField("Position Animation Curve", element.positionAnimationCurve);
        element.startPosition = EditorGUILayout.Vector2Field("Start Position", element.startPosition);
        element.endPosition = EditorGUILayout.Vector2Field("End Position", element.endPosition);

        GUILayout.Space(gap);

        GUILayout.Label("Rotation");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Start Rotation"))
            myScript.SetStartRotation();
        if (GUILayout.Button("Set End Rotation"))
            myScript.SetEndRotation();
        GUILayout.EndHorizontal();
        element.positionAnimationCurve =
            EditorGUILayout.CurveField("Rotation Animation Curve", element.rotationAnimationCurve);
        element.startRotation = EditorGUILayout.FloatField("Start Rotation", element.startRotation);
        element.endRotation = EditorGUILayout.FloatField("End Rotation", element.endRotation);

        GUILayout.Space(gap);

        GUILayout.Label("Scale");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Start Scale"))
            myScript.SetStartScale();
        if (GUILayout.Button("Set End Scale"))
            myScript.SetEndScale();
        GUILayout.EndHorizontal();
        element.positionAnimationCurve =
            EditorGUILayout.CurveField("Scale Animation Curve", element.scaleAnimationCurve);
        element.startScale = EditorGUILayout.Vector2Field("Start Scale", element.startScale);
        element.endScale = EditorGUILayout.Vector2Field("End Scale", element.endScale);
    }
}