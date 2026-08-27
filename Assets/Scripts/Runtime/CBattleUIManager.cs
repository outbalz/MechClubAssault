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
    [SerializeField] private CTurnStateManager _turnStateManager;

    [Space]
    [Header("UI Element")]
    [SerializeField] private GameObject _playerTurnUI;

    [Space]
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private Image _speedFill;
    [SerializeField] private Slider _speedSlider;

    [Space]
    [SerializeField] private TMP_Text _energyText;
    [SerializeField] private TMP_Text _energyMaxText;
    [SerializeField] private Image _energyFill;

    [Space]
    [SerializeField] private Toggle[] _WeaponToggles;

    [Space]
    [SerializeField] private Toggle _readyToggle;
    #endregion

    #region private var
    //private float _turnEnergy;
    //private float _previousSpeedBarVal;
    //private float _previousSpeedCost;
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

        if (_turnStateManager == null)
        {
            if (TryGetComponent<CTurnStateManager>(out _turnStateManager) == false)
            {
                Debug.LogWarning("Missing CTurnStateManager");
                return;
            }
        }
        if (_playerTurnUI == null)
        {
            Debug.LogWarning("Missing _playerTurnUI");
        }

        if(_speedText == null || _energyText == null || _energyMaxText == null)
        {
            Debug.LogWarning("Missing text element");
        }


        if(_speedFill == null || _energyFill == null )
        {
            Debug.LogWarning("Missing Image element");
        }


    }



    public void TurnInitSelectedUnitUi()
    {
        if(_unitInputManager.SelectedUnit == null)
        {
            SetPlayerTurnUI(false);
            return;
        }


        //CUnitController unit = _unitInputManager.SelectedUnit;

        SetReadyToggle(true);
        InitWeaponToggle();
        SetAccelerationLevel(true);

    }


    public void SetAccelerationLevel(bool init)
    {
        if (_unitInputManager.SelectedUnit == null)
        {
            return;
        }

        CUnitController unit = _unitInputManager.SelectedUnit;
        CUnitMovementController MovementController = _unitInputManager.SelectedUnit.MovementController;

        float previousSpeedBarVal = MovementController.AccelerationLevel;
        float previousSpeedCost = 0;

        MovementController.SetAccelerationLevel((int)_speedSlider.value);

        if (init)
        {
            previousSpeedCost = 0;
            previousSpeedBarVal = 0;
            _speedSlider.SetValueWithoutNotify(1);
            MovementController.SetAccelerationLevel(1);
        }


        if (previousSpeedBarVal > 0)
        {
            previousSpeedCost = previousSpeedBarVal * MovementController.FlightModule._accelerationEnergyCost;
        }

        else if (previousSpeedBarVal < 0)
        {
            previousSpeedCost = previousSpeedBarVal * MovementController.FlightModule._decelerationEnergyCost * -1f;
        }




        //_unitInputManager.SelectedUnit.GetSpeed();

        if (MovementController.Speed < 0)
        {
            //_speedSlider.SetValueWithoutNotify(-2 - MovementController.Speed);

            MovementController.SetAccelerationLevel((int)_speedSlider.value +1);
            _speedSlider.value++;
            return;
            //MovementController.SetAccelerationLevel((int)_speedSlider.value);

        }

        if (MovementController.Speed > MovementController.FlightModule._maxSpeed)
        {
            //float overflow = MovementController.Speed - MovementController.FlightModule._maxSpeed;

            //_speedSlider.SetValueWithoutNotify(3 - overflow);
            MovementController.SetAccelerationLevel((int)_speedSlider.value -1);
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

        float tempCost = energyCost - previousSpeedCost;

        if (unit.Energy < tempCost)
        {
            //energyCost = previousSpeedCost;
            MovementController.SetAccelerationLevel((int)previousSpeedBarVal);
            _speedSlider.value = previousSpeedBarVal;
            return;
        }

        //_previousSpeedCost = energyCost;

        unit.Energy -= tempCost;

        List<Vector3> posList = new List<Vector3>();

        posList.Add(unit.transform.position);

        Vector3 pos = unit.transform.position + unit.transform.rotation * Vector3.forward * MovementController.Speed * 5;

        posList.Add(pos);

        unit.VisualizePath(posList);

        MovementController.SetTargetPos(pos, pos);

        //_previousSpeedBarVal = _speedSlider.value;

        _speedText.text = $"{MovementController.Speed:00}";
        _speedFill.fillAmount = MovementController.Speed / MovementController.FlightModule._maxSpeed;

        _energyText.text = $"{unit.Energy:00}";
        _energyMaxText.text = $"{unit.MaxEnergy:00}";
        _energyFill.fillAmount = unit.Energy / unit.MaxEnergy;

        //Debug.Log("!!");
    }

    public void SetWeaponEnable(int i)
    {
        if (_unitInputManager.SelectedUnit == null)
        {
            return;
        }

        bool Enable = _WeaponToggles[i].isOn;

        CUnitController unit = _unitInputManager.SelectedUnit;
        CUnitWeaponContorller weaponContorller = _unitInputManager.SelectedUnit.WeaponContorller;

        bool previousEnable;
        float WeaponCost;

        if(i  == 0)
        {
            previousEnable = weaponContorller.WeaponEnableL;
            WeaponCost = weaponContorller.WeaponL._weaponEnegyCost;
            WeaponCost *= previousEnable ? -1 : 1;
        }
        else
        {
            previousEnable = weaponContorller.WeaponEnableR;
            WeaponCost = weaponContorller.WeaponR._weaponEnegyCost;
            WeaponCost *= previousEnable ? -1 : 1;
        }

        if(unit.Energy < WeaponCost)
        {
            _WeaponToggles[i].SetIsOnWithoutNotify(false);
            WeaponCost = 0;
            Enable = false;
        }

        unit.Energy -= WeaponCost;
        weaponContorller.SetWeaponEnable((i==0)? true : false, Enable);

        _energyText.text = $"{unit.Energy:00}";
        _energyMaxText.text = $"{unit.MaxEnergy:00}";
        _energyFill.fillAmount = unit.Energy / unit.MaxEnergy;
    }
    
    private void InitWeaponToggle()
    {
        if (_unitInputManager.SelectedUnit == null)
        {
            return;
        }

        CUnitController unit = _unitInputManager.SelectedUnit;
        CUnitWeaponContorller weaponContorller = _unitInputManager.SelectedUnit.WeaponContorller;

        if (weaponContorller.WeaponEnableL)
        {
            if (unit.Energy >= weaponContorller.WeaponL._weaponEnegyCost)
            {
                unit.Energy -= weaponContorller.WeaponL._weaponEnegyCost;
            }
            else
            {
                weaponContorller.SetWeaponEnable(true, false);
            }
        }

        if (weaponContorller.WeaponEnableR)
        {
            if (unit.Energy >= weaponContorller.WeaponR._weaponEnegyCost)
            {
                unit.Energy -= weaponContorller.WeaponR._weaponEnegyCost;
            }
            else
            {
                weaponContorller.SetWeaponEnable(false, false);
            }
        }

        _WeaponToggles[0].SetIsOnWithoutNotify(weaponContorller.WeaponEnableL);
        _WeaponToggles[1].SetIsOnWithoutNotify(weaponContorller.WeaponEnableR);
    }

    public void SetPlayerTurnUI(bool enable)
    {
        _playerTurnUI.SetActive(enable);
    }

    public void SetReadyToggle(bool init)
    {
        if (_unitInputManager.SelectedUnit == null)
        {
            return;
        }

        CUnitController unit = _unitInputManager.SelectedUnit;

        if (init)
        {
            _readyToggle.SetIsOnWithoutNotify(false);
            unit.IsReady = false;
            return;
        }

        bool isReady = _readyToggle.isOn;

        unit.IsReady = isReady;

        _turnStateManager.SetReadyCount(isReady);
    }


/*
    public void AppllyTurnEnergy()
    {
        _unitInputManager.SelectedUnit.Energy = _turnEnergy;
    }
*/


}
