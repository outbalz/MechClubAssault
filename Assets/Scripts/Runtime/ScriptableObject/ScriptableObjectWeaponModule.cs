using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponModule", menuName = "ScriptableObjects/WeaponModule", order = 1)]
public class ScriptableObjectWeaponModule : ScriptableObject, IItemable
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;

    [Space]
    [TextArea]
    [SerializeField] private string _description;

    [Space]
    [SerializeField] private float _weaponInnerArcDeg;
    [SerializeField] private float _weaponOutterArcDeg;
    [SerializeField] private float _weaponRange;
    [SerializeField] private float _weaponCoolDown;
    [SerializeField] private float _weaponDamege;
    [SerializeField] private float _weaponEnegyCost;

    [SerializeField] private float _price;

    public string ModuleName { get { return _name; } }
    public Sprite Icon { get { return _icon; } }
    public string Description { get { return _description; } }

    public float WeaponInnerArcDeg { get { return _weaponInnerArcDeg; } }
    public float WeaponOutterArcDeg { get { return _weaponOutterArcDeg; } }
    public float WeaponRange {  get { return _weaponRange; } }
    public float WeaponCoolDown { get { return _weaponCoolDown; } }
    public float WeaponDamege { get { return _weaponDamege; } }
    public float WeaponEnegyCost {  get { return _weaponEnegyCost; } }
    public float Price { get { return _price; } }

}
