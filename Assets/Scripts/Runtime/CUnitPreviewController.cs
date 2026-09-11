using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUnitPreviewController : MonoBehaviour
{
    #region inspector
    [Header("Camera")]
    [SerializeField] private Camera _previewCamera;

    [Space]
    [Header("Style")]
    [SerializeField] private CHairStyleController _hairStyleController;
    [SerializeField] private Renderer _eyeRenderer;
    [SerializeField] private Renderer _eyeLightRenderer;

    [Space]
    [Header("Module")]
    [SerializeField] private GameObject _weaponL;
    [SerializeField] private GameObject _weaponR;
    [SerializeField] private GameObject _flightModule;
    #endregion

    #region private var
    private RenderTexture _previewTexture;
    #endregion

    #region getter
    public RenderTexture PreviewTexure { get { return _previewTexture; } }
    #endregion

    private void Awake()
    {

        if (_hairStyleController == null)
        {
            Debug.LogWarning("Missing _hairStyleController");
        }

        if (_eyeRenderer == null || _eyeLightRenderer == null)
        {
            Debug.LogWarning("Missing Eye renderer");
        }

        if(_weaponL == null || _weaponR == null || _flightModule == null)
        {
            Debug.LogWarning("Missing module element");
        }

        if (_previewCamera != null)
        {
            _previewTexture = new RenderTexture(512, 512, 3);

            _previewCamera.targetTexture = _previewTexture;
        }

    }

    public void UnitStyleinit
    (
    int hairIndex,
    Color hairColor,
    Color hairHighLightColor,
    ScriptableObjectEyeColorData eyeColor
    )
    {
        _hairStyleController.InitializeHair(hairIndex, hairColor, hairHighLightColor);
        _eyeRenderer.material = eyeColor.EyeMaterial;
        _eyeLightRenderer.material = eyeColor.EyeMaterial;
    }

    public void SetModuleActive(bool hasWeaponL, bool hasWeaponR, bool hasFlightModule)
    {
        _weaponL.SetActive(hasWeaponL);
        _weaponR.SetActive(hasWeaponR);
        _flightModule.SetActive(hasFlightModule);
    }
}
