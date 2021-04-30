using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeze : MonoBehaviour
{
    IEnumerator Start()
    {
        PlatformerController.instance.lockControls = true;
        var sr = PlatformerController.instance.GetComponent<SpriteRenderer>();
        var playerIce = Instantiate(Resources.Load("PlayerIce") as GameObject, sr.bounds.center, Quaternion.identity);
        playerIce.transform.parent = PlatformerController.instance.transform;
        playerIce.transform.localScale = Vector3.one * sr.bounds.size.y;
        //Debug.Log("player frozen");
        
        yield return new WaitForSeconds(3);
        
        Destroy(playerIce);
        PlatformerController.instance.lockControls = false;
    }
}
