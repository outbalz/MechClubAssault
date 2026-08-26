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
    [SerializeField] private TMP_Text _energyText;
    [SerializeField] private Slider _speedSlider;
    [SerializeField] private Toggle[] _WeaponToggles;
    #endregion

    #region private var
    //private float _turnEnergy;
    private float _previousSpeedBarVal;
    private float _previousSpeedCost;
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

        if(_speedText == null || _energyText == null)
        {
            Debug.LogWarning("Missing text element");
        }

    }



    public void TurnInitSelectedUnitUi()
    {
        if(_unitInputManager.SelectedUnit == null)
        {
            SetPlayerTurnUI(false);
            return;
        }

        CUnitController unit = _unitInputManager.SelectedUnit;

        SetAccelerationLevel(true);
    }


    public void SetAccelerationLevel(bool init)
    {
        if (_unitInputManager.SelectedUnit == null)
        {
            return;
        }

        if (init)
        {
            _previousSpeedCost = 0;
            _previousSpeedBarVal = 0;
            _speedSlider.SetValueWithoutNotify(1);
        }

        CUnitController unit = _unitInputManager.SelectedUnit;
        CUnitMovementController MovementController = _unitInputManager.SelectedUnit.MovementController;

        MovementController.SetAccelerationLevel((int)_speedSlider.value);

        //_unitInputManager.SelectedUnit.GetSpeed();

        if (MovementController.Speed < 0)
        {
            //_speedSlider.SetValueWithoutNotify(-2 - MovementController.Speed);
            _speedSlider.value++;
            return;
            //MovementController.SetAccelerationLevel((int)_speedSlider.value);

        }

        if (MovementController.Speed > MovementController.FlightModule._maxSpeed)
        {
            //float overflow = MovementController.Speed - MovementController.FlightModule._maxSpeed;

            //_speedSlider.SetValueWithoutNotify(3 - overflow);
            _speedSlider.value--;
            return;
            //MovementController.SetAccelerationLevel((int)_speedSlider.value);
            
        }

        // float energyCost = Mathf.Abs(_speedSlider.value) - Mathf.Abs(_previousSpeedBarVal);
        float energyCost = 0;

        if (_speedSlider.value > 0)
        {
            energyCost = _speedSlider.value * MovementController.FlightModule._accelerationEnergyCost;
        }

        else if (_speedSlider.value < 0) 
        { 
            energyCost = _speedSlider.value * MovementController.FlightModule._decelerationEnergyCost * -1f;
        }

        /*
        else if(_previousSpeedBarVal > 0)
        {
            energyCost *= MovementController.FlightModule._accelerationEnergyCost;
        }

        else if(_previousSpeedBarVal < 0)
        {
            energyCost *= MovementController.FlightModule._decelerationEnergyCost * -1f;
        }
        */

        float tempCost = energyCost - _previousSpeedCost;

        if (unit.Energy < tempCost)
        {
            _speedSlider.SetValueWithoutNotify(_previousSpeedBarVal);

            energyCost = _previousSpeedCost;
            MovementController.SetAccelerationLevel((int)_speedSlider.value);
        }

        _previousSpeedCost = energyCost;

        unit.Energy -= tempCost;

        List<Vector3> posList = new List<Vector3>();

        posList.Add(unit.transform.position);

        Vector3 pos = unit.transform.position + unit.transform.rotation * Vector3.forward * MovementController.Speed * 5;

        posList.Add(pos);

        unit.VisualizePath(posList);

        MovementController.SetTargetPos(pos, pos);

        _previousSpeedBarVal = _speedSlider.value;

        _speedText.text = $"{MovementController.Speed}/mps";
        _energyText.text = $"{unit.Energy} / {unit.MaxEnergy}";

        Debug.Log("!!");
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

/*
    public void AppllyTurnEnergy()
    {
        _unitInputManager.SelectedUnit.Energy = _turnEnergy;
    }
*/


}
