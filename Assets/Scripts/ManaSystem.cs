using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem : FillSystem
{
    public static ManaSystem instance;
    private void Awake()
    {
        instance = this;
    }
}
