using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class crumblingfloor : MonoBehaviour
{

    [EventRef] public string crumbling;
    [EventRef] public string crumblingWarning;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("player has come in contact with the platform");
            RuntimeManager.PlayOneShot(crumblingWarning, transform.position);
            StartCoroutine(crumbleTrigger());
        }
    }

    private void crumbleTriggered()
    {
        RuntimeManager.PlayOneShot(crumbling, transform.position);
        Destroy(this.gameObject, 1);
    }

    IEnumerator crumbleTrigger()
    {
        GetComponentInChildren<Animator>().SetTrigger("Crumble");
        RuntimeManager.PlayOneShot(crumbling, transform.position);
        yield return new WaitForSeconds(1);
        GetComponent<Collider2D>().enabled = false;
        
        yield return new WaitForSeconds(4);
        ResetCrumble();
    }

    void ResetCrumble()
    {
        GetComponentInChildren<Animator>().SetTrigger("Reset");
        GetComponent<Collider2D>().enabled = true;
    }

}