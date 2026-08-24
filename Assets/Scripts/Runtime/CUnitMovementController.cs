using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitMovementController : MonoBehaviour
{
    #region inspector
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    #endregion

    #region private var
    private Vector3 _targetPos;
    private bool _onMove;
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
            _onMove = UnitMovemet(_targetPos);
        }
    }

    private bool UnitMovemet(Vector3 dest)
    {
        if ((dest - transform.position).sqrMagnitude > _movementSpeed * _movementSpeed * Time.deltaTime * Time.deltaTime + Time.deltaTime)
        {
            Vector3 dir = (dest - transform.position).normalized;

            transform.position += dir * _movementSpeed * Time.deltaTime;

            UnitRotation(dir);

            return true;
        }

        else
        {
            transform.position = dest;
        }

        return false;
    }

    private void UnitRotation(Vector3 dir)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), GetSmoothT(_rotationSpeed));
    }

    private float GetSmoothT(float sharpness)
    {
        return 1f - Mathf.Exp(-sharpness * Time.deltaTime);
    }


    public void SetTargetPos(Vector3 pos)
    {
        _targetPos = pos;
    }

    public bool IsOnMove()
    {
        return _onMove;
    }

    public void SetOnMove(bool onMove = true)
    {
        _onMove = onMove;
    }

}
