using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{

    public Transform[] backgrounds;     // Array (list) of all the backgournd and foreground elements
    private float[] parallaxScales;     
    public float smoothing = 1f;        // How smooth the parallax is going to be. Set above 0

    private Transform cam;              
    private Vector3 previousCamPos;     

     void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];

        for(int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z*-1;
        }
    }

    
    void Update()
    {
       for(int i = 0; i < backgrounds.Length; i++)
        {
            float parallax_x = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float parallax_y = (previousCamPos.y - cam.position.y) * parallaxScales[i];

            float backgroundTargetposX = backgrounds[i].position.x + parallax_x;
            float backgroundTargetposY = backgrounds[i].position.y + parallax_y;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetposX, backgroundTargetposY, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
