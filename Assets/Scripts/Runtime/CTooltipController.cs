using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CTooltipController : MonoBehaviour
{

    #region inspector
    [SerializeField] private TMP_Text _header;
    [SerializeField] private TMP_Text _content;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private RectTransform _rootTransform;
    #endregion

    #region private var
    private static CTooltipController _instance;
    #endregion

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        if(_header == null || _content == null)
        {
            Debug.LogWarning("Missing TextElemnt");
        }

        if(_rectTransform == null)
        {
            if(TryGetComponent<RectTransform>(out _rectTransform))
            {
                Debug.LogWarning("Missing _rectTransform");
            }
        }

        if(_rootTransform == null)
        {
            if(transform.parent.TryGetComponent<RectTransform>(out _rootTransform))
            {
                Debug.LogWarning("Missing _rootTransform");
            }
        }

        gameObject.SetActive(false);
    }

    public static void ShowTooltip(string header, string content)
    {
        if (_instance == null)
        {
            Debug.LogWarning("CTooltipController _instance is missing");
            return;
        }

        _instance.gameObject.SetActive(true);
        _instance._header.text = header;
        _instance._content.text = content;
    }



    public static void HideTooltip()
    {
        if (_instance == null)
        {
            Debug.LogWarning("CTooltipController _instance is missing");
            return;
        }

        _instance.gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        float pivotX = -0.05f;
        float pivotY = 1;

        if((Screen.width - mousePos.x) / _rootTransform.localScale.x < _rectTransform.sizeDelta.x)
        {
            pivotX = 1;
        }

        if(mousePos.y / _rootTransform.localScale.y < _rectTransform.sizeDelta.y)
        {
            pivotY = 0;
        }

        _rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = mousePos;
    }

}
