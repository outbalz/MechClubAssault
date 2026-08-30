using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponModule", menuName = "ScriptableObjects/WeaponModule", order = 1)]
public class ScriptableObjectWeaponModule : ScriptableObject
{
    [SerializeField] private string _name;

    [SerializeField] private float _weaponInnerArcDeg;
    [SerializeField] private float _weaponOutterArcDeg;
    [SerializeField] private float _weaponRange;
    [SerializeField] private float _weaponCoolDown;
    [SerializeField] private float _weaponDamege;
    [SerializeField] private float _weaponEnegyCost;

    public string ModuleName { get { return _name; } }

    public float WeaponInnerArcDeg { get { return _weaponInnerArcDeg; } }
    public float WeaponOutterArcDeg { get { return _weaponOutterArcDeg; } }
    public float WeaponRange {  get { return _weaponRange; } }
    public float WeaponCoolDown { get { return _weaponCoolDown; } }
    public float WeaponDamege { get { return _weaponDamege; } }
    public float WeaponEnegyCost {  get { return _weaponEnegyCost; } }

}
