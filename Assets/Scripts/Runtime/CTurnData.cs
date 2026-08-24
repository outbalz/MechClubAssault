using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTurnData
{
    private int _turnNum;
    private Vector3[] _positions;
    private Vector3 _dest;

    private int _destReachSec;

    public int TurnNum { get { return _turnNum; } set { _turnNum = value; } }
    public Vector3[] Positions { get { return _positions; } set { _positions = value; } }
    public Vector3 Dest { get { return _dest; } set { _dest = value; } }
    public int DestReachSec { get { return _destReachSec; } set { _destReachSec = value; } }


    public CTurnData(int turnNum)
    {
        _positions = new Vector3[10];
        _turnNum = turnNum;
    }
}
