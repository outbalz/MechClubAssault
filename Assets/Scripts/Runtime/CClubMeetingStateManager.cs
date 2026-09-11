using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _recruitPanel;
    [SerializeField] private CanvasGroup _recruitButton;
    [SerializeField] private GameObject _closeButton;

    [Space]
    [Header("Layout")]
    [SerializeField] private Transform _managementPanelLayoutTr;
    [SerializeField] private Transform _inventorySlotLayoutTr;
    [SerializeField] private Transform _unitPreviewLayoutTr;
    [SerializeField] private Transform _messageLayoutTr;

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
    [SerializeField] private GameObject _unitPreviewPrefab;
    [SerializeField] private GameObject _messagePrefab;
    #endregion

    #region private var
    private CGameProgressManager _gameProgressManager;
    private CSoundManager _soundManager;

    private EClubMeetingState _currentState;

    //private IItemable[] _shopItems;

    //private int _gameProgressManager.RerollPrice = 1;

    private CItemSlotController[] _inventorySlot = new CItemSlotController[24];

    private static CClubMeetingStateManager _instance;
    #endregion

    public static CClubMeetingStateManager Instance {  get { return _instance; } }

    private void Awake()
    {
        #region debug
        if (_managementPanel == null || _shopPanel == null || _recruitPanel == null)
        {
            Debug.LogWarning("Missing Panel element");
        }

        if (_fundText == null || _reputationText == null)
        {
            Debug.LogWarning("Missing Text element");
        }

        if (_managementPanelLayoutTr == null || _inventorySlotLayoutTr == null || _unitPreviewLayoutTr == null || _messageLayoutTr == null)
        {
            Debug.LogWarning("Missing LayoutTr element");
        }

        if (_closeButton == null || _recruitButton == null)
        {
            Debug.LogWarning("Missing Button element");
        }

        if (_clubMemberPanelPrefab == null || _itemSlotPrefab == null ||_unitPreviewPrefab == null || _messagePrefab == null) 
        {
            Debug.LogWarning("Missing prefab element");
        }
        #endregion

        if(_instance == null)
        {
            _instance = this;
        }
 
    }

    private void Start()
    {
        _gameProgressManager = CGameProgressManager.Instance;
        _soundManager = CSoundManager.Instance;

        _currentState = EClubMeetingState.ActivitySelection;

        //_gameProgressManager.ApplyRandomState();

        UpdateFundText();
        InitializeClupMember();
        InitializeShopItems();
        InitializeInventorySlot();

        if (_gameProgressManager.IsLevelInited == false)
        {
            _gameProgressManager.IsLevelInited = true;
            _gameProgressManager.HasRecruitedThisLevel = false;
            RerollShopItems();
        }

        if(_gameProgressManager.HasRecruitedThisLevel == true)
        {
            _recruitButton.alpha = 0.3f;
            _recruitButton.interactable = false;
        }

        _recruitChanceText.text = $"{_gameProgressManager.RecruitChance}%";
    }

    private void InitializeClupMember()
    {
        List<CClubMember> clubMembers = _gameProgressManager.ClubMembers;

        for (int i = 0; i < clubMembers.Count; i++)
        {
            GameObject memberPanel = Instantiate(_clubMemberPanelPrefab, _managementPanelLayoutTr);
            GameObject memberPreview = Instantiate(_unitPreviewPrefab, _unitPreviewLayoutTr);
            memberPreview.transform.position = new Vector3(0, 0, -2 *i);
            memberPanel.GetComponent<CChararcterPanelContorller>().InitializePanel(clubMembers[i], memberPreview.GetComponent<CUnitPreviewController>());
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

        for (int i = 0; i < _gameProgressManager.ShopItems.Length; i++)
        {
            if (_gameProgressManager.ShopItems[i] == null)
            {
                _shopItemCanvas[i].alpha = 0.3f;
                _shopItemCanvas[i].interactable = false;
                _shopItemText[i].text = "판매됨";
                _shopItemPriceText[i].text = "-";
                _shopItemDescriptionText[i].text = "-";
                continue;
            }

            _shopItemText[i].text = _gameProgressManager.ShopItems[i].ModuleName;
            _shopItemPriceText[i].text = $"{_gameProgressManager.ShopItems[i].Price}";
            _shopItemDescriptionText[i].text = CUtil.GetFormetedDescription(_gameProgressManager.ShopItems[i].Description, false);
            _shopItemIcon[i].sprite = _gameProgressManager.ShopItems[i].Icon;
            _shopItemCanvas[i].alpha = 1f;
            _shopItemCanvas[i].interactable = true; 
        }
    }


    private void RerollShopItems()
    {
        _gameProgressManager.ShopItems = new IItemable[]
        {
            _gameProgressManager.SODB.GetRandomModule(),
            _gameProgressManager.SODB.GetRandomModule(),
            _gameProgressManager.SODB.GetRandomModule()
        };

        for (int i = 0; i < _gameProgressManager.ShopItems.Length; i++)
        {
            _shopItemText[i].text = _gameProgressManager.ShopItems[i].ModuleName;
            _shopItemPriceText[i].text = $"{_gameProgressManager.ShopItems[i].Price}";
            _shopItemDescriptionText[i].text = CUtil.GetFormetedDescription(_gameProgressManager.ShopItems[i].Description, false);
            _shopItemIcon[i].sprite = _gameProgressManager.ShopItems[i].Icon;
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
            _rerollText.text = $"리롤 {_gameProgressManager.RerollPrice}만원";
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
        _soundManager.PlayCursorSound();
    }

    public void OnManagementButtonClicked()
    {
        ChangeState(EClubMeetingState.Management);
        _soundManager.PlaySelectSound();
    }

    public void OnShopButtonClicked()
    {
        ChangeState(EClubMeetingState.Shop);
        _soundManager.PlaySelectSound();
    }

    public void OnRecruitButtonClicked()
    {
        ChangeState(EClubMeetingState.Recruit);
        _soundManager.PlaySelectSound();
    }

    public void OnShopItemClicked(int index)
    {
        if (index < 0 || index >= _gameProgressManager.ShopItems.Length)
        {
            Debug.LogWarning("Invalid shop item index.");
            return;
        }

        IItemable selectedItem = _gameProgressManager.ShopItems[index];

        if (_gameProgressManager.Fund < selectedItem.Price)
        {
            Debug.LogWarning("Not enough funds to purchase this item.");
            ShowMessage("자금이 부족해요!!");
            _soundManager.PlayCancelSound();
            return;
        }

        if (_gameProgressManager.AddItemToInventory(selectedItem))
        {
            _gameProgressManager.ShopItems[index] = null;
            _shopItemCanvas[index].alpha = 0.3f; 
            _shopItemCanvas[index].interactable = false; 
            _gameProgressManager.Fund -= selectedItem.Price;
            UpdateFundText();
            UpdateInventorySlot();
            
            Debug.Log($"Purchased {selectedItem.ModuleName} for {selectedItem.Price}.");
            _soundManager.PlaySelectSound();
        }
        else
        {
            Debug.LogWarning("Not enough space in inventory to add this item.");
            ShowMessage("창고가 가득찼어요!!");
            _soundManager.PlayCancelSound();
        }
    }

    public void OnRerollByFundButtonClicked()
    {
        if (_gameProgressManager.Fund < _gameProgressManager.RerollPrice)
        {
            Debug.LogWarning("Not enough funds to reroll shop items.");
            ShowMessage("자금이 부족해요!!");
            _soundManager.PlayCancelSound();
            return;
        }

        _gameProgressManager.Fund -= _gameProgressManager.RerollPrice;
        _gameProgressManager.RerollPrice++;
        //_gameProgressManager.SetRandomState();
        UpdateFundText();
        RerollShopItems();
        _soundManager.PlayCursorSound();
    }

    public void OnRerollByReputationButtonClicked()
    {
        if (_gameProgressManager.Reputation < 5)
        {
            Debug.LogWarning("Not enough reputation to reroll shop items.");
            ShowMessage("평판이 부족해요!!");
            _soundManager.PlayCancelSound();
            return;
        }

        _gameProgressManager.Reputation -= 5;
        //_gameProgressManager.SetRandomState();
        UpdateFundText();
        RerollShopItems();
        _soundManager.PlayCursorSound();
    }

    public void OnRecruitNewMemberButtonClicked(CanvasGroup canvasGroup)
    {
        if(_gameProgressManager.Reputation < 5)
        {
            Debug.LogWarning("Not enough reputation to Recruit New Member");
            ShowMessage("평판이 부족해요!!");
            _soundManager.PlayCancelSound();
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

            _soundManager.PlayCursorSound();

        }

        else
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0.3f;

            _gameProgressManager.HasRecruitedThisLevel = true;
            CClubMember newMember = new CClubMember(CUtil.GetRandomName(),null,null,null,null,null);

            _recruitLogText.text += $"\n모집 성공! {newMember.Name}이(가) 메카 동아리에 들어왔습니다! \n(확률:{_gameProgressManager.RecruitChance}%)";
            
            if(_recruitLogText.rectTransform.sizeDelta.y < _recruitLogText.preferredHeight)
            {
                _recruitLogText.text = $"모집 성공! {newMember.Name}이(가) 메카 동아리에 들어왔습니다! \n(확률:{_gameProgressManager.RecruitChance}%)";
            }

            _gameProgressManager.ClubMembers.Add(newMember);

            GameObject newMemberPanel = Instantiate(_clubMemberPanelPrefab, _managementPanelLayoutTr);

            GameObject memberPreview = Instantiate(_unitPreviewPrefab, _unitPreviewLayoutTr);
            memberPreview.transform.position = new Vector3(0, 0, -2 * _gameProgressManager.ClubMembers.Count -1);
            newMemberPanel.GetComponent<CChararcterPanelContorller>().InitializePanel(newMember, memberPreview.GetComponent<CUnitPreviewController>());

            _gameProgressManager.RecruitChance = 1;

            _recruitChanceText.text = $"{_gameProgressManager.RecruitChance}%";
            _soundManager.PlaySelectSound();
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

    public void ShowMessage(string messageTxt)
    {
        GameObject message = Instantiate(_messagePrefab,_messageLayoutTr);

        TMP_Text text = message.GetComponentInChildren<TMP_Text>();
        text.text = messageTxt;

        StartCoroutine(Co_messageRoutine(message));
    }

    private IEnumerator Co_messageRoutine(GameObject messageGO)
    {
        CanvasGroup canvasGroup = messageGO.GetComponent<CanvasGroup>();

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime;
            messageGO.transform.position += Vector3.up * 10 * Time.unscaledDeltaTime;
            yield return null;
        }

        Destroy(messageGO);
    }
}
