using UnityEditor;
using UnityEngine;

namespace CutsceneSystem
{
    [CustomEditor(typeof(Cutscene))]
    public class CutsceneEditor : Editor
    {
        public override void OnInspectorGUI() //makes a dynamic slider based on how many frames there are
        {
            Cutscene cutscene = (Cutscene) target;

            GUILayout.Label("Starting Frame");
            cutscene.firstFrame = EditorGUILayout.IntSlider(cutscene.firstFrame, 0, cutscene.GetFrameCount() - 1);
        }
    }
}