using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem : FillSystem
{
    private static ManaSystem _instance;

    public static ManaSystem instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ManaSystem>();
            }

            return _instance;
        }
    }
}