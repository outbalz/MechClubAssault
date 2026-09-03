using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CItemDragContorller : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Transform _previousParent;

    void Start()
    {
        _previousParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _previousParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(_previousParent);
    }
}