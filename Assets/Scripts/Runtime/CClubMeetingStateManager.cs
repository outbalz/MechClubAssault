using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum EClubMeetingState
{
    ActivitySelection,
    Management,
    Shop,
    Recruit
}

public class CClubMeetingStateManager : MonoBehaviour
{
    #region inspector
    [Header("Panel")]
    [SerializeField] private GameObject _ActivityPanel;
    [SerializeField] private GameObject _managementPanel;
    [SerializeField] private Transform _managementPanelLayoutTr;
    [SerializeField] private Transform _inventorySlotLayoutTr;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _recruitPanel;
    [SerializeField] private GameObject _closeButton;

    [Space]
    [Header("shop item")]
    [SerializeField] private CanvasGroup[] _shopItemCanvas;
    [SerializeField] private TMP_Text[] _shopItemText;
    [SerializeField] private TMP_Text[] _shopItemPriceText;
    [SerializeField] private TMP_Text[] _shopItemDescriptionText;

    [SerializeField] private Image[] _shopItemIcon;


    [Space]
    [Header("Text")]
    [SerializeField] private TMP_Text _fundText;
    [SerializeField] private TMP_Text _reputationText;
    [SerializeField] private TMP_Text _rerollText;
    [SerializeField] private TMP_Text _recruitChanceText;
    [SerializeField] private TMP_Text _recruitLogText;

    [Space]
    [Header("Prefab")]
    [SerializeField] private GameObject _clubMemberPanelPrefab;
    [SerializeField] private GameObject _itemSlotPrefab;
    #endregion

    #region private var
    private CGameProgressManager _gameProgressManager;

    private EClubMeetingState _currentState;

    private IItemable[] _shopItems;

    private int _rerollPrice = 1;

    private CItemSlotController[] _inventorySlot = new CItemSlotController[24];
    #endregion


    private void Awake()
    {
        #region debug
        if (_managementPanel == null)
        {
            Debug.LogWarning("Management Panel is not assigned in the inspector.");
        }

        if (_shopPanel == null)
        {
            Debug.LogWarning("Shop Panel is not assigned in the inspector.");
        }

        if (_recruitPanel == null)
        {
            Debug.LogWarning("Recruit Panel is not assigned in the inspector.");
        }

        if (_fundText == null)
        {
            Debug.LogWarning("Fund Text is not assigned in the inspector.");
        }

        if (_reputationText == null)
        {
            Debug.LogWarning("Reputation Text is not assigned in the inspector.");
        }

        if (_clubMemberPanelPrefab == null)
        {
            Debug.LogWarning("Club Member Panel Prefab is not assigned in the inspector.");
        }

        if (_managementPanelLayoutTr == null)
        {
            Debug.LogWarning("Management Panel Content Transform is not assigned in the inspector.");
        }

        if (_closeButton == null)
        {
            Debug.LogWarning("Close Button is not assigned in the inspector.");
        }

        if (_itemSlotPrefab == null)
        {
            Debug.LogWarning("_itemSlotPrefab is not assigned in the inspector.");
        }

        if(_inventorySlotLayoutTr == null)
        {
            Debug.LogWarning("_inventorySlotLayoutTr is not assigned in the inspector.");
        }
        #endregion
    }

    private void Start()
    {
        _gameProgressManager = CGameProgressManager.Instance;

        _rerollPrice = 1;
        _currentState = EClubMeetingState.ActivitySelection;

        UpdateFundText();
        InitializeClupMember();
        InitializeShopItems();
        InitializeInventorySlot();
    }

    private void InitializeClupMember()
    {
        List<CClubMember> clubMembers = _gameProgressManager.ClubMembers;

        for (int i = 0; i < clubMembers.Count; i++)
        {
            GameObject memberPanel = Instantiate(_clubMemberPanelPrefab, _managementPanelLayoutTr);
            memberPanel.GetComponent<CChararcterPanelContorller>().InitializePanel(clubMembers[i]);
        }
    } 

    private void InitializeInventorySlot()
    {
        for (int i = 0; i < _inventorySlot.Length; i++)
        {
            GameObject slot = Instantiate(_itemSlotPrefab, _inventorySlotLayoutTr);
            CItemSlotController itemSlotController = slot.GetComponent<CItemSlotController>();

            _inventorySlot[i] = itemSlotController;

            if(_gameProgressManager.Inventory.Count > i)
            {
                itemSlotController.InitializeSlot(null, _gameProgressManager.Inventory[i]);
            }

            else
            {
                itemSlotController.InitializeSlot(null, null);
            }

        }
    }

    private void UpdateInventorySlot()
    {
        for (int i = 0; i < _inventorySlot.Length; i++)
        {
            CItemSlotController itemSlotController = _inventorySlot[i];

            if (_gameProgressManager.Inventory.Count > i)
            {
                itemSlotController.InitializeSlot(null, _gameProgressManager.Inventory[i]);
            }

            else
            {
                itemSlotController.InitializeSlot(null, null);
            }
        }
    }

    private void InitializeShopItems()
    {
        _shopItems = new IItemable[]
        {
            _gameProgressManager.SODB.GetRandomModule(),
            _gameProgressManager.SODB.GetRandomModule(),
            _gameProgressManager.SODB.GetRandomModule()
        };

        for (int i = 0; i < _shopItems.Length; i++)
        {
            _shopItemText[i].text = _shopItems[i].ModuleName;
            _shopItemPriceText[i].text = $"{_shopItems[i].Price}";
            _shopItemDescriptionText[i].text = _shopItems[i].Description;
            _shopItemIcon[i].sprite = _shopItems[i].Icon;
            _shopItemCanvas[i].alpha = 1f;
            _shopItemCanvas[i].interactable = true; 
        }
    }

    public void UpdateFundText()
    {
        if (_fundText != null)
        {
            _fundText.text = _gameProgressManager.FundToString();
        }

        if (_reputationText != null)
        {
            _reputationText.text = $"{_gameProgressManager.Reputation}";
        }

        if(_rerollText != null)
        {
            _rerollText.text = $"리롤 {_rerollPrice}만원";
        }
    }

    public void ChangeState(EClubMeetingState newState)
    {
        _currentState = newState;
        _ActivityPanel.SetActive(newState == EClubMeetingState.ActivitySelection);
        _managementPanel.SetActive(newState == EClubMeetingState.Management);
        _shopPanel.SetActive(newState == EClubMeetingState.Shop);
        _recruitPanel.SetActive(newState == EClubMeetingState.Recruit);
        _closeButton.SetActive(newState != EClubMeetingState.ActivitySelection);
    }

    public void OnCloseButtonClicked()
    {
        ChangeState(EClubMeetingState.ActivitySelection);
    }

    public void OnManagementButtonClicked()
    {
        ChangeState(EClubMeetingState.Management);
    }

    public void OnShopButtonClicked()
    {
        ChangeState(EClubMeetingState.Shop);
    }

    public void OnRecruitButtonClicked()
    {
        ChangeState(EClubMeetingState.Recruit);
    }

    public void OnShopItemClicked(int index)
    {
        if (index < 0 || index >= _shopItems.Length)
        {
            Debug.LogWarning("Invalid shop item index.");
            return;
        }

        IItemable selectedItem = _shopItems[index];

        if (_gameProgressManager.Fund < selectedItem.Price)
        {
            Debug.LogWarning("Not enough funds to purchase this item.");
            return;
        }

        if (_gameProgressManager.AddItemToInventory(selectedItem))
        {
            _shopItemCanvas[index].alpha = 0.3f; // Make the purchased item semi-transparent
            _shopItemCanvas[index].interactable = false; // Disable interaction with the purchased item
            _gameProgressManager.Fund -= selectedItem.Price;
            UpdateFundText();
            UpdateInventorySlot();
            Debug.Log($"Purchased {selectedItem.ModuleName} for {selectedItem.Price}.");
        }
        else
        {
            Debug.LogWarning("Not enough space in inventory to add this item.");
        }
    }

    public void OnRerollByFundButtonClicked()
    {
        if (_gameProgressManager.Fund < _rerollPrice)
        {
            Debug.LogWarning("Not enough funds to reroll shop items.");
            return;
        }

        _gameProgressManager.Fund -= _rerollPrice;
        _rerollPrice++;
        UpdateFundText();
        InitializeShopItems();
    }

    public void OnRerollByReputationButtonClicked()
    {
        if (_gameProgressManager.Reputation < 5)
        {
            Debug.LogWarning("Not enough reputation to reroll shop items.");
            return;
        }

        _gameProgressManager.Reputation -= 5;
        UpdateFundText();
        InitializeShopItems();
    }

    public void OnRecruitNewMemberButtonClicked(CanvasGroup canvasGroup)
    {
        if(_gameProgressManager.Reputation < 5)
        {
            Debug.LogWarning("Not enough reputation to Recruit New Member");
            return;
        }

        _gameProgressManager.Reputation -= 5;
        UpdateFundText();

        int ranNum = UnityEngine.Random.Range(1, 101);

        if (ranNum > _gameProgressManager.RecruitChance)
        {
            int ranTextNum = UnityEngine.Random.Range(0, 4);

            switch (ranTextNum)
            {
                case 0:
                    _recruitLogText.text += $"\n열심히 노력했지만 아무도 관심을 주지 않았습니다. \n(성공확률:{_gameProgressManager.RecruitChance}%)";
                    break;
                case 1:
                    _recruitLogText.text += $"\n입부신청 희망자를 찾는데 실패하였습니다. \n(성공확률:{_gameProgressManager.RecruitChance}%)";
                    break;
                case 2:
                    _recruitLogText.text += $"\n동아리에 들어오겠다고 말한 친구가 끝내 오지 않았습니다. \n(성공확률:{_gameProgressManager.RecruitChance}%)";
                    break;
                case 3:
                    _recruitLogText.text += $"\n관심을 보인 사람은 있었지만 입부신청은 없었습니다. \n(성공확률:{_gameProgressManager.RecruitChance}%)";
                    break;
                default:
                    break;
            }

            if(_recruitLogText.rectTransform.sizeDelta.y < _recruitLogText.preferredHeight)
            {
                switch (ranTextNum)
                {
                    case 0:
                        _recruitLogText.text = $"열심히 노력했지만 아무도 관심을 주지 않았습니다. \n(성공확률:{_gameProgressManager.RecruitChance}%)";
                        break;
                    case 1:
                        _recruitLogText.text = $"입부신청 희망자를 찾는데 실패하였습니다. \n(성공확률:{_gameProgressManager.RecruitChance}%)";
                        break;
                    case 2:
                        _recruitLogText.text = $"동아리에 들어오겠다고 말한 친구가 끝내 오지 않았습니다. \n(성공확률:{_gameProgressManager.RecruitChance}%)";
                        break;
                    case 3:
                        _recruitLogText.text = $"관심을 보인 사람은 있었지만 입부신청은 없었습니다. \n(성공확률:{_gameProgressManager.RecruitChance}%)";
                        break;
                    default:
                        break;
                }
            }

            _gameProgressManager.RecruitChance *= 2;

            _recruitChanceText.text = $"{_gameProgressManager.RecruitChance}%";

        }

        else
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0.3f;

            CClubMember newMember = new CClubMember(CUtil.GetRandomName(),null,null,null,null,null);

            _recruitLogText.text += $"\n모집 성공! {newMember.Name}이(가) 메카 동아리에 들어왔습니다! \n(확률:{_gameProgressManager.RecruitChance}%)";
            
            if(_recruitLogText.rectTransform.sizeDelta.y < _recruitLogText.preferredHeight)
            {
                _recruitLogText.text = $"모집 성공! {newMember.Name}이(가) 메카 동아리에 들어왔습니다! \n(확률:{_gameProgressManager.RecruitChance}%)";
            }

            _gameProgressManager.ClubMembers.Add(newMember);

            GameObject newMemberPanel = Instantiate(_clubMemberPanelPrefab, _managementPanelLayoutTr);
            newMemberPanel.GetComponent<CChararcterPanelContorller>().InitializePanel(newMember);

            _gameProgressManager.RecruitChance = 1;

            _recruitChanceText.text = $"{_gameProgressManager.RecruitChance}%";
        }

    }


    /*
    public bool CheckScene()
    {
        for (int i = 0; i < _gameProgressManager.ClubMembers.Count; i++)
        {
            CClubMember clubMember = _gameProgressManager.ClubMembers[i];

            bool moduleCheck = true;

            if(clubMember.GeneratorModule == null || clubMember.ShieldModule == null || clubMember.FlightModule == null|| clubMember.WeaponModuleL == null || clubMember.WeaponModuleR == null)
            {
                moduleCheck = false;
                continue;
            }

            if (moduleCheck)
            {
                return true;
            }
        }

        Debug.Log("No valid clubMember");
        return false;
    }*/
}
