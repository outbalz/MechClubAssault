using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CClubMember
{
    private string _name;

    private ScriptableObjectGeneratorModule _generatorModule;
    private ScriptableObjectShieldModule _shieldModule;
    private ScriptableObjectFlightModule _flightModule;
    private ScriptableObjectWeaponModule _weaponModuleL;
    private ScriptableObjectWeaponModule _weaponModuleR;

    public string Name { get { return _name; } }

    public ScriptableObjectGeneratorModule GeneratorModule { get { return _generatorModule; } set { _generatorModule = value; } }
    public ScriptableObjectShieldModule ShieldModule { get { return _shieldModule; } set { _shieldModule = value; } }
    public ScriptableObjectFlightModule FlightModule { get { return _flightModule; } set { _flightModule = value; } }
    public ScriptableObjectWeaponModule WeaponModuleL { get { return _weaponModuleL; } set { _weaponModuleL = value; } }
    public ScriptableObjectWeaponModule WeaponModuleR { get { return _weaponModuleR; } set { _weaponModuleR = value; } }

    public CClubMember
        (
            string name,
            ScriptableObjectGeneratorModule generatorModule,
            ScriptableObjectShieldModule shieldModule,
            ScriptableObjectFlightModule flightModule,
            ScriptableObjectWeaponModule weaponModuleL,
            ScriptableObjectWeaponModule weaponModuleR/**/
        )
    {
        this._name = name;
        this._generatorModule = generatorModule;
        this._shieldModule = shieldModule;
        this._flightModule = flightModule;
        this._weaponModuleL = weaponModuleL;
        this._weaponModuleR = weaponModuleR;/**/
    }
}

public class CGameProgressManager : MonoBehaviour
{
    #region inspector
    [SerializeField] private ScriptableObjectDataBase _SODB;
    #endregion

    #region private var
    private List<CClubMember> _clubMembers = new List<CClubMember>();
    private static CGameProgressManager _instance;

    private float _fund = 0;
    private float _reputation = 0;
    #endregion

    #region getter
    public static CGameProgressManager Instance { get { return _instance; } }
    public List<CClubMember> ClubMembers { get { return _clubMembers; } }
    public float Fund { get { return _fund; } set { _fund = value; } }
    public float Reputation { get { return _reputation; } set { _reputation = value; } }
    #endregion


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }

    }

    public void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public void ResetGameProgress()
    {
        _clubMembers.Clear();
        _fund = 20;
        _reputation = 20;
        _clubMembers.Add(new CClubMember(CUtil.GetRandomName(), _SODB.GetGeneratorModule(0), _SODB.GetShieldModule(0), _SODB.GetFlightModule(0), _SODB.GetWeaponModule(0), _SODB.GetWeaponModule(0)));
        _clubMembers.Add(new CClubMember(CUtil.GetRandomName(), _SODB.GetGeneratorModule(0), _SODB.GetShieldModule(0), _SODB.GetFlightModule(0), _SODB.GetWeaponModule(0), _SODB.GetWeaponModule(0)));
    }

    public void AddClubMember(CClubMember member)
    {
        _clubMembers.Add(member);
    }

    public string FundToString()
    {
        return _fund.ToString("N1") + "만원";
    }
}
