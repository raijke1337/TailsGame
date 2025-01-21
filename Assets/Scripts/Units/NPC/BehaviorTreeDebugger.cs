using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeDebugger 
{
    string cachedMessage;
    public void PrintDebug(string message)
    {
        if (message != cachedMessage)
        {
            cachedMessage = message;
            Debug.Log(message);
        }
    }
}
