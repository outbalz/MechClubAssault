using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitMovementController : MonoBehaviour
{
    #region inspector (debug)
    [Header("debug")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    #endregion

    #region private var
    private Vector3 _targetPos;
    private Vector3 _finalTargetPos;
    private bool _onMove;
    private bool _reachedDest;
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

        transform.position += transform.rotation * Vector3.forward * _movementSpeed * Time.deltaTime;

        if ((dest - transform.position).sqrMagnitude <= _movementSpeed * _movementSpeed + _movementSpeed)
        {
            return true;
        }

        return false;
    }
    /*
    private void UnitMovemet()
    {
        transform.position += transform.rotation * Vector3.forward * _movementSpeed * Time.deltaTime;
    }
    */
    private void UnitRotation(Vector3 dir)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), _rotationSpeed * Time.deltaTime);
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

    public void SetSpeed(float movementSpeed, float rotationSpeed)
    {
        _movementSpeed = movementSpeed;
        _rotationSpeed = rotationSpeed;
    }

}
