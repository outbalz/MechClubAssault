using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTitleMenuController : MonoBehaviour
{
    #region inspector
    [SerializeField] private GameObject _gameClearPenal;
    [SerializeField] private GameObject _loadGameButton;
    #endregion

    #region private var
    private CGameProgressManager _gameProgressManager;
    #endregion


    private void Start()
    {
        _gameProgressManager = CGameProgressManager.Instance;

        if (_gameProgressManager.Level >= 14)
        {
            _gameClearPenal.SetActive(true);
            _loadGameButton.SetActive(false);
        }

        else if(_gameProgressManager.Level < 1)
        {
            _loadGameButton.SetActive(false);
        }

        else
        {
            _loadGameButton.SetActive(true);
        }

    }

    public void StartNewGame()
    {
        if (_gameProgressManager != null)
        {
            _gameProgressManager.ResetGameProgress();
            CSoundManager.Instance.PlaySelectSound();
        }
        else
        {
            Debug.LogWarning("Game Progress Manager instance is not available.");
        }
    }

    public void OpenOptitonWindow()
    {
        CGameOptitonManager.Instance.OpenOptitonWindow();
    }
}
