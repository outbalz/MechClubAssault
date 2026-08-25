using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitWeaponContorller : MonoBehaviour
{
    #region inspector
    [Header("weaponL")]
    [SerializeField] private LineRenderer _weaponLInnerArc;
    [SerializeField] private LineRenderer _weaponLOutterArc;
    [SerializeField] private float _weaponLInnerArcDeg;
    [SerializeField] private float _weaponLOutterArcDeg;
    [SerializeField] private float _weaponRangeL;
    [SerializeField] private bool _weaponEnableL;
    [SerializeField] private float _weaponCoolDownL;
    [SerializeField] private float _weaponDamegeL;

    [SerializeField] private ParticleSystem _weaponParticleL;


    [Header("weaponR")]
    [SerializeField] private LineRenderer _weaponRInnerArc;
    [SerializeField] private LineRenderer _weaponROutterArc;
    [SerializeField] private float _weaponRInnerArcDeg;
    [SerializeField] private float _weaponROutterArcDeg;
    [SerializeField] private float _weaponRangeR;
    [SerializeField] private bool _weaponEnableR;
    [SerializeField] private float _weaponCoolDownR;
    [SerializeField] private float _weaponDamegeR;

    [SerializeField] private ParticleSystem _weaponParticleR;

    [Space]
    [Header("Turn State Manager")]
    [SerializeField] CTurnStateManager _turnStateManager;


    [Space]
    [Header("Ray")]
    [SerializeField] private LayerMask _weaponRayLayerMask;
    #endregion

    #region private var
    private float _weaponTimerL;
    private float _weaponTimerR;
    #endregion

    private void Awake()
    {
        if(_weaponLInnerArc == null || _weaponLOutterArc == null)
        {
            Debug.LogWarning("Missing Left weaponArc");
            _weaponEnableL = false;
        }

        else
        {
            _weaponEnableL = true;
        }

        if(_weaponRInnerArc == null || _weaponROutterArc == null)
        {
            Debug.LogWarning("Missing Right weaponArc");
            _weaponEnableR = false;
        }

        else
        {
            _weaponEnableR = true;
        }
    }


    void Start()
    {
        WeaponInit();
    }


    void Update()
    {
        WeaponUpdate();
    }

    private void WeaponInit()
    {
        if(_weaponEnableL == false && _weaponEnableR == false)
        {
            return;
        }

        if (_weaponEnableL)
        {
            _weaponLInnerArc.positionCount = 2;
            _weaponLInnerArc.SetPosition(1, Vector3.forward * _weaponRangeL);
            _weaponLInnerArc.transform.Rotate(Vector3.up, _weaponLInnerArcDeg);

            _weaponLOutterArc.positionCount = 2;
            _weaponLOutterArc.SetPosition(1, Vector3.forward * _weaponRangeL);
            _weaponLOutterArc.transform.Rotate(Vector3.up, -_weaponLOutterArcDeg);
        }

        if(_weaponEnableR)
        {
            _weaponRInnerArc.positionCount = 2;
            _weaponRInnerArc.SetPosition(1, Vector3.forward * _weaponRangeR);
            _weaponRInnerArc.transform.Rotate(Vector3.up, -_weaponRInnerArcDeg);

            _weaponROutterArc.positionCount = 2;
            _weaponROutterArc.SetPosition(1, Vector3.forward * _weaponRangeR);
            _weaponROutterArc.transform.Rotate(Vector3.up, _weaponROutterArcDeg);
        }
    }

    private void WeaponUpdate()
    {
        if(_turnStateManager.TurnState != CTurnStateManager.ETurnState.TurnResolve)
        {
            return;
        }

        if (_weaponEnableL == false && _weaponEnableR == false)
        {
            return;
        }

        if (_weaponEnableL)
        {
            if (_weaponTimerL <= 0)
            {
                CastWeaponRay
                    (
                    _weaponLInnerArc.transform.position,
                    -_weaponLOutterArcDeg,
                    _weaponLInnerArcDeg,
                    _weaponRangeL,
                    out RaycastHit hit,
                    out bool isHit
                    );

                if (isHit)
                {
                    _weaponParticleL.Play();
                    _weaponTimerL = _weaponCoolDownL;
                    hit.collider.GetComponent<IDamageable>().TakeHit(_weaponDamegeL);
                }
            }

            else
            {
                _weaponTimerL -= Time.deltaTime;
            }

        }

        if (_weaponEnableR)
        {
            if (_weaponTimerR <= 0)
            {
                CastWeaponRay
                    (
                    _weaponRInnerArc.transform.position,
                    -_weaponRInnerArcDeg,
                    _weaponROutterArcDeg,
                    _weaponRangeR,
                    out RaycastHit hit,
                    out bool isHit
                    );

                if (isHit)
                {
                    _weaponParticleR.Play();
                    _weaponTimerR = _weaponCoolDownR;
                    hit.collider.GetComponent<IDamageable>().TakeHit(_weaponDamegeR);
                }
            }

            else
            {
                _weaponTimerR -= Time.deltaTime;
            }

        }

    }


    private void CastWeaponRay(Vector3 origin, float minArc, float maxArc, float range, out RaycastHit hit, out bool isHit)
    {
        origin.y = 0;
        isHit = false;

        Vector3 dir = transform.forward;
        Ray ray;

        for (float i = minArc; i < maxArc; i += 5)
        {
            dir = Quaternion.Euler(0,i,0) * transform.forward;

            ray = new Ray(origin, dir);

            if(Physics.Raycast(ray, out hit, range, _weaponRayLayerMask))
            {
                isHit = true;
                return;
            }

        }

        dir = Quaternion.Euler(0, maxArc, 0) * transform.forward;

        ray = new Ray(origin, dir);

        if (Physics.Raycast(ray, out hit, range, _weaponRayLayerMask))
        {
            isHit = true;
            return;
        }


    }


    /*
    public void WeaponTestFire()
    {
        _weaponParticleL.Play();
        _weaponParticleR.Play();
    }
    */

}
