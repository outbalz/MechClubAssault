using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CUnitMovementController))]
public class CEnemyUnitContorller : MonoBehaviour, IDamageable, ICombatTracker
{
    #region inspector
    [Space]
    [Header("Shield")]
    [SerializeField] private ScriptableObjectShieldModule _shieldModule;

    [Space]
    [Header("Movement")]
    [SerializeField] private CUnitMovementController _movementController;

    [Space]
    [Header("UI")]
    [SerializeField] private Transform _unitUi;
    [SerializeField] private Image _shieldBar;

    [Space]
    [Header("carmera")]
    [SerializeField] private Transform _cameraTr;

    [Space]
    [Header("Manager")]
    [SerializeField] private CTurnStateManager _turnStateManager;
    #endregion

    #region Debug
    [Header("Debug")]
    [SerializeField] private float _shield;
    #endregion

    #region private var
    private CTurnData _turnData;
    private int _turnNum = 0;
    private int _lastCombatTurn = 0;
    private CUnitController _targetUnit;
    #endregion

    #region getter
    public CUnitMovementController MovementController { get { return _movementController; } }
    public CTurnData TurnData { get { return _turnData; } }
    public CUnitController TargetUnit { /*get { return _targetUnit; }*/ set { _targetUnit = value; } }
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

        if (_turnData == null)
        {
            _turnData = new CTurnData(_turnNum);
        }

        if(_unitUi == null || _shieldBar == null)
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
            SetShieldBar();
        }

        if(_turnStateManager == null)
        {
            Debug.LogWarning("Missing _turnStateManager");
        }

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
                _movementController.Speed = MovementController.FlightModule._startSpeed - MovementController.FlightModule._deceleration;
                break;
            case 1:
                _movementController.Speed = MovementController.FlightModule._startSpeed;
                break;
            case 2:
                _movementController.Speed = MovementController.FlightModule._startSpeed + MovementController.FlightModule._acceleration;
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
        float turnRate = _movementController.FlightModule._turnRate;

        Vector3 dest = _targetUnit.transform.position;

        dest.y = 0;

        Vector3[] posPath = new Vector3[5];
        //List<Vector3> linePos = new List<Vector3>();

        posPath[0] = transform.position;
        //linePos.Add(transform.position);

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

                    //linePos.Add(moveVector);

                    if ((dest - moveVector).sqrMagnitude <= speed * speed)
                    {
                        pathReachedDest = true;
                    }
                }

                /*
                    Quaternion tempRot = rot;

                    tempRot = Quaternion.LookRotation(dest - moveVector, Vector3.up);

                    rot = Quaternion.RotateTowards(rot, tempRot, this.TurnRate * 0.1f);

                    moveVector += rot * Vector3.forward * this.Speed;
                */

            }

            else
            {
                moveVector += rot * Vector3.forward * speed;
                //linePos.Add(moveVector);
            }


            posPath[i] = moveVector;

            //posPath[i].y = _MAPHIGHT;

            Debug.DrawRay(posPath[i - 1], posPath[i] - posPath[i - 1], pathReachedDest ? Color.yellow : Color.blue, 2f);

        }

        TurnData.Positions = posPath;

        MovementController.SetTargetPos(dest, posPath[posPath.Length - 1]);


        //Debug.Log(pathReachedDest);

        /*
        // for test-----------
        _selectedUnit.MovementController.SetOnMove(true);
        //----------------------
        */
    }

    private void AIShieldRegen()
    {
        if(_shield == _shieldModule._maxShield)
        {
            return;
        }

        int regenTurn = 0;

        switch (_shieldModule._shieldRegenCost)
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


        if(_turnNum - _lastCombatTurn < regenTurn)
        {
            return;
        }

        _shield += _shieldModule._shieldRegen;

        _lastCombatTurn = _turnNum;

        if (_shield > _shieldModule._maxShield)
        {
            _shield = _shieldModule._maxShield;
        }

        SetShieldBar();

    }


    private void SetShieldBar()
    {
        _shieldBar.fillAmount = _shield / _shieldModule._maxShield;
    }

    public void TakeHit(float damage)
    {
        _shield -= damage;

        SetShieldBar();

        _lastCombatTurn = _turnNum;
    }

    public void SetLastCombatTurn()
    {
        _lastCombatTurn = _turnNum;
    }

}
