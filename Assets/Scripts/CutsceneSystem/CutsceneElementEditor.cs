using UnityEditor;
using UnityEngine;

namespace CutsceneSystem
{
    [CustomEditor(typeof(Element))]
    public class CutsceneElementEditor : Editor
    {
        private float gap = 10;

        public override void OnInspectorGUI()
        {
            Element element = (Element) target;

            GUILayout.Label("Timing");
            GUILayout.BeginHorizontal();
            element.elementStartTime = EditorGUILayout.FloatField("Element Start Time", element.elementStartTime);
            element.elementDuration = EditorGUILayout.FloatField("Element Duration", element.elementDuration);
            GUILayout.EndHorizontal();

            GUILayout.Space(gap);

            GUILayout.Label("Transform");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Go To Start Transform"))
                element.GoToStartTransform();
            if (GUILayout.Button("Go To End Transform"))
                element.GoToEndTransform();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Start Transform"))
                element.SetStartTransform();
            if (GUILayout.Button("Set End Transform"))
                element.SetEndTransform();
            GUILayout.EndHorizontal();

            GUILayout.Space(gap);

            GUILayout.Label("Position");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Start Position"))
                element.SetStartPosition();
            if (GUILayout.Button("Set End Position"))
                element.SetEndPosition();
            GUILayout.EndHorizontal();
            element.positionAnimationCurve =
                EditorGUILayout.CurveField("Position Animation Curve", element.positionAnimationCurve);
            element.startPosition = EditorGUILayout.Vector2Field("Start Position", element.startPosition);
            element.endPosition = EditorGUILayout.Vector2Field("End Position", element.endPosition);

            GUILayout.Space(gap);

            GUILayout.Label("Rotation");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Start Rotation"))
                element.SetStartRotation();
            if (GUILayout.Button("Set End Rotation"))
                element.SetEndRotation();
            GUILayout.EndHorizontal();
            element.rotationAnimationCurve =
                EditorGUILayout.CurveField("Rotation Animation Curve", element.rotationAnimationCurve);
            element.startRotation = EditorGUILayout.FloatField("Start Rotation", element.startRotation);
            element.endRotation = EditorGUILayout.FloatField("End Rotation", element.endRotation);

            GUILayout.Space(gap);

            GUILayout.Label("Scale");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Start Scale"))
                element.SetStartScale();
            if (GUILayout.Button("Set End Scale"))
                element.SetEndScale();
            GUILayout.EndHorizontal();
            element.scaleAnimationCurve =
                EditorGUILayout.CurveField("Scale Animation Curve", element.scaleAnimationCurve);
            element.startScale = EditorGUILayout.Vector2Field("Start Scale", element.startScale);
            element.endScale = EditorGUILayout.Vector2Field("End Scale", element.endScale);
        }
    }
}