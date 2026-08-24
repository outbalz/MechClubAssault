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
    [SerializeField] private GameObject _playerTurnUI;

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

        if (_playerTurnUI == null)
        {
            Debug.LogWarning("Missing _playerTurnUI");
        }
    }

    private void Start()
    {
        ChangeTurnState(ETurnState.TurnInit);
    }

    private void ChangeTurnState(ETurnState turnState)
    {
        _turnState = turnState;
        _playerTurnUI.SetActive(false);

        switch (_turnState)
        {
            case ETurnState.TurnInit:
                TurnInit();
                break;
            case ETurnState.AwaitPlayerInput:
                _playerTurnUI.SetActive(true);
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
            for (int j = 0; j < _playerUnits[i].TurnData.Positions.Length ; j++)
            {
                _playerUnits[i].TurnData.Positions[j] = _playerUnits[i].transform.position + _playerUnits[i].transform.rotation * Vector3.forward * _playerUnits[i].Speed * j;
            }
        }

        ChangeTurnState(ETurnState.AwaitPlayerInput);
    }

    private void AIInput()
    {
        ChangeTurnState(ETurnState.TurnResolve);
    }

    private void TurnResolveInit()
    {
        _turnTimer = 1;
        _turnSec = 0;
        TurnResolveUpDatePerSec();
    }

    private void TurnResolveUpDate()
    {
        if (_turnTimer <= 0)
        {
            _turnTimer += 1;
            _turnSec++;

            if (_turnSec >= 10)
            {
                ChangeTurnState(ETurnState.TurnInit);
                return;
            }

            TurnResolveUpDatePerSec();
        }

        _turnTimer -= Time.deltaTime;

    }


    private void TurnResolveUpDatePerSec()
    {
        for (int i = 0; i < _playerUnits.Count; i++)
        {
            _playerUnits[i].MovementController.SetTargetPos(_playerUnits[i].TurnData.Dest, _playerUnits[i].TurnData.DestReachSec);
        }
    }

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
