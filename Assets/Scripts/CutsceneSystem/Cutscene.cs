using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


namespace CutsceneSystem
{
    public class Cutscene : MonoBehaviour
    {
        private Frame[] frames;
        private Camera cam;

        private void Awake()
        {
            frames = GetComponentsInChildren<Frame>();
            cam = Camera.main;
        }

        private void Start()
        {
            StartCoroutine(C_StartCutscene());
        }

        public IEnumerator C_StartCutscene()
        {
            for (int frameIndex = 0; frameIndex < frames.Length; frameIndex++)
            {
                cam.transform.position = frames[frameIndex].transform.position + new Vector3(0, 0, -10);
                frames[frameIndex].StartFrame();
                yield return new WaitForSeconds(frames[frameIndex].frameDuration);
            }
        }
    }
}