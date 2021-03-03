using System;
using UnityEngine;

namespace CutsceneSystem
{
    public class Frame : MonoBehaviour
    {
        public float frameDuration = 5;
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
                print("ele");
                StartCoroutine(element.C_Animate());
            }
        }
    }
}