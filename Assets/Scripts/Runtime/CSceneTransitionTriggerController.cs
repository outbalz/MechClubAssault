using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSceneTransitionTrigerController : MonoBehaviour
{
    private CSeceneManager _sceneManager;

    private void Start()
    {
        _sceneManager = CSeceneManager.Instance;
    }

    public void TriggerSceneTransition(ScriptableObjectSceneData sceneData)
    {
        if (_sceneManager != null)
        {
            _sceneManager.LoadScene(sceneData, -1f);
        }

        else
        {
            Debug.LogWarning("Scene Manager instance is not available.");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
