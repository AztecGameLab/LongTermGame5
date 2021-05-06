using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PlayerFreeze : MonoBehaviour
{
    [SerializeField, EventRef] private string freezeStart = "event:/Enemies/Water Enemy/Water Enemy Ice Ball Hit Player + Freeze";
    
    IEnumerator Start()
    {
        RuntimeManager.PlayOneShotAttached(freezeStart, gameObject);
        PlatformerController.instance.lockControls = true;
        var sr = PlatformerController.instance.GetComponent<SpriteRenderer>();
        var playerIce = Instantiate(Resources.Load("PlayerIce") as GameObject, sr.bounds.center, Quaternion.identity);
        playerIce.transform.parent = PlatformerController.instance.transform;
        playerIce.transform.localScale = Vector3.one * sr.bounds.size.y;
        
        yield return new WaitForSecondsRealtime(1f);

        PlatformerController.instance.lockControls = false;
        Destroy(playerIce);
        Destroy(this);
    }
}
