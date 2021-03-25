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
            Debug.Log("player has come in contact with the platform");
            AudioSource.PlayClipAtPoint(crumblingWarning, gameObject.transform.position);
            crumbleTriggered();
        }
    }

    private void crumbleTriggered()
    {
        AudioSource.PlayClipAtPoint(crumbling, gameObject.transform.position);
        Destroy(this.gameObject, 1);
    }

}