using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformerParameters", menuName = "2D/Entities/PlatformerParameters", order = 0)]
public class PlatformerParameters : EntityData
{

    [Tooltip("How long will the player be invincible after taking damage (s)")]
    public float InvincibilityTime = 1;

    [Tooltip("How powerfull is the knockback (s)")]
    public float KnockBackIntensity = 1;

    [Tooltip("How long will controls lock for the player")]
    public float KnockBackTime = 1;

    #region JumpingParameters

    [Header("Jump Parameters")]

    [Tooltip("How much time you are still allowed to jump after leaving a ledge (seconds)")]
    public float CoyoteTime = 1;
    
    [Tooltip("Max height a jump can go when held down (meters)")]
    public float JumpHeight = 1;

    [Tooltip("How long will a jump input stay in the jump buffer (seconds)")]
    public float JumpBufferTime = 0.1f;

    [Tooltip("How many jumps does our player have")]
    public int JumpCount = 1;

    [Tooltip("How fast does a jump move us up")]
    public float JumpSpeed = 1;

    #endregion

    


    #region StickMovement
    [Space]
    [Header("Stick Inputs")]

    [Tooltip("Maximum fall velocity for the player")]
    public float MaxFallSpeed = 2;

    [Tooltip("What is the fastest our player can accelerate to under normal circustances(rad)")]
    public float MaxRunSpeed = 2;

    [Tooltip("At what angle is something NOT considered under our feet (+- degrees)")]
    public float MaxGroundAngle = 45;

    [Tooltip("The is the acceleration curve of our player")]
    public float AccelerationMultiplier;

    [Tooltip("The amount of time from leaving the ground will we still consider grounded")]
    public float GroundedTimeout = .25f;

    #endregion

    #region attacking
    [Space]
    [Header("Attacking Parameters")]

    [Tooltip("How much damage does the basic attack use")]
    public float BasicBaseDamage = 1;

    [Tooltip("At what point does the attack damage an enemy (sec)")]
    public float BasicAttackDelay = .25f;

    [Tooltip("How long is a basic attack valid (sec)")]
    public float BasicAttackForgiveness = .25f;

    [Tooltip("How long until we can use our basic attack again (sec) [must be larger than Attack Delay + Basic Attack Forgveness]")]
    public float BasicAttackCooldown = .25f;

    [Tooltip("Float Attack Size")]
    public float BasicAttackSize = .5f;

    [Tooltip("How far can we attack")]
    public float BasicAttackRange = 3f;

    [Tooltip("How fast should time move while you are aiming your weapon")]
    public float BulletTimeSlowDown = 0.1f;

    

    #endregion
}
