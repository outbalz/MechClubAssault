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
    [SerializeField] private float _camLookAtHeight = 1.5f;

    [SerializeField] private float _offesetZmin = -20f;
    [SerializeField] private float _offesetZmax = -2.7f;

    [Min(0f)]
    [SerializeField] private float _sharpness = 18f;

    [SerializeField] private float _sensitivity = 1.0f;
    #endregion

    #region private var
    private Quaternion _rotOffset;
    #endregion

    private void Awake()
    {
        _camera = Camera.main;
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

        BuildThirdPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _sharpness, true);
    }

    private void Update()
    {
        CameraOffsetUpdate();
    }

    private void LateUpdate()
    {
        CameraUpdate();
    }

    private float GetSmmothT(float sharpness)
    {
        return 1f - Mathf.Exp(-sharpness * Time.deltaTime);
    }

    private void ApplyPose(Vector3 desiredPos, Quaternion desiredRot, float sharpness, bool snap)
    {

        if (snap)
        {
            _camTr.position = desiredPos;
            _camTr.rotation = desiredRot;

            return;
        }


        float t = GetSmmothT(sharpness);

        _camTr.position = Vector3.Lerp(_camTr.position, desiredPos, t);

        _camTr.rotation = Quaternion.Slerp(_camTr.rotation, desiredRot, t);

    }


    private void CameraUpdate()
    {
        Vector3 desiredPos;
        Quaternion desiredRot;

        BuildThirdPose(out desiredPos, out desiredRot);

        ApplyPose(desiredPos, desiredRot, _sharpness, true);
    }

    private void BuildThirdPose(out Vector3 desiredPos, out Quaternion desiredRot)
    {

        float camPitch = 2f;

        desiredPos = _target.position + ( _rotOffset * _target.rotation * _camOffset);

        desiredPos.y = _target.position.y + camPitch;

        Vector3 lookPos = _target.position + Vector3.up * _camLookAtHeight;
        desiredRot =  Quaternion.LookRotation(lookPos - desiredPos, Vector3.up);


    }

    private void CameraOffsetUpdate()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput != 0f)
        {
            _camOffset.z += wheelInput * _sensitivity;
            _camOffset.z = Mathf.Clamp(_camOffset.z, _offesetZmin, _offesetZmax);
        }


        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");

            _rotOffset *= Quaternion.AngleAxis(mouseX * _sensitivity, Vector3.up);
        }
    }

}
