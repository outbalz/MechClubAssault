using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Transform _managementPanelContentTr;
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

    [Space]
    [Header("Prefab")]
    [SerializeField] private GameObject _clubMemberPanelPrefab;
    #endregion

    #region private var
    private CGameProgressManager _gameProgressManager;

    private EClubMeetingState _currentState;

    private CItemData[] _shopItems;

    private int _rerollPrice = 1;
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

        if (_managementPanelContentTr == null)
        {
            Debug.LogWarning("Management Panel Content Transform is not assigned in the inspector.");
        }

        if (_closeButton == null)
        {
            Debug.LogWarning("Close Button is not assigned in the inspector.");
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
    }

    private void InitializeClupMember()
    {
        List<CClubMember> clubMembers = _gameProgressManager.ClubMembers;

        for (int i = 0; i < clubMembers.Count; i++)
        {
            GameObject memberPanel = Instantiate(_clubMemberPanelPrefab, _managementPanelContentTr);
        }
    } 

    private void InitializeShopItems()
    {
        _shopItems = new CItemData[]
        {
            new CItemData(_gameProgressManager.SODB.GetRandomModule()),
            new CItemData(_gameProgressManager.SODB.GetRandomModule()),
            new CItemData(_gameProgressManager.SODB.GetRandomModule())
        };

        for (int i = 0; i < _shopItems.Length; i++)
        {
            _shopItemText[i].text = _shopItems[i].Name;
            _shopItemPriceText[i].text = $"{_shopItems[i].Price}";
            _shopItemDescriptionText[i].text = _shopItems[i].Item.Description;
            _shopItemIcon[i].sprite = _shopItems[i].Item.Icon;
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

        CItemData selectedItem = _shopItems[index];

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
            Debug.Log($"Purchased {selectedItem.Name} for {selectedItem.Price}.");
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

}
