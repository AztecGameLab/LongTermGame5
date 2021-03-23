using UnityEngine;

namespace CutsceneSystem
{
    public class Frame : MonoBehaviour
    {
        public float frameDuration = 5;
        public float fadeInDuration = 0.5f;
        public float fadeOutDuration = 0.5f;
        private Element[] elements;

        private void Awake()
        {
            elements = GetComponentsInChildren<Element>();
        }

        public void StartFrame()
        {
            if (elements == null)
                return;

            foreach (Element element in elements)
            {
                StartCoroutine(element.C_Animate());
            }
        }
    }
}