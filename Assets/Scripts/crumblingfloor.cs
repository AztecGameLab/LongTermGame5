using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crumblingfloor : MonoBehaviour
{

    public AudioClip crumbling;

    public AudioClip crumblingWarning;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("player has come in contact with the platform");
            AudioSource.PlayClipAtPoint(crumblingWarning, gameObject.transform.position);
            StartCoroutine(crumbleTrigger());
        }
    }

    private void crumbleTriggered()
    {
        AudioSource.PlayClipAtPoint(crumbling, gameObject.transform.position);
        Destroy(this.gameObject, 1);
    }

    IEnumerator crumbleTrigger()
    {
        GetComponentInChildren<Animator>().SetTrigger("Crumble");
        AudioSource.PlayClipAtPoint(crumbling, gameObject.transform.position);
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