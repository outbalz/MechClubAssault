using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CUnitMovementController : MonoBehaviour
{
    #region inspector (debug)
    [Header("debug")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    #endregion

    #region private var
    private Vector3 _targetPos;
    private bool _onMove;

    private Vector3 _p1;
    private Vector3 _p1Nomal;
    private Vector3 _p2;
    private Vector3 _p3;

    private float _t;
    private int _tScale;
    #endregion


    private void Awake()
    {
        _targetPos = transform.position;
        _onMove = false;
    }

    void Update()
    {
        if (_onMove)
        {
            _onMove = UnitMovemet();
        }
    }

    private bool UnitMovemet()
    {
        _t += Time.deltaTime / _tScale;

        if(_t >= 1)
        {
            return false;
        }
        
        Vector3 moveVector = GetBezierVector(_t);

        Vector3 dir = moveVector - transform.position;

        UnitRotation(dir);

        transform.position = moveVector;


        return true;
    }

    private void UnitRotation(Vector3 dir)
    {
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    private void GetBezierPoint()
    {

        _p1 = transform.position;
        _p1Nomal = transform.forward;
        _p3 = _targetPos;

        _t = 0;

        float baseLength = (_p3 - _p1).magnitude;

        float radian = Vector3.Angle(_p1Nomal, _p3 - _p1) * Mathf.Deg2Rad;

        float vectorLength = baseLength / (2f * Mathf.Cos(radian));

        _p2 = _p1 +  _p1Nomal * vectorLength;
    }

    private Vector3 GetBezierVector(float t)
    {
        Vector3 L1 = Vector3.Lerp(_p1, _p2, t);
        Vector3 L2 = Vector3.Lerp(_p2, _p3, t);

        return Vector3.Lerp(L1, L2, t);
    }




    public void SetTargetPos(Vector3 pos, int sec)
    {
        _targetPos = pos;
        _onMove = true;

        _tScale = sec;

        GetBezierPoint();
    }

    public bool IsOnMove()
    {
        return _onMove;
    }

    public void SetOnMove(bool onMove = true)
    {
        _onMove = onMove;
    }

    public void SetSpeed(float movementSpeed, float rotationSpeed)
    {
        _movementSpeed = movementSpeed;
        _rotationSpeed = rotationSpeed;
    }

}
