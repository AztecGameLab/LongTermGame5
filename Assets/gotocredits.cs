using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gotocredits : MonoBehaviour
{
    public Level credits;
    public void Credits()
    {
        LevelUtil.Get().TransitionTo(credits);
    }
}
