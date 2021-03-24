using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Starting Postion:")]
    [SerializeField] bool startAtTop;
    [SerializeField] bool startAtButtom;

    [SerializeField] GameObject elevatorPlatform;
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;
    [SerializeField] float speed;


    // Start is called before the first frame update
    void Start()
    {
        if (startAtTop)
        {
            elevatorPlatform.transform.position = top.position;
        } else if (startAtButtom)
        {
            elevatorPlatform.transform.position = bottom.position;
        }
    }


    [EasyButtons.Button]
    public void MoveUp()
    {
        //Coroutine that moves the platform to the top slowly 
        StartCoroutine(MoveTowrds(top.position));
    }
    [EasyButtons.Button]
    public void MoveDown()
    {
        //Coroutine that moves the platform to the bottom slowly 
        StartCoroutine(MoveTowrds(bottom.position));
    }


    IEnumerator MoveTowrds(Vector2 targetPos)
    {
        Vector2 currentPos = elevatorPlatform.transform.position;
        while (currentPos != targetPos)
        {
            currentPos = elevatorPlatform.transform.position;
            elevatorPlatform.transform.position = Vector2.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
            yield return 0;
        }
        

    }
}
