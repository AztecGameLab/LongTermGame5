using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebuggingInputTool : MonoBehaviour
{
    private void Start()
    {
        Debug.LogWarning("DebuggingInputTool in use, delete before build");
    }
}
