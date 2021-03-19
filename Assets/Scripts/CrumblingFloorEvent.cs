using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingFloorEvent : MonoBehaviour
{
    public float crumbleDelay;

    public AudioClip crumbling;

    public AudioClip crumblingWarning;

    public float timerCountDown = 1;

    bool isColliding = false;

    private void Update()
    {
        if (isColliding == true)
        {
            timerCountDown -= Time.deltaTime;
            if (timerCountDown < 0)
            {
                timerCountDown = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player has come in contact with the platform");
            AudioSource.PlayClipAtPoint(crumblingWarning, gameObject.transform.position);
            isColliding = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(timerCountDown <= 0)
            {
                AudioSource.PlayClipAtPoint(crumbling, gameObject.transform.position);
                Debug.Log("destroying object after one second");
                Destroy(gameObject.transform.parent.gameObject, crumbleDelay);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Player has left platform before colllapse");
            isColliding = false;
            timerCountDown = 1;
        }
    }
}
