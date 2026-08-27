using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CUnitMovementController))]
public class CUnitController : MonoBehaviour, IDamageable, ICombatTracker
{
    #region inspector
    [Header("Energy")]
    [SerializeField] private ScriptableObjectGeneratorModule _generator;
    [SerializeField] private float _energy;

    [Space]
    [Header("Movement")]
    [SerializeField] private CUnitMovementController _movementController;

    [Space]
    [Header("UI")]
    [SerializeField] private Transform _unitUi;
    [SerializeField] private Image _shieldBar;

    [Space]
    [Header("Shield")]
    [SerializeField] private ScriptableObjectShieldModule _shieldModule;
    [SerializeField] private float _shield;

    [Space]
    [Header("Line Renderer")]
    [SerializeField] private LineRenderer _lineRenderer;

    [Space]
    [Header("weapon")]
    [SerializeField] private CUnitWeaponContorller _weaponContorller;
    #endregion

    #region private var
    private CTurnData _turnData;
    private int _turnNum = 0;
    private int _lastCombatTurn = 0;
    private Transform _cameraTr;
    private bool _isReady = false;
    private float _previousShield;
    private int _shieldRegenLevel = 0;
    #endregion

    #region getter
    public CUnitMovementController MovementController { get { return _movementController; } }
    public CUnitWeaponContorller WeaponContorller { get { return _weaponContorller; } }
    public float Energy { get { return _energy; } set { _energy = value; } }
    public float MaxEnergy { get { return _generator._maxEnergy; } }
    public ScriptableObjectShieldModule ShieldModule {  get { return _shieldModule; } }
    public float Shield { get {  return _shield; } set { _shield = value; } }
    public float PreviousShield { get { return _previousShield; } }
    public int ShieldRegenLevel { get {  return _shieldRegenLevel; } set { _shieldRegenLevel = value; } }

    public CTurnData TurnData { get { return _turnData; } }
    public bool IsReady { get { return _isReady; } set { _isReady = value; } }
    #endregion


    private void Reset()
    {
        InitializeUnit();
    }

    private void Awake()
    {
        InitializeUnit();
    }

    private void LateUpdate()
    {
        _unitUi.rotation = _cameraTr.rotation;
    }

    private void InitializeUnit(int turnNum = 0)
    {
        _turnNum = turnNum;
        _lastCombatTurn = turnNum;

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

        if(_weaponContorller == null)
        {
            if(TryGetComponent<CUnitWeaponContorller>(out _weaponContorller) == false)
            {
                Debug.LogWarning("Missing CUnitWeaponContorller");
            }
        }

        if (_unitUi == null)
        {
            Debug.LogWarning("Missing _unitUi");
        }

        if (_generator == null)
        {
            Debug.LogWarning("Missing _generator");
        }

        else
        {
            _energy = _generator._startEnergy;
        }

        if (_unitUi == null || _shieldBar == null)
        {
            Debug.LogWarning("Missing Ui element");
        }

        if (_shieldModule == null)
        {
            Debug.LogWarning("Missing _shieldModule");
        }

        else
        {
            _shield = _shieldModule._startShield;
            _shieldRegenLevel = 0;
            SetShieldBar();
        }
    }

    public void VisualizePath(List<Vector3> posList)
    {
        Vector3[] posArr = posList.ToArray();

        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = posArr.Length;
        _lineRenderer.SetPositions(posArr);
    }

    public void SetShieldBar()
    {
        _shieldBar.fillAmount = _shield / _shieldModule._maxShield;
    }

    public void TakeHit(float damage)
    {
        _shield -= damage;

        SetShieldBar();

        _lastCombatTurn = _turnNum;
    }

    public void TurnInit(int turnNum)
    {
        _turnNum = turnNum;
        _previousShield = _shield;

        if(_turnNum <= 1)
        {
            return;
        }

        _energy += _generator._energyRegen;

        if(_energy > _generator._maxEnergy)
        {
            _energy = _generator._maxEnergy;
        }

    }
    public void SetLastCombatTurn()
    {
        _lastCombatTurn = _turnNum;
    }

}
