using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CItemSlotController : MonoBehaviour, IDropHandler
{
    public enum ESlotType
    {
        Inventory,
        GeneratorSlot,
        ShieldSlot,
        FlightModuleSlot,
        WeaponSlotL,
        WeaponSlotR
    }

    #region inspector
    [SerializeField] private ESlotType _slotType;
    [SerializeField] private CItemDragContorller _itemController;
    #endregion

    #region private var
    private CClubMember _clubMember;
    #endregion

    #region getter
    public ESlotType SlotType { get { return _slotType; } set { _slotType = value; } }
    #endregion

    public void InitializeSlot(CClubMember clubMember,IItemable item)
    {
        _clubMember = clubMember;

        if(_itemController == null)
        {
            Debug.LogWarning("Missing _itemController");
            return;
        }

        _itemController.InitializeItem(this, item);
    }

    public void ApplySlot()
    {
        if (_slotType == ESlotType.Inventory)
        {
            if(_itemController.Item == null)
            {
                return;
            }

            CGameProgressManager.Instance.AddItemToInventory(_itemController.Item);
            return;
        }

        else if (_clubMember == null)
        {
            Debug.LogWarning("Missing _clubMember");
            return;
        }

        switch (_slotType)
        {
            case ESlotType.GeneratorSlot:
                _clubMember.GeneratorModule = _itemController.Item as ScriptableObjectGeneratorModule;
                break;
            case ESlotType.ShieldSlot:
                _clubMember.ShieldModule = _itemController.Item as ScriptableObjectShieldModule;
                break;
            case ESlotType.FlightModuleSlot:
                _clubMember.FlightModule = _itemController.Item as ScriptableObjectFlightModule;
                break;
            case ESlotType.WeaponSlotL:
                _clubMember.WeaponModuleL = _itemController.Item as ScriptableObjectWeaponModule;
                break;
            case ESlotType.WeaponSlotR:
                _clubMember.WeaponModuleR = _itemController.Item as ScriptableObjectWeaponModule;
                break;
            default:
                Debug.LogWarning("unknown slotType Err");
                break;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropedGO = eventData.pointerDrag;
        CItemDragContorller dropedItem = dropedGO.GetComponent<CItemDragContorller>();

        if (dropedItem == null)
        {
            return;
        }

        CItemSlotController originSlot = dropedItem.Slot;


        if (_slotType == ESlotType.Inventory)
        {
            if (originSlot.SlotType == ESlotType.Inventory)
            {
                IItemable tempItem = dropedItem.Item;
                dropedItem.InitializeItem(_itemController.Item);
                _itemController.InitializeItem(tempItem);
                return;
            }

            if (_itemController.Item == null)
            {
                _itemController.gameObject.SetActive(true);
                _itemController.InitializeItem(dropedItem.Item);
                ApplySlot();
                dropedItem.InitializeItem(null);
                originSlot.ApplySlot();
                return;
            }

            switch (originSlot.SlotType)
            {
                case ESlotType.GeneratorSlot:

                    if(_itemController.Item is ScriptableObjectGeneratorModule)
                    {
                        CGameProgressManager.Instance.Inventory.Remove(_itemController.Item);
                        IItemable tempItem = dropedItem.Item;
                        dropedItem.InitializeItem(_itemController.Item);
                        _itemController.InitializeItem(tempItem);
                        ApplySlot();
                        originSlot.ApplySlot();
                        return;
                    }

                    else
                    {
                        return;
                    }

                case ESlotType.ShieldSlot:

                    if (_itemController.Item is ScriptableObjectShieldModule)
                    {
                        CGameProgressManager.Instance.Inventory.Remove(_itemController.Item);
                        IItemable tempItem = dropedItem.Item;
                        dropedItem.InitializeItem(_itemController.Item);
                        _itemController.InitializeItem(tempItem);
                        ApplySlot();
                        originSlot.ApplySlot();
                        return;
                    }

                    else
                    {
                        return;
                    }

                case ESlotType.FlightModuleSlot:

                    if (_itemController.Item is ScriptableObjectFlightModule)
                    {
                        CGameProgressManager.Instance.Inventory.Remove(_itemController.Item);
                        IItemable tempItem = dropedItem.Item;
                        dropedItem.InitializeItem(_itemController.Item);
                        _itemController.InitializeItem(tempItem);
                        ApplySlot();
                        originSlot.ApplySlot();
                        return;
                    }

                    else
                    {
                        return;
                    }

                case ESlotType.WeaponSlotL:
                case ESlotType.WeaponSlotR:

                    if (_itemController.Item is ScriptableObjectWeaponModule)
                    {
                        CGameProgressManager.Instance.Inventory.Remove(_itemController.Item);
                        IItemable tempItem = dropedItem.Item;
                        dropedItem.InitializeItem(_itemController.Item);
                        _itemController.InitializeItem(tempItem);
                        ApplySlot();
                        originSlot.ApplySlot();
                        return;
                    }

                    else
                    {
                        return;
                    }

                default:
                    break;
            }

            return;
        }

        else if (_clubMember == null)
        {
            Debug.LogWarning("Missing _clubMember");
            return;
        }

        switch (_slotType)
        {
            case ESlotType.GeneratorSlot:

                if (dropedItem.Item is ScriptableObjectGeneratorModule)
                {
                    if (originSlot.SlotType == ESlotType.Inventory)
                    {
                        CGameProgressManager.Instance.Inventory.Remove(dropedItem.Item);
                    }

                    _itemController.gameObject.SetActive(true);
                    IItemable tempItem = dropedItem.Item;
                    dropedItem.InitializeItem(_itemController.Item);
                    _itemController.InitializeItem(tempItem);
                    ApplySlot();
                    originSlot.ApplySlot();
                    return;
                }

                else
                {
                    return;
                }

            case ESlotType.ShieldSlot:

                if (dropedItem.Item is ScriptableObjectShieldModule)
                {
                    if (originSlot.SlotType == ESlotType.Inventory)
                    {
                        CGameProgressManager.Instance.Inventory.Remove(dropedItem.Item);
                    }

                    _itemController.gameObject.SetActive(true);
                    IItemable tempItem = dropedItem.Item;
                    dropedItem.InitializeItem(_itemController.Item);
                    _itemController.InitializeItem(tempItem);
                    ApplySlot();
                    originSlot.ApplySlot();
                    return;
                }

                else
                {
                    return;
                }

            case ESlotType.FlightModuleSlot:

                if (dropedItem.Item is ScriptableObjectFlightModule)
                {
                    if (originSlot.SlotType == ESlotType.Inventory)
                    {
                        CGameProgressManager.Instance.Inventory.Remove(dropedItem.Item);
                    }

                    _itemController.gameObject.SetActive(true);
                    IItemable tempItem = dropedItem.Item;
                    dropedItem.InitializeItem(_itemController.Item);
                    _itemController.InitializeItem(tempItem);
                    ApplySlot();
                    originSlot.ApplySlot();
                    return;
                }

                else
                {
                    return;
                }

            case ESlotType.WeaponSlotL:
            case ESlotType.WeaponSlotR:

                if (dropedItem.Item is ScriptableObjectWeaponModule)
                {
                    if (originSlot.SlotType == ESlotType.Inventory)
                    {
                        CGameProgressManager.Instance.Inventory.Remove(dropedItem.Item);
                    }

                    _itemController.gameObject.SetActive(true);
                    IItemable tempItem = dropedItem.Item;
                    dropedItem.InitializeItem(_itemController.Item);
                    _itemController.InitializeItem(tempItem);
                    ApplySlot();
                    originSlot.ApplySlot();
                    return;
                }

                else
                {
                    return;
                }

            default:
                break;
        }

    }
}
