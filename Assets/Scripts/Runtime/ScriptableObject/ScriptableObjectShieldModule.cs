using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newShieldModule", menuName = "ScriptableObjects/ShieldModule", order = 4)]
public class ScriptableObjectShieldModule : ScriptableObject
{
    [SerializeField] private string _name;

    [SerializeField] private float _startShield;
    [SerializeField] private float _maxShield;
    [SerializeField] private float _shieldRegen;
    [SerializeField] private float _shieldRegenCost;

    public string ModuleName { get { return _name; } }

    public float StartShield { get { return _startShield; } }
    public float MaxShield { get { return _maxShield; } }
    public float ShieldRegen { get { return _shieldRegen; } }
    public float ShieldRegenCost { get { return _shieldRegenCost; } }
}
