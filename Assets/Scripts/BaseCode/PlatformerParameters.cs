using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformerParameters", menuName = "2D/CharacterController/PlatformerParameters", order = 0)]
public class PlatformerParameters : ScriptableObject
{
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

    [Tooltip("Maximum fall velocity for the player")]
    public float MaxFallSpeed = 2;

    [Tooltip("What is the fastest our player can accelerate to under normal circustances(rad)")]
    public float MaxRunSpeed = 2;

    [Tooltip("At what angle is something NOT considered under our feet (+- degrees)")]
    public float MaxGroundAngle = 45;

    [Tooltip("The is the acceleration curve of our player")]
    public float AccelerationMultiplier;

    [Tooltip("The is the deceleration curve of our player")]
    public AnimationCurve DecelerationProfile;
}
