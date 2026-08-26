using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CBattleUIManager : MonoBehaviour
{
    #region inspector
    [Header("manager")]
    [SerializeField] private CUnitInputManager _unitInputManager;

    [Space]
    [Header("UI Element")]
    [SerializeField] private GameObject _playerTurnUI;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private Toggle[] _WeaponToggles;
    #endregion

    private void Reset()
    {
        Initialize();
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_unitInputManager == null)
        {
            if (TryGetComponent<CUnitInputManager>(out _unitInputManager) == false)
            {
                Debug.LogWarning("Missing CUnitInputManager");

                enabled = false;
                return;
            }
        }

        if (_playerTurnUI == null)
        {
            Debug.LogWarning("Missing _playerTurnUI");
        }
    }


    private void Start()
    {
        _speedText.text = $"{_unitInputManager.SelectedUnit.Speed}/mps";
    }

    public void SetAccelerationLevel(bool init)
    {
        if (_unitInputManager.SelectedUnit == null)
        {
            return;
        }

        if (init)
        {
            _speedSlider.value = 1;
        }

        _unitInputManager.SelectedUnit.MovementController.SetAccelerationLevel((int)_speedSlider.value);

        _unitInputManager.SelectedUnit.GetSpeed();

        if (_unitInputManager.SelectedUnit.Speed < 0)
        {
            _speedSlider.value++;
            _unitInputManager.SelectedUnit.MovementController.SetAccelerationLevel((int)_speedSlider.value);
            _unitInputManager.SelectedUnit.GetSpeed();
        }

        if (_unitInputManager.SelectedUnit.Speed > _unitInputManager.SelectedUnit.MaxSpeed)
        {
            _speedSlider.value--;

            _unitInputManager.SelectedUnit.MovementController.SetAccelerationLevel((int)_speedSlider.value);
            _unitInputManager.SelectedUnit.GetSpeed();
        }

        List<Vector3> posList = new List<Vector3>();

        posList.Add(_unitInputManager.SelectedUnit.transform.position);

        Vector3 pos = _unitInputManager.SelectedUnit.transform.position + _unitInputManager.SelectedUnit.transform.rotation * Vector3.forward * _unitInputManager.SelectedUnit.Speed * 5;

        posList.Add(pos);

        _unitInputManager.SelectedUnit.VisualizePath(posList);

        _unitInputManager.SelectedUnit.MovementController.SetTargetPos(pos, pos);

        _speedText.text = $"{_unitInputManager.SelectedUnit.Speed}/mps";
    }

    public void SetWeaponEnable(int i)
    {
        if (_unitInputManager.SelectedUnit == null)
        {
            return;
        }

        bool Enable = _WeaponToggles[i].isOn;

        _unitInputManager.SelectedUnit.WeaponContorller.SetWeaponEnable((i==0)? true : false, Enable);
    }

    public void SetPlayerTurnUI(bool enable)
    {
        _playerTurnUI.SetActive(enable);
    }

}
