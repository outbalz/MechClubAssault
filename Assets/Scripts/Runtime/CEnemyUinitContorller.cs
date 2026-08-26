using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CUnitMovementController))]
public class CEnemyUinitContorller : MonoBehaviour, IDamageable
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
    [SerializeField] private float _maxShield;

    [Space]
    [Header("UI")]
    [SerializeField] private Transform _unitUi;
    //[SerializeField] private Slider _shieldBar;

    [SerializeField] private Image _shieldBar;

    [Space]
    [Header("carmera")]
    [SerializeField] private Transform _cameraTr;
    #endregion


    #region private var
    private CTurnData _turnData;
    private int _turnNum = 0;
    #endregion

    #region getter
    public CUnitMovementController MovementController { get { return _movementController; } }
    public float Speed { get { return _speed; } }
    public float MaxSpeed { get { return _maxSpeed; } }
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

    private void LateUpdate()
    {
        _unitUi.rotation =_cameraTr.rotation;
    }

    private void InitializeUnit(int turnNum = 0)
    {
        _turnNum = turnNum;

        _cameraTr = Camera.main.transform;

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

        if(_unitUi == null)
        {
            Debug.LogWarning("Missing _unitUi");
        }

        _shield = _maxShield;
        SetShieldBar();
    }

    private void SetShieldBar()
    {
        //_shieldBar.maxValue = _maxShield;
        //_shieldBar.value = _shield;
        _shieldBar.fillAmount = _shield/_maxShield;
    }

    public void GetSpeed()
    {
        _movementController.GetSpeed(out _speed, out _turnRate, out _maxSpeed);
    }

    public void TakeHit(float damage)
    {
        _shield -= damage;

        SetShieldBar();
    }

}
