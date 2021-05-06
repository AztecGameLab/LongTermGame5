using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using FMODUnity;
[CreateAssetMenu(fileName = "FreezeProjectile", menuName = "LTG5/Weapons/IceBall", order = 0)]
public class FreezeProjectile : ProjectileWeapon
{
    public IceBall iceBallPrefab;
    private IceBall _currentIceBall;
    
    [SerializeField]
    private float iceBallSpeed; // launchforce
    [SerializeField]
    private float IceBallSize = 3f; 
    [SerializeField]
    private float upForce;
    [SerializeField]
    private float gravityForce;
    [SerializeField]
    public float damage;
    public float chargeTime;
    private bool isCharging;
    public Vector2 position;
    public Vector2 radius;
    // called when the player releases the button
    override
    public  void Fire(Vector2 direction)
    {
        _currentIceBall.transform.SetParent(PlatformerController.instance.transform.parent);
        position = direction;
        isCharging = false;
        PlatformerController.instance.StopCoroutine(IceBallPower());
        chargeTime = 0f;
        iceBallSpeed = 10 + IceBallSize;
        var targetVelocity = iceBallSpeed * direction + (Vector2) (_currentIceBall.transform.up * upForce);
        _currentIceBall.Launch(targetVelocity, gravityForce);
        
        IceBallSize = 3f;
    }
    //called when the player hold down the button
    override
    public void Charge(Vector2 direction)
    {
        chargeTime = 0f;
        damage = 0;
        _currentIceBall = Instantiate(iceBallPrefab, iceBallPrefab.transform.position, Quaternion.identity);
        _currentIceBall.transform.SetParent(PlatformerController.instance.transform);
        _currentIceBall.transform.localPosition = (direction * 1.5f);
        _currentIceBall.GetComponent<Collider2D>().enabled = false;
        isCharging = true;
        PlatformerController.instance.StartCoroutine(IceBallPower());
    }
    //runs after charge
    override
    public void OnAimChange(Vector2 direction)
    {
        _currentIceBall.transform.localPosition = (direction * 1.5f);
    }
   private IEnumerator IceBallPower()
    {
        while (chargeTime < 3 && isCharging)
        {
            chargeTime += Time.fixedDeltaTime;
            IceBallSize = 3 + chargeTime;
            _currentIceBall.transform.localScale = new Vector3(IceBallSize, IceBallSize, IceBallSize);
            damage += 0.5f;
            yield return null;
        }
    }
}
