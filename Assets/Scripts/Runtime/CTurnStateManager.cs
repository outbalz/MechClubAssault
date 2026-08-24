using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CTurnStateManager : MonoBehaviour
{
    private enum ETurnState
    {
        TurnInit,
        AwaitPlayerInput,
        AIInput,
        TurnResolve
    }

    private int _turnNum;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
