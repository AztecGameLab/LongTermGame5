using UnityEngine;
using System.Collections;

namespace CutsceneSystem
{
    public class Cutscene : MonoBehaviour
    {
        public int firstFrame = 0;
        public Frame[] frames;
        private Camera cam;

        private void Awake()
        {
            frames = GetComponentsInChildren<Frame>();
            cam = Camera.main;
        }

        public int GetFrameCount()
        {
            return GetComponentsInChildren<Frame>().Length;
        }

        private void Start()
        {
            StartCoroutine(C_StartCutscene());
        }

        public IEnumerator C_StartCutscene()
        {
            firstFrame = Mathf.Clamp(firstFrame, 0, frames.Length - 1);

            for (int frameIndex = firstFrame; frameIndex < frames.Length; frameIndex++)
            {
                cam.transform.position = frames[frameIndex].transform.position + new Vector3(0, 0, -10);
                frames[frameIndex].StartFrame();
                yield return new WaitForSeconds(frames[frameIndex].frameDuration);
            }
        }
    }
}