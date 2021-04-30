using UnityEngine;
using System.Collections;
using System.Diagnostics.Contracts;
using Cinemachine;
using UnityEngine.UI;

namespace CutsceneSystem
{
    //Cutscenes contain Frames, Frames contain Elements, Elements control the transform of a specific object
    //Cutscene transitions between frames based on each frame's duration
    //When a frame starts, all of its elements start animating based on the timing variables for each element

    public class Cutscene : MonoBehaviour
    {
        public int firstFrame = 0;
        public Frame[] frames;
        public Transform cam;
        private Image fadeImage;
        

        public int GetFrameCount() //for editor script
        {
            return GetComponentsInChildren<Frame>().Length;
        }

        private void Start()
        {
            frames = GetComponentsInChildren<Frame>();
            fadeImage = GameObject.Find("BlackImage").GetComponent<Image>();
            cam = FindObjectOfType<CinemachineVirtualCamera>().transform;
            StartCoroutine(C_StartCutscene());
        }

        public IEnumerator C_StartCutscene()
        {
            firstFrame = Mathf.Clamp(firstFrame, 0, frames.Length - 1);

            for (int frameIndex = firstFrame;
                frameIndex < frames.Length;
                frameIndex++) //for each frame, move the camera to that frame and play it. start next frame one the previous frame is over
            {
                Frame currentFrame = frames[frameIndex];
                cam.position = currentFrame.transform.position + new Vector3(0, 0, -10);

                StartCoroutine(FadeScreen(Fade.In, currentFrame.fadeInDuration));

                currentFrame.StartFrame();

                yield return new WaitForSeconds(currentFrame.frameDuration - currentFrame.fadeOutDuration);
                StartCoroutine(FadeScreen(Fade.Out, frames[frameIndex].fadeOutDuration));
                yield return new WaitForSeconds(currentFrame.fadeOutDuration);
            }
            LevelUtil.Get().LoadSavedGame();
        }

        IEnumerator FadeScreen(Fade fadeType, float fadeTime)
        {
            float currentTime = 0;
            Color newColor = fadeImage.color;
            while (currentTime <= fadeTime)
            {
                if (fadeType == Fade.Out)
                    newColor.a = currentTime / fadeTime; //if fade out, go from 0 to 1
                else
                    newColor.a = (fadeTime - currentTime) / fadeTime; //if fade in, fo from 1 to 0

                fadeImage.color = newColor;
                currentTime += Time.deltaTime;
                yield return null;
            }
        }

        enum Fade
        {
            In,
            Out
        };
    }
}