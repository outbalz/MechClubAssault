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
    [SerializeField] private Slider _shieldSlider;

    [Space]
    [SerializeField] private Toggle[] _WeaponToggles;

    [Space]
    [SerializeField] private Toggle _readyToggle;

    [Space]
    [SerializeField] private GameObject _resultUI;
    [SerializeField] private TMP_Text _resultText;

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


        CUnitController unit = _unitInputManager.SelectedUnit;
        unit.IsInitedForTurn = true;

        //SetValToSelectedUnitVal(unit);

        SetShieldRegen(true);
        SetReadyToggle(true);
        InitWeaponToggle();
        SetAccelerationLevel(true);
    }

    private void UpdateEnergy(CUnitController unit)
    {
        _energyText.text = $"{unit.Energy:00}";
        _energyMaxText.text = $"{unit.MaxEnergy:00}";
        _energyFill.fillAmount = unit.Energy / unit.MaxEnergy;
    }

    private void UpdateSpeed(CUnitController unit, CUnitMovementController MovementController)
    {
        _speedText.text = $"{MovementController.Speed:00}";
        _speedFill.fillAmount = MovementController.Speed / MovementController.FlightModule.MaxSpeed;
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
            previousSpeedCost = previousSpeedBarVal * MovementController.FlightModule.AccelerationEnergyCost;
        }

        else if (previousSpeedBarVal < 0)
        {
            previousSpeedCost = previousSpeedBarVal * MovementController.FlightModule.DecelerationEnergyCost * -1f;
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

        if (MovementController.Speed > MovementController.FlightModule.MaxSpeed)
        {
            //float overflow = MovementController.Speed - MovementController.FlightModule.MaxSpeed;

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
            energyCost = _speedSlider.value * MovementController.FlightModule.AccelerationEnergyCost;
        }

        else if (_speedSlider.value < 0) 
        { 
            energyCost = _speedSlider.value * MovementController.FlightModule.DecelerationEnergyCost * -1f;
        }

        /*
        else if(_previousSpeedBarVal > 0)
        {
            energyCost *= MovementController.FlightModule.AccelerationEnergyCost;
        }

        else if(_previousSpeedBarVal < 0)
        {
            energyCost *= MovementController.FlightModule.DecelerationEnergyCost * -1f;
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

        //_speedText.text = $"{MovementController.Speed:00}";
        //_speedFill.fillAmount = MovementController.Speed / MovementController.FlightModule.MaxSpeed;

        UpdateSpeed(unit, MovementController);
        UpdateEnergy(unit);

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
            WeaponCost = weaponContorller.WeaponL.WeaponEnegyCost;
            WeaponCost *= previousEnable ? -1 : 1;
        }
        else
        {
            previousEnable = weaponContorller.WeaponEnableR;
            WeaponCost = weaponContorller.WeaponR.WeaponEnegyCost;
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

        UpdateEnergy(unit);
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
            if (unit.Energy >= weaponContorller.WeaponL.WeaponEnegyCost)
            {
                unit.Energy -= weaponContorller.WeaponL.WeaponEnegyCost;
            }
            else
            {
                weaponContorller.SetWeaponEnable(true, false);
            }
        }

        if (weaponContorller.WeaponEnableR)
        {
            if (unit.Energy >= weaponContorller.WeaponR.WeaponEnegyCost)
            {
                unit.Energy -= weaponContorller.WeaponR.WeaponEnegyCost;
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

        if (isReady)
        {
            CUnitController nextUnit = _turnStateManager.GetNextUnreadyUnit();

            if (nextUnit != null)
            {
                _unitInputManager.SetSelectedUint(nextUnit);
            }

        }
    }

    public void SetShieldRegen(bool init)
    {
        if (_unitInputManager.SelectedUnit == null)
        {
            return;
        }

        CUnitController unit = _unitInputManager.SelectedUnit;

        if (init)
        {
            unit.ShieldRegenLevel = 0;
            _shieldSlider.SetValueWithoutNotify(0);
            return;
        }


        int shieldRegenLevel = (int)_shieldSlider.value;

        float shieldRegenCost = unit.ShieldModule.ShieldRegenCost;

        shieldRegenCost *= shieldRegenLevel - unit.ShieldRegenLevel;

        if(shieldRegenCost > unit.Energy)
        {
            _shieldSlider.value = unit.ShieldRegenLevel;
            return;
        }

        unit.Energy -= shieldRegenCost;

        unit.ShieldRegenLevel = shieldRegenLevel;

        unit.Shield = unit.PreviousShield + unit.ShieldRegenLevel * unit.ShieldModule.ShieldRegen;

        if(unit.Shield >= unit.ShieldModule.MaxShield + unit.ShieldModule.ShieldRegen)
        {
            _shieldSlider.value--;
            return;
        }

        //unit.SetShieldBar();

        UpdateEnergy(unit);

    }

    public void BattleWin()
    {
        _resultUI.SetActive(true);
        _resultText.text = "승리!!";
    }

    public void BattleLost()
    {
        _resultUI.SetActive(true);
        _resultText.text = "패배...";
    }

    public void SetValToSelectedUnitVal(CUnitController unit)
    {
        if(unit.IsInitedForTurn == false)
        {
            TurnInitSelectedUnitUi();
            return;
        }

        /*
        _speedSlider.value = (unit.MovementController.AccelerationLevel);
        _WeaponToggles[0].isOn = (unit.WeaponContorller.WeaponEnableL);
        _WeaponToggles[1].isOn = (unit.WeaponContorller.WeaponEnableR);
        _shieldSlider.value =(unit.ShieldRegenLevel);
        _readyToggle.SetIsOnWithoutNotify(unit.IsReady);

        unit.SetShieldBar();
        UpdateSpeed(unit, unit.MovementController);
        UpdateEnergy(unit);
        /**/

        _speedSlider.SetValueWithoutNotify(unit.MovementController.AccelerationLevel);
        _WeaponToggles[0].SetIsOnWithoutNotify(unit.WeaponContorller.WeaponEnableL);
        _WeaponToggles[1].SetIsOnWithoutNotify(unit.WeaponContorller.WeaponEnableR);
        _shieldSlider.SetValueWithoutNotify(unit.ShieldRegenLevel);
        _readyToggle.SetIsOnWithoutNotify(unit.IsReady);

        //unit.SetShieldBar();
        UpdateSpeed(unit, unit.MovementController);
        UpdateEnergy(unit);
        
    }
}
