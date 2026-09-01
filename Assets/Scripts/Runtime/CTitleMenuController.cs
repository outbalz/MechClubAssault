using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTitleMenuController : MonoBehaviour
{
    private CGameProgressManager _gameProgressManager;

    private void Start()
    {
        _gameProgressManager = CGameProgressManager.Instance;
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
