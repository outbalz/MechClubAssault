using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "newDataBase", menuName = "ScriptableObjects/Database", order = 0)]
public class ScriptableObjectDataBase : ScriptableObject
{
    [SerializeField] private List<ScriptableObjectGeneratorModule> _generatorModules = new List<ScriptableObjectGeneratorModule>();
    [SerializeField] private List<ScriptableObjectShieldModule> _shieldModules = new List<ScriptableObjectShieldModule>();
    [SerializeField] private List<ScriptableObjectFlightModule> _flightModules = new List<ScriptableObjectFlightModule>();
    [SerializeField] private List<ScriptableObjectWeaponModule> _weaponModules = new List<ScriptableObjectWeaponModule>();

    public List<ScriptableObjectGeneratorModule> GeneratorModules { get { return _generatorModules; } }
    public List<ScriptableObjectShieldModule> ShieldModules { get { return _shieldModules; } }
    public List<ScriptableObjectFlightModule> FlightModules { get { return _flightModules; } }
    public List<ScriptableObjectWeaponModule> WeaponModules { get { return _weaponModules; } }

    public int GetGeneratorModuleCount()
    {
        return _generatorModules.Count;
    }

    public int GetShieldModuleCount()
    {
        return _shieldModules.Count;
    }

    public int GetFlightModuleCount()
    {
        return _flightModules.Count;
    }

    public int GetWeaponModuleCount()
    {
        return _weaponModules.Count;
    }

    public ScriptableObjectGeneratorModule GetGeneratorModule(int index)
    {
        if (index >= 0 && index < _generatorModules.Count)
        {
            return _generatorModules[index];
        }
        return null;
    }

    public ScriptableObjectShieldModule GetShieldModule(int index)
    {
        if (index >= 0 && index < _shieldModules.Count)
        {
            return _shieldModules[index];
        }
        return null;
    }

    public ScriptableObjectFlightModule GetFlightModule(int index)
    {
        if (index >= 0 && index < _flightModules.Count)
        {
            return _flightModules[index];
        }
        return null;
    }

    public ScriptableObjectWeaponModule GetWeaponModule(int index)
    {
        if (index >= 0 && index < _weaponModules.Count)
        {
            return _weaponModules[index];
        }
        return null;
    }
}
