using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTurnData
{
    private int _turnNum;
    private Vector3[] _positions;

    public int TurnNum { get { return _turnNum; } set { _turnNum = value; } }
    public Vector3[] Positions { get { return _positions; } set { _positions = value; } }

    public CTurnData(int turnNum)
    {
        _positions = new Vector3[10];
        _turnNum = turnNum;
    }
}
