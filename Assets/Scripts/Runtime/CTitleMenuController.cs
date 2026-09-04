using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTitleMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _gameClearPenal;

    private CGameProgressManager _gameProgressManager;

    private void Start()
    {
        _gameProgressManager = CGameProgressManager.Instance;
        if(_gameProgressManager.Level >= 14)
        {
            _gameClearPenal.SetActive(true);
        }
    }

    public void StartNewGame()
    {
        if (_gameProgressManager != null)
        {
            _gameProgressManager.ResetGameProgress();
        }
        else
        {
            Debug.LogWarning("Game Progress Manager instance is not available.");
        }
    }
}
