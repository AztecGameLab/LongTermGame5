using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingToolExample : MonoBehaviour
{

    //Here is where you connect your debugging tool
    // you can use OnZeroKey, OnOneKey to OnNineKey 
    public void OnOneKey() //function that runs when you press the one key
    {
        YourFunctionYouWantToTest();
    }
    public void OnTwoKey() //function that runs when you press the two key
    {
        print(AnotherFunctionToTest()); //if you want to see what your function outputs, just put it in a print()
    }


    //Below this is your normal work that you want to test

    public void YourFunctionYouWantToTest()
    {
        print("first function");
    }

    private bool AnotherFunctionToTest()
    {
        print("second function");
        return true;
    }
}
