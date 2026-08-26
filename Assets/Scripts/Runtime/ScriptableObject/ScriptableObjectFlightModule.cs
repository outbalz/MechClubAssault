using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newFlightModule", menuName = "ScriptableObjects/FlightModule", order = 3)]
public class ScriptableObjectFlightModule : ScriptableObject
{
    public string _name;

    public float _startSpeed;
    public float _maxSpeed;
    public float _turnRate;
    public float _acceleration;
    public float _deceleration;
    public float _airResistance;

    public float _accelerationEnergyCost;
    public float _decelerationEnergyCost;

}