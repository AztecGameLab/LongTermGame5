using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class soundEvent : MonoBehaviour
{
    [EventRef] public string sound = "Sound";

    public void PlaySound()
    {
        RuntimeManager.PlayOneShot(sound, transform.position);
    }
}
