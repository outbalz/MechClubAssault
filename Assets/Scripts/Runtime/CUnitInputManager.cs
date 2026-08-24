using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CUnitInputManager : MonoBehaviour
{
    #region inspector
    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Space]
    [Header("Ray")]
    [SerializeField] private float _rayMaxDistance;
    [SerializeField] private LayerMask _rayLayerMask;

    [Space]
    [Header("Turn State Manager")]
    [SerializeField] CTurnStateManager _turnStateManager;
    #endregion

    #region inspector (debug)
    [Space]
    [Header("Debug")]
    [SerializeField] private CUnitController _selectedUnit;
    [SerializeField] private GameObject _targetUnit;
    [SerializeField] private Transform _posMarcker;
    #endregion

    #region private var
    private const float _MAPHIGHT = 0;
    private Ray _previousRay;
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

        if(_turnStateManager == null)
        {
            if(TryGetComponent<CTurnStateManager>(out _turnStateManager) == false)
            {
                Debug.LogWarning("Missing CTurnStateManager");
            }
        }
    }



    void Update()
    {
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            SetUnitMovementPath();
        }
    }


    private void SetUnitMovementPath()
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
                UnitMovementPathToRayHit(hit);
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

        if(_selectedUnit.MovementController == null)
        {
            Debug.LogWarning("Missing MovementController");
            return;
        }

        Vector3 dest = hit.point;

        dest.y = _MAPHIGHT;

        Vector3[] posPath = new Vector3[10];
        List<Vector3> linePos = new List<Vector3>();

        posPath[0] = _selectedUnit.transform.position;
        
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

                    rot = Quaternion.RotateTowards(rot, tempRot, _selectedUnit.TurnRate * 0.2f);

                    moveVector += rot * Vector3.forward * _selectedUnit.Speed * 0.2f;

                    linePos.Add(moveVector);
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
                moveVector += rot * Vector3.forward * _selectedUnit.Speed;
                linePos.Add(moveVector);
            }


            posPath[i] =  moveVector;

            if ((dest - posPath[i]).sqrMagnitude <= _selectedUnit.Speed * _selectedUnit.Speed)
            {
                pathReachedDest = true;
            }
            //posPath[i].y = _MAPHIGHT;

            Debug.DrawRay(posPath[i - 1], posPath[i] - posPath[i - 1], pathReachedDest ? Color.yellow : Color.blue , 2f);

        }

        _selectedUnit.TurnData.Positions = posPath;
        _selectedUnit.VisualizePath(linePos);

        _selectedUnit.MovementController.SetTargetPos(dest, posPath[posPath.Length-1]);


        _posMarcker.position = dest;

        //Debug.Log(pathReachedDest);

        /*
        // for test-----------
        _selectedUnit.MovementController.SetOnMove(true);
        //----------------------
        */
    }



    public void SetSelectedUint(CUnitController unit)
    {
        _selectedUnit = unit;
    }

}
