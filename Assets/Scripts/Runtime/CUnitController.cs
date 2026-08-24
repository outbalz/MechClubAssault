using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CUnitMovementController))]
public class CUnitController : MonoBehaviour
{
    #region inspector
    [SerializeField] private CUnitMovementController _movementController;
    [SerializeField] private float _speed;
    [SerializeField] private float _turnRate;
    [SerializeField] private float _shield;
    #endregion

    #region private var
    private CTurnData _turnData;
    private int _turnNum = 0;
    #endregion

    #region getter
    public CUnitMovementController MovementController { get { return _movementController; } }
    public float Speed { get { return _speed; } }
    public float TurnRate { get { return _turnRate; } }
    public CTurnData TurnData { get { return _turnData; } }
    #endregion


    private void Reset()
    {
        InitializeUnit();
    }

    private void Awake()
    {
        InitializeUnit();
    }

    private void InitializeUnit(int turnNum = 0)
    {
        _turnNum = turnNum;

        if (_movementController == null)
        {
            if (TryGetComponent<CUnitMovementController>(out _movementController) == false)
            {
                Debug.LogWarning("Missing CUnitMovementController");
            }
        }

        if (_turnData == null)
        {
            _turnData = new CTurnData(_turnNum);
        }
    }


}
