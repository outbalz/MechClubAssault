using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CUnitData
{
    [SerializeField] private string _unitName;

    [SerializeField] private ScriptableObjectFlightModule _flightModule;
    [SerializeField] private ScriptableObjectGeneratorModule _generatorModule;
    [SerializeField] private ScriptableObjectShieldModule _shieldModule;
    [SerializeField] private ScriptableObjectWeaponModule _weaponModuleL;
    [SerializeField] private ScriptableObjectWeaponModule _weaponModuleR;


    public string UnitName {  get { return _unitName; } }

    public ScriptableObjectFlightModule FlightModule {  get { return _flightModule; } }
    public ScriptableObjectGeneratorModule GeneratorModule { get { return _generatorModule; } }
    public ScriptableObjectShieldModule ShieldModule { get { return _shieldModule; } }
    public ScriptableObjectWeaponModule WeaponModuleL {  get { return _weaponModuleL; } }
    public ScriptableObjectWeaponModule WeaponModuleR {  get { return _weaponModuleR; } }

}


public class CUnitSpawnManager : MonoBehaviour
{

    #region inspector
    [Header("Unit Data")]
    [SerializeField] private List<CUnitData> _playerUnitData;
    [SerializeField] private List<CUnitData> _enemyUnitData;

    [Space]
    [Header("Unit prefab")]
    [SerializeField] private GameObject _playerUnitPrefab;
    [SerializeField] private GameObject _enemyUnitPrefab;

    [Space]
    [Header("SpawnPoint")]
    [SerializeField] private List<Vector3> _spanwPointPlayerUnit;
    [SerializeField] private List<Vector3> _spanwPointEnemyUnit;

    [Space]
    [Header("Manager")]
    [SerializeField] private CTurnStateManager _turnStateManager;
    #endregion

    #region private var
    private List<CUnitController> _playrUnits = new List<CUnitController>();
    private List<CEnemyUnitContorller> _enemyUnits = new List<CEnemyUnitContorller>();
    #endregion

    private void Awake()
    {
        if(_playerUnitData.Count == 0 || _enemyUnitData.Count == 0)
        {
            Debug.LogWarning("Missing UnitData");
            enabled = false;
            return;
        }

        if(_playerUnitPrefab == null || _enemyUnitPrefab == null)
        {
            Debug.LogWarning("Missing UnitPrefab");
            enabled = false;
            return;
        }

        if(_turnStateManager == null)
        {
            Debug.LogWarning("Missing Manager");
        }
    }


    void Start()
    {
        SpawnUnit();
        SpawnEnemy();
        _turnStateManager.InitializeUnitList(_playrUnits, _enemyUnits);
    }

    private void SpawnUnit()
    {
        int posI = 0;
        float offsetZ = 0;

        if (_spanwPointPlayerUnit.Count == 0)
        {
            _spanwPointPlayerUnit.Add(Vector3.zero);
        }

        for (int i = 0; i < _playerUnitData.Count; i++)
        {
            if (_playerUnitData[i] == null)
            {
                continue;
            }

            GameObject unit = Instantiate(_playerUnitPrefab);
            CUnitController controller = unit.GetComponent<CUnitController>();

            controller.UnitModuleInit
                (
                    _playerUnitData[i].GeneratorModule,
                    _playerUnitData[i].ShieldModule,
                    _playerUnitData[i].FlightModule,
                    _playerUnitData[i].WeaponModuleL,
                    _playerUnitData[i].WeaponModuleR
                );

            _playrUnits.Add(controller);

            unit.transform.position = _spanwPointPlayerUnit[posI];
            unit.transform.position += Vector3.back * offsetZ;
            posI++;

            if (posI >= _spanwPointPlayerUnit.Count)
            {
                posI = 0;
                offsetZ -= 10;
            }

        }
    }

    private void SpawnEnemy()
    {
        int posI = 0;
        float offsetZ = 0;

        if (_spanwPointEnemyUnit.Count == 0)
        {
            _spanwPointEnemyUnit.Add(Vector3.zero);
        }

        for (int i = 0; i < _enemyUnitData.Count; i++)
        {
            if (_enemyUnitData[i] == null)
            {
                continue;
            }

            GameObject unit = Instantiate(_enemyUnitPrefab);
            CEnemyUnitContorller controller = unit.GetComponent<CEnemyUnitContorller>();

            controller.UnitModuleInit
                (
                    _enemyUnitData[i].ShieldModule,
                    _enemyUnitData[i].FlightModule,
                    _enemyUnitData[i].WeaponModuleL,
                    _enemyUnitData[i].WeaponModuleR
                );

            _enemyUnits.Add(controller);

            unit.transform.position = _spanwPointEnemyUnit[posI];
            unit.transform.position += Vector3.back * offsetZ;
            posI++;

            if (posI >= _spanwPointEnemyUnit.Count)
            {
                posI = 0;
                offsetZ += 10;
            }

        }
    }

}
