using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CUnitMovementController : MonoBehaviour
{
    #region inspector (debug)
    [Header("speed")]
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _turnRate;

    [Space]
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _airResistance;
    #endregion

    #region private var
    private Vector3 _targetPos;
    private Vector3 _finalTargetPos;
    private bool _onMove;
    private bool _reachedDest;
    private int _accelerationLevel;
    private float _previousSpeed;
    #endregion


    private void Awake()
    {
        _targetPos = transform.position;
        _onMove = false;
        _reachedDest = false;
    }

    void Update()
    {
        if (_onMove)
        {
            if (_reachedDest == false)
            {
                _reachedDest = UnitMovemet(_targetPos);
            }

            else
            {
                UnitMovemet(_finalTargetPos);
            }
        }
    }

    private bool UnitMovemet(Vector3 dest)
    {
        Vector3 dir = (dest - transform.position).normalized;

        UnitRotation(dir);

        transform.position += transform.rotation * Vector3.forward * _speed * Time.deltaTime;

        if ((dest - transform.position).sqrMagnitude <= _speed * _speed + _speed)
        {
            return true;
        }

        return false;
    }
    /*
    private void UnitMovemet()
    {
        transform.position += transform.rotation * Vector3.forward * _speed * Time.deltaTime;
    }
    */
    private void UnitRotation(Vector3 dir)
    {
        if(dir == Vector3.zero)
        {
            return;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), _turnRate * Time.deltaTime);
    }

    public void SetTargetPos(Vector3 pos, Vector3 fainalPos)
    {
        _targetPos = pos;
        _finalTargetPos = fainalPos;
        _reachedDest = false;
    }

    public bool IsOnMove()
    {
        return _onMove;
    }

    public void SetOnMove(bool onMove = true)
    {
        _onMove = onMove;
    }

    public void SetSpeed(float speed, float turnRate)
    {
        _speed = speed;
        _turnRate = turnRate;
    }

    public void GetSpeed(out float speed, out float turnRate, out float maxSpeed)
    {
        speed = _speed;
        turnRate = _turnRate;
        maxSpeed = _maxSpeed;
    }

    public void SpeedTurnInit()
    {
        _speed -= _airResistance;

        if (_speed < 0)
        {
            _speed = 0;
        }

        _previousSpeed = _speed;
    }

    public void SetAccelerationLevel(int level)
    {
        _accelerationLevel = level;

        float tempSpeed = _previousSpeed;

        if(_accelerationLevel < 0)
        {
            tempSpeed += _deceleration * _accelerationLevel;
        }

        else if(_accelerationLevel > 0)
        {
            tempSpeed += _acceleration * _accelerationLevel;
        }

        _speed = tempSpeed;
    }
}
