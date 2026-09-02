using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CClubMeetingStateManager : MonoBehaviour
{
    #region inspector
    [Header("Panel")]
    [SerializeField] private GameObject _ActivityPanel;
    [SerializeField] private GameObject _managementPanel;
    [SerializeField] private Transform _managementPanelContentTr;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _recruitPanel;

    [Space]
    [Header("Text")]
    [SerializeField] private TMP_Text _fundText;
    [SerializeField] private TMP_Text _reputationText;

    [Space]
    [Header("Prefab")]
    [SerializeField] private GameObject _clubMemberPanelPrefab;
    #endregion

    #region private var
    private CGameProgressManager _gameProgressManager;
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
        #endregion

    }

    private void Start()
    {
        _gameProgressManager = CGameProgressManager.Instance;
        UpdateFundText();
        InitializeClupMember();
    }

    private void InitializeClupMember()
    {
        List<CClubMember> clubMembers = _gameProgressManager.ClubMembers;

        for (int i = 0; i < clubMembers.Count; i++)
        {
            GameObject memberPanel = Instantiate(_clubMemberPanelPrefab, _managementPanelContentTr);
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
    }


}
