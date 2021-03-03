using System;
using UnityEngine;

namespace CutsceneSystem
{
    public class Cutscene : MonoBehaviour
    {
        private Frame[] frames;

        private void Start()
        {
            frames = GetComponentsInChildren<Frame>();
        }
    }
}
