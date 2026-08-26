using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGeneratorModule", menuName = "ScriptableObjects/GeneratorModule", order = 2)]
public class ScriptableObjectGeneratorModule : ScriptableObject
{
    public string _name;

    public float _maxEnergy;
    public float _startEnergy;
    public float _energyRegen;
}
