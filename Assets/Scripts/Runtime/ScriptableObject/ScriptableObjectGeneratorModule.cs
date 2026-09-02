using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGeneratorModule", menuName = "ScriptableObjects/GeneratorModule", order = 2)]
public class ScriptableObjectGeneratorModule : ScriptableObject
{
    [SerializeField] private string _name;

    [SerializeField] private float _maxEnergy;
    [SerializeField] private float _startEnergy;
    [SerializeField] private float _energyRegen;

    [SerializeField] private float _price;

    public string ModuleName { get { return _name; } }

    public float MaxEnergy {  get { return _maxEnergy; } }
    public float StartEnergy { get { return _startEnergy; } }
    public float EnergyRegen { get { return _energyRegen; } }
    public float Price { get { return _price; } }
}
