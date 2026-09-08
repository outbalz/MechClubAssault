using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Space]
    [Header("Preview")]
    [SerializeField] private RawImage _previewImage;
    #endregion

    #region private var
    private CClubMember _clubMember;
    private CUnitPreviewController _previewController;
    #endregion

    public void InitializePanel(CClubMember clubMember, CUnitPreviewController previewController)
    {
        _clubMember = clubMember;
        _nameText.text = _clubMember.Name;

        _weaponSlotL.InitializeSlot(clubMember, clubMember.WeaponModuleL, this);
        _weaponSlotR.InitializeSlot(clubMember, clubMember.WeaponModuleR, this);
        _generatorSlot.InitializeSlot(clubMember, clubMember.GeneratorModule, this);
        _shieldSlot.InitializeSlot(clubMember, clubMember.ShieldModule, this);
        _flightModuleSlot.InitializeSlot(clubMember, clubMember.FlightModule, this);

        _previewController = previewController;

        _previewController.UnitStyleinit(clubMember.HairStyleIndex, clubMember.HairColor, clubMember.HairHighightColor, clubMember.EyeColorData);

        _previewImage.texture = _previewController.PreviewTexure;
        bool hasWeaponL = (clubMember.WeaponModuleL != null);
        bool hasWeaponR = (clubMember.WeaponModuleR != null);
        bool hasFlightModule = (clubMember.FlightModule != null);
        _previewController.SetModuleActive(hasWeaponL, hasWeaponR, hasFlightModule);
    }

    public void UpdatePreview()
    {
        bool hasWeaponL = (_clubMember.WeaponModuleL != null);
        bool hasWeaponR = (_clubMember.WeaponModuleR != null);
        bool hasFlightModule = (_clubMember.FlightModule != null);
        _previewController.SetModuleActive(hasWeaponL, hasWeaponR, hasFlightModule);
    }

}
