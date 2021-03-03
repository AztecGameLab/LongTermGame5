using System;
using UnityEngine;

namespace CutsceneSystem
{
    public class Frame : MonoBehaviour
    {
        private Element[] elements;

        private void Start()
        {
            elements = GetComponentsInChildren<Element>();
        }
    }
}
