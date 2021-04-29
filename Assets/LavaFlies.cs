using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FMODUnity;


public class LavaFlies : MonoBehaviour
{
    public float interval = 1.5f;
    public float damagePerTick = 1;
    private bool playerInTrigger;

    [EventRef] public string damageSound;
    [EventRef] public string angrySound;


    private void OnTriggerEnter2D(Collider2D other)
    {
        var pc = other.GetComponent<PlatformerController>();
        if (pc)
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var pc = other.GetComponent<PlatformerController>();
        if (pc)
        {
            playerInTrigger = false;
        }
    }

    IEnumerator Start()
    {
        while (true)
        {
            while (playerInTrigger)
            {
                PlatformerController.instance.TakeDamage(damagePerTick);
                print("tick");
                RuntimeManager.PlayOneShot(damageSound, transform.position);
                yield return new WaitForSeconds(interval);
                yield return null;
            }
            yield return null;
        }
    }
}