using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

/****************************************
 *  Camera will simulate a shaking motion. 
 *  Need to connect the player sprite to
 *  to the CameraShake.cs script.
 ****************************************/
public class CameraShake : MonoBehaviour{
    public static CameraShake instance;
    public Transform player;

    [SerializeField] private float maxOffset = 0.1f;
    [SerializeField] private float frequency = 8.0f;
    [SerializeField] private float shakeTimeRemaining = 0f;
    [SerializeField] private float rotationMultiplier = 7.0f;
    
    private void Start()
    {
        instance = this;
    }
    
    private void LateUpdate(){
        if (shakeTimeRemaining > 0){
            shakeTimeRemaining -= Time.deltaTime;

            Vector3 offset = new Vector3(GetPerlinNoise(1) * maxOffset, GetPerlinNoise(2) * maxOffset, 0f);

            // moves the camera around the assumed cameras position. 
            transform.position = offset + AssumedCamPos();

            // rotates the camera
            transform.localRotation = Quaternion.Euler(0f, 0f, GetPerlinNoise(3) * 2 * rotationMultiplier);

            // Resets the cameras position and rotation to assumed values before the shake.
            // When shakeTimeRemaining has reached 0, this will run once.
            if (shakeTimeRemaining <= 0){
                transform.position = AssumedCamPos();
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }
    // GetPerlinNoise() - instead of returning a number from [0,1], this will also return negative numbers. 
    public float GetPerlinNoise(float seed){
        return (Mathf.PerlinNoise(Time.time * frequency, seed) * 2 - 1);
    }

    // GetPlayerPos() - Returns the Cameras assumed location, relative to the players position.
    // if there is a script that follows the player, this function might not be needed.
    private Vector3 AssumedCamPos(){
        return new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    // Other varibles like frequency could be add to this StartShake function later. 
    [Button]
    public void StartShake(float time){
        shakeTimeRemaining = time;
    }
}

