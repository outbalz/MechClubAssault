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
                UnitMovemet();
            }
        }
    }

    private bool UnitMovemet(Vector3 dest)
    {
        if ((dest - transform.position).sqrMagnitude > _movementSpeed * _movementSpeed * Time.deltaTime * Time.deltaTime + Time.deltaTime)
        {
            Vector3 dir = (dest - transform.position).normalized;

            UnitRotation(dir);

            transform.position += transform.rotation * Vector3.forward * _movementSpeed * Time.deltaTime;

            return false;
        }

        else
        {
            transform.position = dest;
        }

        return true;
    }

    private void UnitMovemet()
    {
        transform.position += transform.rotation * Vector3.forward * _movementSpeed * Time.deltaTime;
    }

    private void UnitRotation(Vector3 dir)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), _rotationSpeed * Time.deltaTime);
    }

    public void SetTargetPos(Vector3 pos)
    {
        _targetPos = pos;
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
