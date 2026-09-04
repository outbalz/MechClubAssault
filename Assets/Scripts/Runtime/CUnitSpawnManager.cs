using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CUnitSpawnManager : MonoBehaviour
{

    #region inspector
    [Header("Unit Data")]
    [SerializeField] private List<CClubMember> _playerUnitData;
    [SerializeField] private List<CEnemyUnitData> _enemyUnitData;

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
    private CGameProgressManager _gameProgressManager;

    private List<CUnitController> _playrUnits = new List<CUnitController>();
    private List<CEnemyUnitContorller> _enemyUnits = new List<CEnemyUnitContorller>();
    #endregion

    private void Awake()
    {

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
        _gameProgressManager = CGameProgressManager.Instance;

        if(_gameProgressManager == null)
        {
            Debug.LogWarning("Missing GameProgressManager");
            enabled = false;
            return;
        }

        _playerUnitData = _gameProgressManager.ClubMembers;

        ScriptableObjectLevelData levelData = _gameProgressManager.GetLevelData();

        _enemyUnitData = levelData.EnemyUnitDatas;
        _spanwPointEnemyUnit = levelData.EnemySpawnPoints;

        _turnStateManager.RewardPrize = levelData.RewardPrize;

        //_enemyUnitData = _gameProgressManager.GetLevelCEnemyUnitDatas(out _spanwPointEnemyUnit);


        if (_playerUnitData.Count == 0)
        {
            Debug.LogWarning("Missing UnitData");
            _playerUnitData.Add(new CClubMember
                (
                CUtil.GetRandomName(),
                _gameProgressManager.SODB.GetGeneratorModule(0),
                _gameProgressManager.SODB.GetShieldModule(0),
                _gameProgressManager.SODB.GetFlightModule(0),
                _gameProgressManager.SODB.GetWeaponModule(0),
                _gameProgressManager.SODB.GetWeaponModule(0)
                ));
            _playerUnitData.Add(new CClubMember
                (
                CUtil.GetRandomName(),
                _gameProgressManager.SODB.GetGeneratorModule(0),
                _gameProgressManager.SODB.GetShieldModule(0),
                _gameProgressManager.SODB.GetFlightModule(0),
                _gameProgressManager.SODB.GetWeaponModule(0),
                _gameProgressManager.SODB.GetWeaponModule(0)
                ));
        }

        if (_enemyUnitData.Count == 0)
        {
            Debug.LogWarning("Missing EnemyUnitData");
            enabled = false;
            return;
        }

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

            if (
                _playerUnitData[i].GeneratorModule == null || 
                _playerUnitData[i].ShieldModule == null || 
                _playerUnitData[i].FlightModule == null || 
                _playerUnitData[i].WeaponModuleL == null || 
                _playerUnitData[i].WeaponModuleR == null
                )
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
