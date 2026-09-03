using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CItemDragContorller : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    #region inspector
    [SerializeField] private Image _image;
    #endregion

    #region private var
    private IItemable _item;
    private CItemSlotController _slot;
    #endregion

    #region getter
    public CItemSlotController Slot { get { return _slot; } set { _slot = value; } }
    public IItemable Item { get { return _item; } set { _item = value; } }
    #endregion


    public void InitializeItem(CItemSlotController slot, IItemable item)
    {
        _slot = slot;

        if (item != null)
        {
            gameObject.SetActive(true);
            _item = item;
            _image.sprite = _item.Icon;
            _image.raycastTarget = true;
        }

        else
        {
            _item = null;
            gameObject.SetActive(false);
        }

        transform.SetParent(slot.transform);
    }


    public void InitializeItem(IItemable item)
    {

        if (item != null)
        {
            gameObject.SetActive(true);
            _item = item;
            _image.sprite = _item.Icon;
            _image.raycastTarget = true;
        }

        else
        {
            _item = null;
            gameObject.SetActive(false);
        }

        transform.SetParent(_slot.transform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_slot.transform);
        _image.raycastTarget = true;
    }
}