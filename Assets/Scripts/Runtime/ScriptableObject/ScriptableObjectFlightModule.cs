using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newFlightModule", menuName = "ScriptableObjects/FlightModule", order = 3)]
public class ScriptableObjectFlightModule : ScriptableObject
{
    [SerializeField] private string _name;

    [SerializeField] private float _startSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _turnRate;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _airResistance;

    [SerializeField] private float _accelerationEnergyCost;
    [SerializeField] private float _decelerationEnergyCost;

    public string moduleName {  get { return _name; } }
    public float StartSpeed { get { return _startSpeed; } }
    public float MaxSpeed { get { return _maxSpeed; } }
    public float TurnRate { get { return _turnRate; } }
    public float Acceleration { get { return _acceleration; } }
    public float Deceleration { get { return _deceleration; } }
    public float AirResistance { get { return _airResistance; } }
    public float AccelerationEnergyCost {  get { return _accelerationEnergyCost; } }
    public float DecelerationEnergyCost { get { return _decelerationEnergyCost; } }

}