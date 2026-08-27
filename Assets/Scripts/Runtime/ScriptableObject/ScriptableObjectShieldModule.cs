using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newShieldModule", menuName = "ScriptableObjects/ShieldModule", order = 4)]
public class ScriptableObjectShieldModule : ScriptableObject
{
    public string _name;

    public float _startShield;
    public float _maxShield;
    public float _shieldRegen;
    public float _shieldRegenCost;
}
