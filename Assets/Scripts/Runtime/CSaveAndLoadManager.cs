using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[System.Serializable]
public class CSaveData
{
    [SerializeField] private string[] _clumManberNames;
    [SerializeField] private int[] _generatorModuleIndex;
    [SerializeField] private int[] _shieldModuleIndex;
    [SerializeField] private int[] _flightModuleIndex;
    [SerializeField] private int[] _weaponModuleLIndex;
    [SerializeField] private int[] _weaponModuleRIndex;
    
    [SerializeField] private int[] _hairStyleIndex;
    [SerializeField] private float[][] _hairColor;
    [SerializeField] private float[][] _hairHighightColor;

    [SerializeField] private int[] _eyeColorDataIndex;

    [SerializeField] private int _level;
    [SerializeField] private float _fund;
    [SerializeField] private float _reputation;
    [SerializeField] private int _recruitChance;

    [SerializeField] private int[] _inventoryItemType;
    [SerializeField] private int[] _inventoryItemIndex;

    public CSaveData(CGameProgressManager gameProgress)
    {
        _level = gameProgress.Level;
        _fund = gameProgress.Fund;
        _reputation = gameProgress.Reputation;
        _recruitChance = gameProgress.RecruitChance;

        _clumManberNames = new string[gameProgress.ClubMembers.Count];
        _generatorModuleIndex = new int[gameProgress.ClubMembers.Count];
        _shieldModuleIndex = new int[gameProgress.ClubMembers.Count];
        _flightModuleIndex = new int[gameProgress.ClubMembers.Count];
        _weaponModuleLIndex = new int[gameProgress.ClubMembers.Count];
        _weaponModuleRIndex = new int[gameProgress.ClubMembers.Count];

        _hairStyleIndex = new int[gameProgress.ClubMembers.Count];
        _hairColor = new float[gameProgress.ClubMembers.Count][];
        _hairHighightColor = new float[gameProgress.ClubMembers.Count][];

        _eyeColorDataIndex = new int[gameProgress.ClubMembers.Count];

        Dictionary<ScriptableObjectGeneratorModule, int> generatorModuleMap = gameProgress.SODB.GetGeneratorModuleMap();
        Dictionary<ScriptableObjectShieldModule, int> shieldModuleMap = gameProgress.SODB.GetShieldModuleMap();
        Dictionary<ScriptableObjectFlightModule, int> flightModuleMap = gameProgress.SODB.GetFlightModuleMap();
        Dictionary<ScriptableObjectWeaponModule, int> weaponModuleMap = gameProgress.SODB.GetWeaponModuleMap();

        Dictionary<ScriptableObjectEyeColorData, int> eyeDataMap = gameProgress.SODB.GetEyeMap();


        for (int i = 0; i < gameProgress.ClubMembers.Count; i++)
        {
            CClubMember clubMember = gameProgress.ClubMembers[i];

            _clumManberNames[i] = clubMember.Name;

            #region module
            if (clubMember.GeneratorModule == null)
            {
                _generatorModuleIndex[i] = -1;
            }

            else
            {
                _generatorModuleIndex[i] = generatorModuleMap[clubMember.GeneratorModule];
            }

            if(clubMember.ShieldModule == null)
            {
                _shieldModuleIndex[i] = -1;
            }

            else
            {
                _shieldModuleIndex[i] = shieldModuleMap[clubMember.ShieldModule]; 
            }

            if(clubMember.FlightModule == null)
            {
                _flightModuleIndex[i] = -1;
            }

            else
            {
                _flightModuleIndex[i] = flightModuleMap[clubMember.FlightModule];
            }

            if(clubMember.WeaponModuleL == null)
            {
                _weaponModuleLIndex[i] = -1;
            }

            else
            {
                _weaponModuleLIndex[i] = weaponModuleMap[clubMember.WeaponModuleL];
            }

            if(clubMember.WeaponModuleR == null)
            {
                _weaponModuleRIndex[i] = -1;
            }

            else
            {
                _weaponModuleRIndex[i] = weaponModuleMap[clubMember.WeaponModuleR];
            }
            #endregion

            _hairStyleIndex[i] = clubMember.HairStyleIndex;

            _hairColor[i] = new float[3] { clubMember.HairColor.r, clubMember.HairColor.g, clubMember.HairColor.b };
            _hairHighightColor[i] = new float[3] { clubMember.HairHighightColor.r, clubMember.HairHighightColor.g, clubMember.HairHighightColor.b };

            _eyeColorDataIndex[i] = eyeDataMap[clubMember.EyeColorData];
        }


        _inventoryItemType = new int[gameProgress.Inventory.Count]; 
        _inventoryItemIndex = new int[gameProgress.Inventory.Count];

        for (int i = 0; i < gameProgress.Inventory.Count; i++)
        {
            IItemable item = gameProgress.Inventory[i];

            switch (item)
            {
                case IItemable I when I is ScriptableObjectGeneratorModule g:
                    _inventoryItemType[i] = 0;
                    _inventoryItemIndex[i] = generatorModuleMap[g];
                    break;
                case IItemable I when I is ScriptableObjectShieldModule s:
                    _inventoryItemType[i] = 1;
                    _inventoryItemIndex[i] = shieldModuleMap[s];
                    break;
                case IItemable I when I is ScriptableObjectFlightModule f:
                    _inventoryItemType[i] = 2;
                    _inventoryItemIndex[i] = flightModuleMap[f];
                    break;
                case IItemable I when I is ScriptableObjectWeaponModule w:
                    _inventoryItemType[i] = 3;
                    _inventoryItemIndex[i] = weaponModuleMap[w];
                    break;
                default:
                    break;
            }
        }

    }

    private List<CClubMember> GetClubMember(ScriptableObjectDataBase DB)
    {

        List<CClubMember> clubMembers = new List<CClubMember>();

        for (int i = 0; i < _clumManberNames.Length; i++)
        {
            string name = _clumManberNames[i];

            ScriptableObjectGeneratorModule generatorModule = (_generatorModuleIndex[i] > -1) ? DB.GetGeneratorModule(_generatorModuleIndex[i]) : null;
            ScriptableObjectShieldModule shieldModule = (_shieldModuleIndex[i] > -1) ? DB.GetShieldModule(_shieldModuleIndex[i]) : null;
            ScriptableObjectFlightModule flightModule = (_flightModuleIndex[i] > -1) ? DB.GetFlightModule(_flightModuleIndex[i]) : null;
            ScriptableObjectWeaponModule weaponModuleL = (_weaponModuleLIndex[i] > -1) ? DB.GetWeaponModule(_weaponModuleLIndex[i]) : null;
            ScriptableObjectWeaponModule weaponModuleR = (_weaponModuleRIndex[i] > -1) ? DB.GetWeaponModule(_weaponModuleRIndex[i]) : null;

            int hairStyleIndex = _hairStyleIndex[i];
            Color hairColor = new Color(_hairColor[i][0], _hairColor[i][1], _hairColor[i][2],1);
            Color hairHighightColor = new Color(_hairHighightColor[i][0], _hairHighightColor[i][1], _hairHighightColor[i][2],1);

            ScriptableObjectEyeColorData eyeColorData = DB.GetEyeColorData(_eyeColorDataIndex[i]);
            CClubMember clubMember = new CClubMember
                (
                name,
                generatorModule,
                shieldModule,
                flightModule,
                weaponModuleL,
                weaponModuleR,
                hairStyleIndex,
                hairColor,
                hairHighightColor,
                eyeColorData
                );

            clubMembers.Add(clubMember);
        }

        return clubMembers;
    }

    private List<IItemable> GetInventory(ScriptableObjectDataBase DB)
    {
        List<IItemable> inventory = new List<IItemable>();

        for (int i = 0; i < _inventoryItemIndex.Length; i++)
        {
            switch (_inventoryItemType[i])
            {
                case 0:
                    inventory.Add(DB.GetGeneratorModule(_inventoryItemIndex[i]));
                    break;
                case 1:
                    inventory.Add(DB.GetShieldModule(_inventoryItemIndex[i]));
                    break;
                case 2:
                    inventory.Add(DB.GetFlightModule(_inventoryItemIndex[i]));
                    break;
                case 3:
                    inventory.Add(DB.GetWeaponModule(_inventoryItemIndex[i]));
                    break;
                default:
                    break;
            }
        }

        return inventory;
    }

    public void LoadData(CGameProgressManager progressManager)
    {
        ScriptableObjectDataBase DB = progressManager.SODB;

        progressManager.ClubMembers = GetClubMember(DB);
        progressManager.Inventory = GetInventory(DB);

        progressManager.Level = _level;
        progressManager.Fund = _fund;
        progressManager.Reputation = _reputation;
        progressManager.RecruitChance = _recruitChance;
    }
}

public static class CSaveAndLoadManager 
{
    private static readonly string _path = Application.persistentDataPath + "/save.data";

    public static void SaveGame()
    {
        if(CGameProgressManager.Instance == null)
        {
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(_path, FileMode.Create);

        CSaveData saveData = new CSaveData(CGameProgressManager.Instance);

        formatter.Serialize(fileStream, saveData);
        fileStream.Close();
    }

    public static void LoadGame()
    {
        if(CGameProgressManager.Instance == null)
        {
            return;
        }

        if (File.Exists(_path) == false)
        {
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(_path, FileMode.Open);

        if(fileStream.Length <= 0)
        {
            return;
        }

        CSaveData saveData = formatter.Deserialize(fileStream) as CSaveData;
        fileStream.Close();

        saveData.LoadData(CGameProgressManager.Instance);
    }
}
