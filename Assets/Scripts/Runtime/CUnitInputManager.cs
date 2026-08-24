using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitInputManager : MonoBehaviour
{
    #region inspector
    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Space]
    [Header("Ray")]
    [SerializeField] private float _rayMaxDistance;
    [SerializeField] private LayerMask _rayLayerMask;
    #endregion

    #region inspector (debug)
    [Space]
    [Header("Debug")]
    [SerializeField] private CUnitController _selectedUnit;
    [SerializeField] private GameObject _targetUnit;
    #endregion

    #region private var
    private const float _MAPHIGHT = 0;
    #endregion


    private void Reset()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }


    private void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClickRay(out RaycastHit hit, out bool isHit);
            if (isHit)
            {
                UnitMoveToRayHit(hit);
            }
        }
    }

    private void OnClickRay(out RaycastHit hit, out bool isHit)
    {
        isHit = false;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, _rayMaxDistance, _rayLayerMask))
        {
            isHit = true;
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 2f);
        }

        else
        {
            Debug.DrawRay(ray.origin, ray.direction * _rayMaxDistance, Color.red, 2f);
        }
    }

    private void UnitMoveToRayHit(RaycastHit hit)
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

        Vector3 pos = new Vector3(hit.point.x, _MAPHIGHT, hit.point.z);


        Vector3[] posPath = new Vector3[10];


        /*
        // for test-----------
        _selectedUnit.MovementController.SetTargetPos(pos);
        _selectedUnit.MovementController.SetOnMove(true);
        //----------------------
        */
    }



    public void SetSelectedUint(CUnitController unit)
    {
        _selectedUnit = unit;
    }

}
