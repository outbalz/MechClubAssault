using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CSeceneManager : MonoBehaviour
{
    public void ReloadBattleScene()
    {
        //for test
        SceneManager.LoadScene(0);
    }

    public void ExitToDescktop()
    {
        //for test
        Application.Quit();
    }
}
