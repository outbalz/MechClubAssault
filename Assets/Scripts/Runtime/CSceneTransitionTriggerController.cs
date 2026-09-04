using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSceneTransitionTrigerController : MonoBehaviour
{

    #region private var
    private CSeceneManager _sceneManager;
    private CGameProgressManager _gameProgressManager;
    #endregion


    private void Start()
    {
        _sceneManager = CSeceneManager.Instance;
        _gameProgressManager = CGameProgressManager.Instance;
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


    public void TriggerSceneTransitionWithCheck(ScriptableObjectSceneData sceneData)
    {
        if (_sceneManager != null)
        {
            if (CheckScene())
            {
                _sceneManager.LoadScene(sceneData, -1f);
            }
        }

        else
        {
            Debug.LogWarning("Scene Manager instance is not available.");
        }
    }

    private bool CheckScene()
    {
        for (int i = 0; i < _gameProgressManager.ClubMembers.Count; i++)
        {
            CClubMember clubMember = _gameProgressManager.ClubMembers[i];

            bool moduleCheck = true;

            if (clubMember.GeneratorModule == null || clubMember.ShieldModule == null || clubMember.FlightModule == null || clubMember.WeaponModuleL == null || clubMember.WeaponModuleR == null)
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
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}