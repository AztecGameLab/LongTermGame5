using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    [SerializeField, EventRef] private string freezeStart = "event:/Enemies/Water Enemy/Water Enemy Ice Ball Hit Player + Freeze";
    [SerializeField, EventRef] private string freezeEnd = "event:/Enemies/Water Enemy/Water Enemy Ice Ball Unfreeze Player";

    // Start is called before the first frame update
    IEnumerator Start()
    {
        RuntimeManager.PlayOneShotAttached(freezeStart, gameObject);
        Collider2D collider = GetComponent<Collider2D>();
        Debug.Log("Made it");
        collider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        var sr = GetComponentInChildren<SpriteRenderer>();
        var go = Instantiate(Resources.Load("Ice") as GameObject, sr.bounds.center, quaternion.identity);
        go.transform.localScale = Vector3.one * sr.bounds.size.y;
        
        yield return new WaitForSeconds(4.0f);
        
        RuntimeManager.PlayOneShotAttached(freezeEnd, gameObject);

        yield return new WaitForSecondsRealtime(1.0f);
        
        Destroy(go);
        collider.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        Debug.Log("unfreeze");
        
        Destroy(this);
    }


}
