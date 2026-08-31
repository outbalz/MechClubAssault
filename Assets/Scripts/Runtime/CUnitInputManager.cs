using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CUnitInputManager : MonoBehaviour
{
    #region inspector
    [Header("Camera")]
    [SerializeField] private Camera _camera;
    [SerializeField] private CCameraController _cameraController;

    [Space]
    [Header("Ray")]
    [SerializeField] private float _rayMaxDistance;
    [SerializeField] private LayerMask _rayLayerMask;

    [Space]
    [Header("Manager")]
    [SerializeField] private CTurnStateManager _turnStateManager;
    [SerializeField] private CBattleUIManager _battleUIManager;
    #endregion

    #region inspector (debug)
    [Space]
    [Header("Debug")]
    [SerializeField] private CUnitController _selectedUnit;
    [SerializeField] private GameObject _targetUnit;
    [SerializeField] private Transform _posMarker;
    #endregion

    #region private var
    private const float _MAPHIGHT = 0;
    private Ray _previousRay;

    private const string _playerUnitTag = "PlayerUnit";
    private const string _rayCatcherTag = "RayCatcher";
    #endregion

    #region getter
    public CUnitController SelectedUnit { get { return _selectedUnit; } }
    #endregion


    private void Reset()
    {
        Initialize();
    }


    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        if(_cameraController == null)
        {
            if(TryGetComponent<CCameraController>(out _cameraController) == false)
            {
                Debug.LogWarning("Missing _cameraController");
            }
        }

        if(_turnStateManager == null)
        {
            if(TryGetComponent<CTurnStateManager>(out _turnStateManager) == false)
            {
                Debug.LogWarning("Missing CTurnStateManager");
            }
        }

        if(_battleUIManager == null)
        {
            if(TryGetComponent<CBattleUIManager>(out _battleUIManager) == false)
            {
                Debug.LogWarning("Missing CBattleUIManager");
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CUnitController nextUnit = _turnStateManager.GetNextUnit(_selectedUnit);

            if (nextUnit != null)
            {
                SetSelectedUint(nextUnit);
            }
        }
    }


    private void LateUpdate()
    {

        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }

        if (Input.GetMouseButton(0))
        {
            OnClickPress();
        }
    }


    private void OnClick()
    {

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (ray.direction != _previousRay.direction)
        {
            OnClickRay(ray, out RaycastHit hit, out bool isHit);
            if (isHit)
            {
                GameObject go = hit.collider.gameObject;

                if (go.CompareTag(_playerUnitTag))
                {
                    SetSelectedUint(go.GetComponent<CUnitController>());
                }

                if (_turnStateManager.TurnState != CTurnStateManager.ETurnState.AwaitPlayerInput)
                {
                    return;
                }

                else if (go.CompareTag(_rayCatcherTag))
                {
                    UnitMovementPathToRayHit(hit);
                }
            }
        }
    }


    private void OnClickPress()
    {

        if(_turnStateManager.TurnState != CTurnStateManager.ETurnState.AwaitPlayerInput)
        {
            return;
        }


        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (ray.direction != _previousRay.direction)
        {
            OnClickRay(ray, out RaycastHit hit, out bool isHit);
            if (isHit)
            {
                GameObject go = hit.collider.gameObject;

                if (go.CompareTag(_rayCatcherTag))
                {
                    UnitMovementPathToRayHit(hit);
                }
            }
        }
    }


    private void OnClickRay(Ray ray,out RaycastHit hit, out bool isHit)
    {
        isHit = false;

        if (Physics.Raycast(ray, out hit, _rayMaxDistance, _rayLayerMask))
        {
            isHit = true;
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 2f);
        }

        else
        {
            Debug.DrawRay(ray.origin, ray.direction * _rayMaxDistance, Color.red, 2f);
        }

        _previousRay = ray;
    }

    private void UnitMovementPathToRayHit(RaycastHit hit)
    {
        if(_selectedUnit == null)
        {
            return;
        }

        float speed = _selectedUnit.MovementController.Speed;
        float turnRate = _selectedUnit.MovementController.FlightModule.TurnRate;

        Vector3 dest = hit.point;

        dest.y = _MAPHIGHT;

        Vector3[] posPath = new Vector3[5];
        List<Vector3> linePos = new List<Vector3>();

        posPath[0] = _selectedUnit.transform.position;
        linePos.Add(_selectedUnit.transform.position);

        bool pathReachedDest = false;

        Quaternion rot = _selectedUnit.transform.rotation;

        for (int i = 1; i < posPath.Length; i++)
        {
            Vector3 moveVector = posPath[i - 1];


            if(pathReachedDest == false)
            {
                for (int j = 0; j < 5; j++)
                {
                    Quaternion tempRot = rot;

                    tempRot = Quaternion.LookRotation(dest - moveVector, Vector3.up);

                    rot = Quaternion.RotateTowards(rot, tempRot, turnRate * 0.2f);

                    moveVector += rot * Vector3.forward * speed * 0.2f;

                    linePos.Add(moveVector);

                    if ((dest - moveVector).sqrMagnitude <= speed * speed)
                    {
                        pathReachedDest = true;
                    }
                }

                /*
                    Quaternion tempRot = rot;

                    tempRot = Quaternion.LookRotation(dest - moveVector, Vector3.up);

                    rot = Quaternion.RotateTowards(rot, tempRot, _selectedUnit.TurnRate * 0.1f);

                    moveVector += rot * Vector3.forward * _selectedUnit.Speed;
                */

            }

            else
            {
                moveVector += rot * Vector3.forward * speed;
                linePos.Add(moveVector);
            }


            posPath[i] =  moveVector;

            //posPath[i].y = _MAPHIGHT;

            Debug.DrawRay(posPath[i - 1], posPath[i] - posPath[i - 1], pathReachedDest ? Color.yellow : Color.blue , 2f);

        }

        _selectedUnit.TurnData.Positions = posPath;
        _selectedUnit.VisualizePath(linePos);

        _selectedUnit.MovementController.SetTargetPos(dest, posPath[posPath.Length-1]);


        _posMarker.position = dest;

        //Debug.Log(pathReachedDest);

        /*
        // for test-----------
        _selectedUnit.MovementController.SetOnMove(true);
        //----------------------
        */
    }



    public void SetSelectedUint(CUnitController unit)
    {
        if(unit == null)
        {
            Debug.LogWarning("unit null Err");
            return;
        }

        if(_selectedUnit != null)
        {
            _selectedUnit.UnitUI.localScale = new Vector3(0.1f, 0.1f, 1f);
        }

        _selectedUnit = unit;
        _selectedUnit.UnitUI.localScale = new Vector3(0.05f, 0.05f, 1f);

        _battleUIManager.SetValToSelectedUnitVal(unit);

        _cameraController.SetTarget(unit.transform);
    }

}
