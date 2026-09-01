using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDebugTool : MonoBehaviour
{
    public void GetRandomName()
    {
        Debug.Log($"Random Name: {CUtil.GetRandomName()}");
        //Debug.Log($"Random Name: {familyName}");
    }
}
