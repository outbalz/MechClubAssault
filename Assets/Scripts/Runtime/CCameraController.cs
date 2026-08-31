using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraController : MonoBehaviour
{
    #region inspector
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _camTr;
    [SerializeField] private Transform _target;

    [SerializeField] private Vector3 _camOffset = new Vector3(0f, 2f, -3f);
    //[SerializeField] private Vector3 _camLookOffset = Vector3.zero;
    [SerializeField] private Vector3 _camTargetOffset = Vector3.zero;
    [SerializeField] private float _camLookAtHeight = 1.5f;

    [SerializeField] private float _offesetZmin = -20f;
    [SerializeField] private float _offesetZmax = -2.7f;

    [Min(0f)]
    [SerializeField] private float _sharpness = 18f;

    [SerializeField] private float _sensitivity = 1.0f;
    #endregion

    #region private var
    private Vector3 _camOffsetDefault;
    private Quaternion _rotOffset;
    private bool _freeCamMod = false;
    private bool _isOnTarget = false;
    #endregion

    private void Awake()
    {
        _camera = Camera.main;
        _camOffsetDefault = _camOffset;
    }


    void Start()
    {
        if (_camera == null)
        {
            Debug.LogWarning("카메라 누락");
            this.enabled = false;
            return;
        }

        if (_target == null)
        {
            Debug.LogWarning("타겟 누락");
            this.enabled = false;
            return;
        }

        _camTr = _camera.transform;

        _rotOffset = Quaternion.identity;

        Vector3 desiredPos;
        Quaternion desiredRot;

        SetCameraPose(out desiredPos, out desiredRot);

        ApplyCameraPose(desiredPos, desiredRot);
        _isOnTarget = true;
    }

    private void Update()
    {
        CameraOffsetUpdate();
    }

    private void LateUpdate()
    {
        CameraUpdate();
    }


    private void ApplyCameraPose(Vector3 desiredPos, Quaternion desiredRot)
    {
        _camTr.position = desiredPos;
        _camTr.rotation = desiredRot;
    }

    
    private float GetSmmothT(float sharpness)
    {
        return 1f - Mathf.Exp(-sharpness * Time.deltaTime);
    }
    
    private void ApplyCameraPose(Vector3 desiredPos, Quaternion desiredRot, float sharpness)
    {

        float t = GetSmmothT(sharpness);

        _camTr.position = Vector3.Lerp(_camTr.position, desiredPos, t);

        _camTr.rotation = Quaternion.Slerp(_camTr.rotation, desiredRot, t);

    }
    /*/**/

    private void CameraUpdate()
    {
        Vector3 desiredPos;
        Quaternion desiredRot;

        SetCameraPose(out desiredPos, out desiredRot);

        if (_isOnTarget)
        {
            ApplyCameraPose(desiredPos, desiredRot);
        }

        else
        {
            ApplyCameraPose(desiredPos, desiredRot, _sharpness);
            if ((desiredPos - _camTr.position).sqrMagnitude <= 1)
            {
                _isOnTarget = true;
            }
        }
    }

    private void SetCameraPose(out Vector3 desiredPos, out Quaternion desiredRot)
    {

        float camPitch = 2f;

        Vector3 lookPos;
        if (_freeCamMod)
        {
            desiredPos = _target.position + _camTargetOffset;
            lookPos = _target.position + _camTargetOffset + _rotOffset * Vector3.forward * _camOffset.z + Vector3.up * _camLookAtHeight;

            desiredPos.y = _target.position.y + camPitch;
            desiredRot =  Quaternion.LookRotation(lookPos - desiredPos, Vector3.up);
            return;
        }

        desiredPos = _target.position + (_rotOffset * _target.rotation * _camOffset);
        lookPos = _target.position + Vector3.up * _camLookAtHeight;

        desiredPos.y = _target.position.y + camPitch;

        desiredRot =  Quaternion.LookRotation(lookPos - desiredPos, Vector3.up);


    }

    private void CameraOffsetUpdate()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput != 0f)
        {
            if (_freeCamMod == false)
            {
                _camOffset.z += wheelInput * _sensitivity;
                _camOffset.z = Mathf.Clamp(_camOffset.z, _offesetZmin, _offesetZmax);
            }

            else
            {
                Vector3 dir = _rotOffset * Vector3.forward * wheelInput;

                _camTargetOffset -= dir * 50 * _sensitivity * Time.deltaTime;
            }
        }


        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");

            _rotOffset *= Quaternion.AngleAxis(mouseX * _sensitivity, Vector3.up);
        }

        if (Input.GetMouseButton(2))
        {
            if(_freeCamMod == false)
            {
                _freeCamMod = true;
                _rotOffset *= _target.rotation * Quaternion.Euler(0, 180, 0);
                _camTargetOffset = _rotOffset * -_camOffset;
                _camTargetOffset.y = _target.position.y;
            }

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            if(mouseX != 0f || mouseY != 0f)
            {
                Vector3 dir = _rotOffset * Vector3.forward * mouseY + _rotOffset * Vector3.right * mouseX;

                _camTargetOffset += dir * 1 * _sensitivity * Time.deltaTime;
            }

        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (v != 0f)
        {
            if(_freeCamMod == false)
            {
                _freeCamMod = true;
                _rotOffset *= _target.rotation * Quaternion.Euler(0, 180, 0);
                _camTargetOffset = _rotOffset *-_camOffset;
                _camTargetOffset.y = _target.position.y;
            }

            Vector3 dir = _rotOffset * Vector3.forward * v;

            _camTargetOffset -= dir * 5 * _sensitivity * Time.deltaTime;
        }

        if (h != 0f)
        {
            if (_freeCamMod == false)
            {
                _freeCamMod = true;
                _rotOffset *= _target.rotation * Quaternion.Euler(0, 180, 0);
                _camTargetOffset = _rotOffset * -_camOffset;
                _camTargetOffset.y = _target.position.y;
            }

            Vector3 dir = _rotOffset * Vector3.right * h;

            _camTargetOffset -= dir * 5 * _sensitivity * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            _freeCamMod = false;
            _rotOffset = Quaternion.identity;
            _camOffset = _camOffsetDefault;
            //_camLookOffset = Vector3.zero;
            _camTargetOffset = Vector3.zero;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _isOnTarget = false;
    }
}
