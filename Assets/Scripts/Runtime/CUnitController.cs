using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CUnitMovementController))]
[RequireComponent(typeof(CUnitWeaponContorller))]
public class CUnitController : MonoBehaviour, IDamageable, ICombatTracker
{
    #region inspector
    [Header("Manager")]
    [SerializeField] private CTurnStateManager _turnStateManager;
 
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

    [Space]
    [Header("knockout")]
    [SerializeField] private CKnockout _knockout;
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
    public float MaxEnergy { get { return _generator.MaxEnergy; } }
    public ScriptableObjectShieldModule ShieldModule {  get { return _shieldModule; } }
    public float Shield { get {  return _shield; } set { _shield = value; } }
    public float PreviousShield { get { return _previousShield; } }
    public int ShieldRegenLevel { get {  return _shieldRegenLevel; } set { _shieldRegenLevel = value; } }

    public CTurnData TurnData { get { return _turnData; } }
    public bool IsReady { get { return _isReady; } set { _isReady = value; } }
    #endregion

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

        /*
        if (_generator == null)
        {
            Debug.LogWarning("Missing _generator");
        }

        else
        {
            _energy = _generator.StartEnergy;
        }
        */

        if (_unitUi == null || _shieldBar == null)
        {
            Debug.LogWarning("Missing Ui element");
        }

        /*
        if (_shieldModule == null)
        {
            Debug.LogWarning("Missing _shieldModule");
        }

        else
        {
            _shield = _shieldModule.StartShield;
            _shieldRegenLevel = 0;
            SetShieldBar();
        }
        */


        if (_knockout == null)
        {
            if(TryGetComponent<CKnockout>(out _knockout)  == false)
            {
                Debug.LogWarning("Missing CKnockout");
            }
        }
    }

    public void UnitModuleInit
        (
        ScriptableObjectGeneratorModule generatorModule,
        ScriptableObjectShieldModule shieldModule,
        ScriptableObjectFlightModule flightModule,
        ScriptableObjectWeaponModule weaponModuleL,
        ScriptableObjectWeaponModule weaponModuleR
        )
    {
        _generator = generatorModule;
        _shieldModule = shieldModule;
        _movementController.SetModule(flightModule);
        _weaponContorller.SetModule(weaponModuleL, weaponModuleR);

        _energy = _generator.StartEnergy;
        _shield = _shieldModule.StartShield;
        _shieldRegenLevel = 0;
        SetShieldBar();

        _turnStateManager = CTurnStateManager.Instance;

        if (_turnStateManager == null)
        {
            Debug.LogWarning("Missing _turnStateManager");
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
        _shieldBar.fillAmount = _shield / _shieldModule.MaxShield;
    }

    public void TakeHit(float damage)
    {
        _shield -= damage;

        SetShieldBar();

        _lastCombatTurn = _turnNum;

        if(_shield <= 0)
        {
            _knockout.enabled = true;
            TryGetComponent<Collider>(out Collider collider);
            collider.enabled = false;
            _turnStateManager.UnitGetKnockedOut(this);
        }
    }

    public void TurnInit(int turnNum)
    {
        _turnNum = turnNum;
        _previousShield = _shield;

        if(_turnNum <= 1)
        {
            return;
        }

        _energy += _generator.EnergyRegen;

        if(_energy > _generator.MaxEnergy)
        {
            _energy = _generator.MaxEnergy;
        }

    }
    public void SetLastCombatTurn()
    {
        _lastCombatTurn = _turnNum;
    }

}
