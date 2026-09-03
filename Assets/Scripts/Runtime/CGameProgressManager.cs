using System;
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


[Serializable]
public class CEnemyUnitData
{
    [SerializeField] private string _unitName;

    [SerializeField] private ScriptableObjectFlightModule _flightModule;
    [SerializeField] private ScriptableObjectShieldModule _shieldModule;
    [SerializeField] private ScriptableObjectWeaponModule _weaponModuleL;
    [SerializeField] private ScriptableObjectWeaponModule _weaponModuleR;


    public string UnitName { get { return _unitName; } }

    public ScriptableObjectFlightModule FlightModule { get { return _flightModule; } }
    public ScriptableObjectShieldModule ShieldModule { get { return _shieldModule; } }
    public ScriptableObjectWeaponModule WeaponModuleL { get { return _weaponModuleL; } }
    public ScriptableObjectWeaponModule WeaponModuleR { get { return _weaponModuleR; } }

}

public class CItemData
{
    private IItemable _item;
    private int _count;
    public IItemable Item { get { return _item; } }
    public int Count { get { return _count; } set { _count = value; } }

    public string Name { get { return _item.ModuleName; } }
    public float Price { get { return _item.Price; } }

    public CItemData(IItemable item, int count = 1)
    {
        this._item = item;
        this._count = count;
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

    private int _level = 0;
    private float _fund = 0;
    private float _reputation = 0;

    private List<CItemData> _inventoryGeneratorModule = new List<CItemData>();
    private List<CItemData> _inventoryShieldModule = new List<CItemData>();
    private List<CItemData> _inventoryFlightModule = new List<CItemData>();
    private List<CItemData> _inventoryWeaponModule = new List<CItemData>();
    #endregion

    #region getter
    public ScriptableObjectDataBase SODB { get { return _SODB; } }
    
    public static CGameProgressManager Instance { get { return _instance; } }
    
    public List<CClubMember> ClubMembers { get { return _clubMembers; } }
    
    public float Fund { get { return _fund; } set { _fund = value; } }
    public float Reputation { get { return _reputation; } set { _reputation = value; } }

    public int Level { get { return _level; } set { _level = value; } }

    public List<CItemData> InventoryGeneratorModule { get { return _inventoryGeneratorModule; } set { _inventoryGeneratorModule = value; } }
    public List<CItemData> InventoryShieldModule { get { return _inventoryShieldModule; } set { _inventoryShieldModule = value; } }
    public List<CItemData> InventoryFlightModule { get { return _inventoryFlightModule; } set { _inventoryFlightModule = value; } }
    public List<CItemData> InventoryWeaponModule { get { return _inventoryWeaponModule; } set { _inventoryWeaponModule = value; } }
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

        _level = 0;
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
        _level = 0;
        _fund = 0;
        _reputation = 20;
        _clubMembers.Add(new CClubMember(CUtil.GetRandomName(), _SODB.GetGeneratorModule(0), _SODB.GetShieldModule(0), _SODB.GetFlightModule(0), _SODB.GetWeaponModule(0), _SODB.GetWeaponModule(0)));
        _clubMembers.Add(new CClubMember(CUtil.GetRandomName(), _SODB.GetGeneratorModule(0), _SODB.GetShieldModule(0), _SODB.GetFlightModule(0), _SODB.GetWeaponModule(0), _SODB.GetWeaponModule(0)));
        _inventoryGeneratorModule.Clear();
        _inventoryShieldModule.Clear();
        _inventoryFlightModule.Clear();
        _inventoryWeaponModule.Clear();
    }

    public void AddClubMember(CClubMember member)
    {
        _clubMembers.Add(member);
    }

    public ScriptableObjectLevelData GetLevelData(int index = -1)
    {
        if (_level < 0 || _level >= _SODB.GetLevelListCount())
        {
            Debug.LogWarning("Invalid level: " + _level);
            return null;
        }

        if (index >= _SODB.GetLevelDataCount(_level))
        {
            Debug.LogWarning("Invalid level data index: " + index + " for level: " + _level);
            return null;
        }

        if (index < 0)
        {
            index = UnityEngine.Random.Range(0, _SODB.GetLevelDataCount(_level));
        }

        ScriptableObjectLevelData levelData = _SODB.GetLevelData(_level, index);

        if (levelData == null)
        {
            Debug.LogWarning("Level data not found for level: " + _level + ", index: " + index);
            return null;
        }

        return levelData;
    }   
    
    /*
    public List<CEnemyUnitData> GetLevelCEnemyUnitDatas(out List<Vector3> enemySpawnPoint,int index = -1)
    {
        if(_level < 0 || _level >= _SODB.GetLevelListCount())
        {
            Debug.LogWarning("Invalid level: " + _level);
            enemySpawnPoint = new List<Vector3>();
            return null;
        }

        if (index >= _SODB.GetLevelDataCount(_level))
        {
            Debug.LogWarning("Invalid level data index: " + index + " for level: " + _level);
            enemySpawnPoint = new List<Vector3>();
            return null;
        }

        if (index < 0)
        {
            index = UnityEngine.Random.Range(0, _SODB.GetLevelDataCount(_level));
        }

        ScriptableObjectLevelData levelData = _SODB.GetLevelData(_level, index);

        if (levelData == null)
        {
            Debug.LogWarning("Level data not found for level: " + _level + ", index: " + index);
            enemySpawnPoint = new List<Vector3>();
            return null;
        }

        enemySpawnPoint = levelData.EnemySpawnPoints;
        return levelData.EnemyUnitDatas;
    }
    */

    public string FundToString()
    {
        return _fund.ToString("N0") + "만원";
    }

    public bool AddItemToInventory(CItemData item)
    {
        List<CItemData> inventoryList = null;

        switch(item.Item)
        {
            case ScriptableObjectGeneratorModule generatorModule:
                inventoryList = _inventoryGeneratorModule;
                break;
            case ScriptableObjectShieldModule shieldModule:
                inventoryList = _inventoryShieldModule;
                break;
            case ScriptableObjectFlightModule flightModule:
                inventoryList = _inventoryFlightModule;
                break;
            case ScriptableObjectWeaponModule weaponModule:
                inventoryList = _inventoryWeaponModule;
                break;
            default:
                Debug.LogWarning("Unknown item type: " + item.Item.GetType().Name);
                return false;
        }

        if (inventoryList != null)
        {
            if (inventoryList.Count > 0)
            {
                for (int i = 0; i < inventoryList.Count; i++)
                {
                    if (inventoryList[i].Item == item.Item)
                    {
                        inventoryList[i].Count += item.Count;
                        return true;
                    }
                }
            }

            if (inventoryList.Count < 24)
            {
                inventoryList.Add(item);
                return true;
            }
            Debug.LogWarning("Not enough space in inventory to add this item.");
            return false;
        }
        Debug.LogWarning("Inventory list is null for item type: " + item.Item.GetType().Name);
        return false;
    }

}
