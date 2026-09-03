using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CChararcterPanelContorller : MonoBehaviour
{

    #region inspector
    [Header("Item Slot")]
    [SerializeField] private CItemSlotController _weaponSlotL;
    [SerializeField] private CItemSlotController _weaponSlotR;
    [SerializeField] private CItemSlotController _generatorSlot;
    [SerializeField] private CItemSlotController _shieldSlot;
    [SerializeField] private CItemSlotController _flightModuleSlot;

    [Space]
    [Header("Text")]
    [SerializeField] private TMP_Text _nameText;
    #endregion

    #region private var
    private CClubMember _clubMember;
    #endregion

    public void InitializePanel(CClubMember clubMember)
    {
        _clubMember = clubMember;
        _nameText.text = _clubMember.Name;

        _weaponSlotL.InitializeSlot(clubMember, clubMember.WeaponModuleL);
        _weaponSlotR.InitializeSlot(clubMember, clubMember.WeaponModuleR);
        _generatorSlot.InitializeSlot(clubMember, clubMember.GeneratorModule);
        _shieldSlot.InitializeSlot(clubMember, clubMember.ShieldModule);
        _flightModuleSlot.InitializeSlot(clubMember, clubMember.FlightModule);
    }

}
