using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CUnitMovementController))]
[RequireComponent(typeof(CUnitWeaponContorller))]
public class CEnemyUnitContorller : MonoBehaviour, IDamageable, ICombatTracker
{
    #region inspector
    [Header("Manager")]
    [SerializeField] private CTurnStateManager _turnStateManager;

    [Space]
    [Header("Shield")]
    [SerializeField] private ScriptableObjectShieldModule _shieldModule;
    [SerializeField] private ParticleSystem _shieldEffect;

    [Space]
    [Header("Movement")]
    [SerializeField] private CUnitMovementController _movementController;

    [Space]
    [Header("weapon")]
    [SerializeField] private CUnitWeaponContorller _weaponContorller;

    [Space]
    [Header("UI")]
    [SerializeField] private Transform _unitUi;
    [SerializeField] private Image _shieldBar;

    [Space]
    [Header("carmera")]
    [SerializeField] private Transform _cameraTr;

    [Space]
    [Header("knockout")]
    [SerializeField] private CKnockout _knockout;

    [Space]
    [Header("Style")]
    [SerializeField] private CHairStyleController _hairStyleController;
    [SerializeField] private Renderer _eyeRenderer;
    [SerializeField] private Renderer _eyeLightRenderer;
    #endregion

    #region Debug
    [Header("Debug")]
    [SerializeField] private float _shield;
    #endregion

    #region private var
    private int _turnNum = 0;
    private int _lastCombatTurn = 0;
    private CUnitController _targetUnit;
    #endregion

    #region getter
    public CUnitMovementController MovementController { get { return _movementController; } }
    public CUnitController TargetUnit { set { _targetUnit = value; } }
    #endregion


    private void Reset()
    {
        InitializeUnit();
    }

    private void Awake()
    {
        InitializeUnit();
    }

    private void Update()
    {
        SetShieldBar();
    }

    private void LateUpdate()
    {
        _unitUi.rotation =_cameraTr.rotation;
    }

    private void InitializeUnit(int turnNum = 0)
    {
        _turnNum = turnNum;
        _lastCombatTurn = _turnNum;

        _cameraTr = Camera.main.transform;

        if (_movementController == null)
        {
            if (TryGetComponent<CUnitMovementController>(out _movementController) == false)
            {
                Debug.LogWarning("Missing CUnitMovementController");
            }

        }

        if (_weaponContorller == null)
        {
            if (TryGetComponent<CUnitWeaponContorller>(out _weaponContorller) == false)
            {
                Debug.LogWarning("Missing CUnitWeaponContorller");
            }
        }

        if(_unitUi == null || _shieldBar == null)
        {
            Debug.LogWarning("Missing Ui element");
        }

        if (_knockout == null)
        {
            if (TryGetComponent<CKnockout>(out _knockout) == false)
            {
                Debug.LogWarning("Missing CKnockout");
            }
        }

        if (_hairStyleController == null)
        {
            Debug.LogWarning("Missing _hairStyleController");
        }

        if (_eyeRenderer == null || _eyeLightRenderer == null)
        {
            Debug.LogWarning("Missing Eye renderer");
        }
    }


    private void UnitStyleInit()
    {
        Color hairColor;
        Color highLightColor;
        CUtil.GetRandomHairColor(out hairColor, out highLightColor);
        _hairStyleController.InitializeHair(Random.Range(0, 8),hairColor,highLightColor);

        ScriptableObjectEyeColorData eyeColorData = CGameProgressManager.Instance.SODB.GetRandomEyeColor();

        _eyeRenderer.material = eyeColorData.EyeMaterial;
        _eyeLightRenderer.material = eyeColorData.EyeMaterial;
    }

    public void UnitModuleInit
        (
        ScriptableObjectShieldModule shieldModule,
        ScriptableObjectFlightModule flightModule,
        ScriptableObjectWeaponModule weaponModuleL,
        ScriptableObjectWeaponModule weaponModuleR
        )
    {
        _shieldModule = shieldModule;
        _movementController.SetModule(flightModule);
        _weaponContorller.SetModule(weaponModuleL, weaponModuleR);

        _shield = _shieldModule.StartShield;

        _turnStateManager = CTurnStateManager.Instance;

        if (_turnStateManager == null)
        {
            Debug.LogWarning("Missing _turnStateManager");
        }

        UnitStyleInit();
    }

    public void CallAIInput(int turn)
    {
        _turnNum = turn;
        SetAIInput();
    }

    private void SetAIInput()
    {
        AISetSpeed();
        AIUnitMovement();
        AIShieldRegen();
        _turnStateManager.SetEnemyReadyCount();
    }


    private void AISetSpeed()
    {
        int speadLevel = Random.Range(0, 4);
        
        switch (speadLevel)
        {
            case 0:
                _movementController.Speed = MovementController.FlightModule.StartSpeed - MovementController.FlightModule.Deceleration;
                break;
            case 1:
                _movementController.Speed = MovementController.FlightModule.StartSpeed;
                break;
            case 2:
                _movementController.Speed = MovementController.FlightModule.StartSpeed + MovementController.FlightModule.Acceleration;
                break;
        }
    }

    private void AIUnitMovement()
    {
        if (_targetUnit == null)
        {
            return;
        }

        float speed = _movementController.Speed;
        float turnRate = _movementController.FlightModule.TurnRate;

        Vector3 dest = _targetUnit.transform.position;

        dest.y = 0;

        Vector3[] posPath = new Vector3[5];

        posPath[0] = transform.position;

        bool pathReachedDest = false;

        Quaternion rot = transform.rotation;

        for (int i = 1; i < posPath.Length; i++)
        {
            Vector3 moveVector = posPath[i - 1];


            if (pathReachedDest == false)
            {
                for (int j = 0; j < 5; j++)
                {
                    Quaternion tempRot = rot;

                    tempRot = Quaternion.LookRotation(dest - moveVector, Vector3.up);

                    rot = Quaternion.RotateTowards(rot, tempRot, turnRate * 0.2f);

                    moveVector += rot * Vector3.forward * speed * 0.2f;

                    if ((dest - moveVector).sqrMagnitude <= speed * speed)
                    {
                        pathReachedDest = true;
                    }
                }

            }

            else
            {
                moveVector += rot * Vector3.forward * speed;
            }


            posPath[i] = moveVector;

            Debug.DrawRay(posPath[i - 1], posPath[i] - posPath[i - 1], pathReachedDest ? Color.yellow : Color.blue, 2f);

        }

        MovementController.SetTargetPos(dest, posPath[posPath.Length - 1]);

    }

    private void AIShieldRegen()
    {
        if(_shield == _shieldModule.MaxShield)
        {
            return;
        }

        int regenTurn = 0;

        switch (_shieldModule.ShieldRegenCost)
        {
            case float n when n >= 8:
                regenTurn = 4;
                break;
            case float n when n >= 6:
                regenTurn = 3;
                break;
            case float n when n >= 4:
                regenTurn = 2;
                break;
            default:
                regenTurn = 1;
                break;
        }


        if(_turnNum - _lastCombatTurn <= regenTurn)
        {
            return;
        }

        _shield += _shieldModule.ShieldRegen;

        _lastCombatTurn = _turnNum;

        if (_shield > _shieldModule.MaxShield)
        {
            _shield = _shieldModule.MaxShield;
        }

    }

    private void SetShieldBar()
    {
        if (Mathf.Abs(_shieldBar.fillAmount - (_shield / _shieldModule.MaxShield)) < 0.1F)
        {
            _shieldBar.fillAmount = _shield / _shieldModule.MaxShield;
            return;
        }

        float lerp = Mathf.Lerp(_shieldBar.fillAmount, _shield / _shieldModule.MaxShield, 0.05f);
        _shieldBar.fillAmount = lerp;
    }

    public void TakeHit(float damage)
    {
        _shield -= damage;

        _lastCombatTurn = _turnNum;

        if (_shield <= 0)
        {
            _knockout.enabled = true;
            TryGetComponent<Collider>(out Collider collider);
            collider.enabled = false;
            _turnStateManager.UnitGetKnockedOut(this);

            if (_movementController.Speed > 8)
            {
                _movementController.Speed = 8;
            }

            return;
        }
        
        _shieldEffect.Play();
    }

    public void SetLastCombatTurn()
    {
        _lastCombatTurn = _turnNum;
    }

}
