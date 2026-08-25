using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CUnitMovementController))]
public class CUnitController : MonoBehaviour
{
    #region inspector
    [Header("Movement")]
    [SerializeField] private CUnitMovementController _movementController;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _turnRate;

    [Space]
    [Header("Shield")]
    [SerializeField] private float _shield;

    [Space]
    [Header("Line Renderer")]
    [SerializeField] private LineRenderer _lineRenderer;
    #endregion

    #region private var
    private CTurnData _turnData;
    private int _turnNum = 0;
    #endregion

    #region getter
    public CUnitMovementController MovementController { get { return _movementController; } }
    public float Speed { get { return _speed; } }
    public float MaxSpeed {  get { return _maxSpeed; } }
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

    private void Start()
    {
        GetSpeed();
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

        if (_lineRenderer == null)
        {
            if (TryGetComponent<LineRenderer>(out _lineRenderer) == false)
            {
                Debug.LogWarning("Missing LineRenderer");
            }

            else
            {
                _lineRenderer.positionCount = 10;
                _lineRenderer.enabled = false;
            }

        }
    }

    public void VisualizePath(List<Vector3> posList)
    {
        Vector3[] posArr = posList.ToArray();

        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = posArr.Length;
        _lineRenderer.SetPositions(posArr);
    }
    
    public void GetSpeed()
    {
        _movementController.GetSpeed(out _speed, out _turnRate, out _maxSpeed);
    }

}
