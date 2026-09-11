using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CHairPresetData
{
    [SerializeField] private GameObject _hairGO;
    [SerializeField] private Renderer[] _mainColoRenderer;
    [SerializeField] private Renderer _highlightRenderer;

    public GameObject HairGO { get { return _hairGO; } }
    public Renderer[] MainColoRenderer { get { return _mainColoRenderer; } }
    public Renderer HighlightRenderer { get { return _highlightRenderer; } }
}


public class CHairStyleController : MonoBehaviour
{
    [SerializeField] private CHairPresetData[] _presetData;

    public void InitializeHair(int index, Color mainColor, Color highLightColor)
    {
        if(_presetData == null)
        {
            Debug.LogWarning("hairPresetData is null");
            return;
        }

        for (int i = 0; i < _presetData.Length; i++)
        {
            if (index == i)
            {
                _presetData[i].HairGO.SetActive(true);

                for (int j = 0; j < _presetData[i].MainColoRenderer.Length; j++)
                {
                    _presetData[i].MainColoRenderer[j].material.color = mainColor;
                }

                if (_presetData[i].HighlightRenderer != null)
                {
                    _presetData[i].HighlightRenderer.material.color = highLightColor;
                }

                continue;
            }

            Destroy(_presetData[i].HairGO);
        }

        _presetData = null;

        this.enabled = false;
    }
}
