using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CTurnStateManager : MonoBehaviour
{
    public enum ETurnState
    {
        TurnInit,
        AwaitPlayerInput,
        AIInput,
        TurnResolve
    }


    #region inspector
    [Header("Unit")]
    [SerializeField] private List<CUnitController> _playerUnits;

    [Space]
    [Header("UI")]
    [SerializeField] private CBattleUIManager _battleUI;
    #endregion

    #region private var
    private int _turnNum;
    private ETurnState _turnState;

    private float _turnTimer;
    private int _turnSec;
    #endregion

    #region getter
    public ETurnState TurnState { get { return _turnState; } }
    #endregion

    private void Awake()
    {
        _turnNum = 0;
        
        if(_battleUI == null)
        {
            if(TryGetComponent<CBattleUIManager>(out _battleUI) == false)
            {
                Debug.LogWarning("Missing CBattleUIManager");
            }
        }
    }

    private void Start()
    {
        ChangeTurnState(ETurnState.TurnInit);
    }

    private void ChangeTurnState(ETurnState turnState)
    {
        _turnState = turnState;
        _battleUI.SetPlayerTurnUI(false);

        switch (_turnState)
        {
            case ETurnState.TurnInit:
                TurnInit();
                break;
            case ETurnState.AwaitPlayerInput:
                _battleUI.SetPlayerTurnUI(true);
                break;
            case ETurnState.AIInput:
                AIInput();
                break;
            case ETurnState.TurnResolve:
                TurnResolveInit();
                break;
            default:
                Debug.LogWarning("_turnState Err");
                break;
        }
    }

    private void TurnInit()
    {
        _turnNum++;

        for (int i = 0; i < _playerUnits.Count; i++)
        {
            Vector3[] tunPosData = _playerUnits[i].TurnData.Positions;

            for (int j = 0; j < tunPosData.Length ; j++)
            {
                tunPosData[j] = _playerUnits[i].transform.position + _playerUnits[i].transform.rotation * Vector3.forward * _playerUnits[i].MovementController.Speed * j;
            }

            _playerUnits[i].TurnInit();

            _playerUnits[i].MovementController.SetTargetPos(tunPosData[tunPosData.Length-1], tunPosData[tunPosData.Length - 1]);
            _playerUnits[i].MovementController.SetOnMove(false);
            _playerUnits[i].MovementController.SpeedTurnInit();
            //_playerUnits[i].GetSpeed();
        }

        ChangeTurnState(ETurnState.AwaitPlayerInput);

        _battleUI.SetAccelerationLevel(true);
    }

    private void AIInput()
    {
        ChangeTurnState(ETurnState.TurnResolve);
    }

    private void TurnResolveInit()
    {
        _turnTimer = 0;
        _turnSec = 0;


        for (int i = 0; i < _playerUnits.Count; i++)
        {
            _playerUnits[i].MovementController.SetOnMove();
        }


        //TurnResolveUpDatePerSec();
    }

    private void TurnResolveUpDate()
    {
        if (_turnTimer <= 0)
        {
            _turnTimer += 1;
            _turnSec++;

            if (_turnSec >= 5)
            {
                ChangeTurnState(ETurnState.TurnInit);
                return;
            }

            //TurnResolveUpDatePerSec();
        }

        _turnTimer -= Time.deltaTime;

    }

    /*
    private void TurnResolveUpDatePerSec()
    {
        for (int i = 0; i < _playerUnits.Count; i++)
        {
            _playerUnits[i].MovementController.SetTargetPos(_playerUnits[i].TurnData.Positions[_turnSec], _playerUnits[i].TurnData.Positions[_playerUnits[i].TurnData.Positions.Length -1]);
        }
    }
    */
    public void SubmitTurn()
    {
        if(_turnState != ETurnState.AwaitPlayerInput)
        {
            return;
        }

        _turnState = ETurnState.AIInput;
        ChangeTurnState(ETurnState.AIInput);
    }


    private void Update()
    {
        if(_turnState == ETurnState.TurnResolve)
        {
            TurnResolveUpDate();
        }
    }


}
