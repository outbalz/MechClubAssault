using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitWeaponContorller : MonoBehaviour
{
    #region inspector
    [Header("weaponL")]
    [SerializeField] private ScriptableObjectWeaponModule _weaponL;

    [Space]
    [SerializeField] private LineRenderer _weaponLInnerArc;
    [SerializeField] private LineRenderer _weaponLOutterArc;

    /*
    [SerializeField] private float _weaponLInnerArcDeg;
    [SerializeField] private float _weaponLOutterArcDeg;
    [SerializeField] private float WeaponRangeL;
    [SerializeField] private float WeaponCoolDownL;
    [SerializeField] private float WeaponDamegeL;
    */

    [SerializeField] private ParticleSystem _weaponParticleL;
    [SerializeField] private bool _weaponEnableL;


    [Header("weaponR")]
    [SerializeField] private ScriptableObjectWeaponModule _weaponR;

    [Space]
    [SerializeField] private LineRenderer _weaponRInnerArc;
    [SerializeField] private LineRenderer _weaponROutterArc;

    /*
    [SerializeField] private float _weaponRInnerArcDeg;
    [SerializeField] private float _weaponROutterArcDeg;
    [SerializeField] private float WeaponRangeR;
    [SerializeField] private float WeaponCoolDownR;
    [SerializeField] private float WeaponDamegeR;
    */

    [SerializeField] private ParticleSystem _weaponParticleR;
    [SerializeField] private bool _weaponEnableR;

    [Space]
    [Header("Turn State Manager")]
    [SerializeField] CTurnStateManager _turnStateManager;


    [Space]
    [Header("Ray")]
    [SerializeField] private LayerMask _weaponRayLayerMask;
    [SerializeField] bool _visualizeRange = true;

    [Space]
    [Header("CombatTracker")]
    [SerializeField] private ICombatTracker _combatTracker;
    #endregion

    #region private var
    private float _weaponTimerL;
    private float _weaponTimerR;
    #endregion

    #region getter
    public bool WeaponEnableL { get { return _weaponEnableL; } set { _weaponEnableL = value; } }
    public bool WeaponEnableR { get { return _weaponEnableR; } set { _weaponEnableR = value; } }
    public ScriptableObjectWeaponModule WeaponL { get { return _weaponL; } }
    public ScriptableObjectWeaponModule WeaponR { get { return _weaponR; } }
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

        if(_combatTracker == null)
        {
            if(TryGetComponent<ICombatTracker>(out _combatTracker) == false)
            {
                Debug.LogWarning("Missing _combatTracker");
            }
        }
    }



    void Update()
    {
        WeaponUpdate();
    }
    
    public void SetModule(ScriptableObjectWeaponModule weaponModuleL, ScriptableObjectWeaponModule weaponModuleR)
    {
        _weaponL = weaponModuleL;
        _weaponR = weaponModuleR;
        WeaponInit();
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
            _weaponLInnerArc.SetPosition(1, Vector3.forward * _weaponL.WeaponRange);
            _weaponLInnerArc.transform.Rotate(Vector3.up, _weaponL.WeaponInnerArcDeg);

            _weaponLOutterArc.positionCount = 2;
            _weaponLOutterArc.SetPosition(1, Vector3.forward * _weaponL.WeaponRange);
            _weaponLOutterArc.transform.Rotate(Vector3.up, -_weaponL.WeaponOutterArcDeg);
        }

        if(_weaponEnableR)
        {
            _weaponRInnerArc.positionCount = 2;
            _weaponRInnerArc.SetPosition(1, Vector3.forward * _weaponR.WeaponRange);
            _weaponRInnerArc.transform.Rotate(Vector3.up, -_weaponR.WeaponInnerArcDeg);

            _weaponROutterArc.positionCount = 2;
            _weaponROutterArc.SetPosition(1, Vector3.forward * _weaponR.WeaponRange);
            _weaponROutterArc.transform.Rotate(Vector3.up, _weaponR.WeaponOutterArcDeg);
        }

        if(_visualizeRange == false)
        {
            _weaponLInnerArc.enabled = false;
            _weaponLOutterArc.enabled = false;
            _weaponRInnerArc.enabled = false;
            _weaponROutterArc.enabled = false;
        }

        _turnStateManager = CTurnStateManager.Instance;
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
                    -_weaponL.WeaponOutterArcDeg,
                    _weaponL.WeaponInnerArcDeg,
                    _weaponL.WeaponRange,
                    out RaycastHit hit,
                    out bool isHit
                    );

                if (isHit)
                {
                    _weaponParticleL.Play();
                    _weaponTimerL = _weaponL.WeaponCoolDown;
                    hit.collider.GetComponent<IDamageable>().TakeHit(_weaponL.WeaponDamege);
                    _combatTracker.SetLastCombatTurn();
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
                    -_weaponR.WeaponInnerArcDeg,
                    _weaponR.WeaponOutterArcDeg,
                    _weaponR.WeaponRange,
                    out RaycastHit hit,
                    out bool isHit
                    );

                if (isHit)
                {
                    _weaponParticleR.Play();
                    _weaponTimerR = _weaponR.WeaponCoolDown;
                    hit.collider.GetComponent<IDamageable>().TakeHit(_weaponR.WeaponDamege);
                    _combatTracker.SetLastCombatTurn();
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


    public void SetWeaponEnable(bool isLeft, bool enable)
    {
        if (isLeft)
        {
            _weaponEnableL = enable;

            _weaponLInnerArc.enabled = enable;
            _weaponLOutterArc.enabled = enable;
        }

        else
        {
            _weaponEnableR = enable;

            _weaponRInnerArc.enabled = enable;
            _weaponROutterArc.enabled = enable;
        }

        if (_visualizeRange == false)
        {
            _weaponLInnerArc.enabled = false;
            _weaponLOutterArc.enabled = false;
            _weaponRInnerArc.enabled = false;
            _weaponROutterArc.enabled = false;
        }
    }

}
