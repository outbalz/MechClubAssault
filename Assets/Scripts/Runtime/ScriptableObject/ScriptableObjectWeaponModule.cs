using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponModule", menuName = "ScriptableObjects/WeaponModule", order = 1)]
public class ScriptableObjectWeaponModule : ScriptableObject
{
    public string _name;

    public float _weaponInnerArcDeg;
    public float _weaponOutterArcDeg;
    public float _weaponRange;
    public float _weaponCoolDown;
    public float _weaponDamege;
    public float _weaponEnegyCost;
}
