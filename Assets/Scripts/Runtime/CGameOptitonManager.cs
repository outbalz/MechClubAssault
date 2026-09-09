using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameOptitonManager : MonoBehaviour
{
    #region inspector
    [SerializeField] private ScriptableObjectSceneData _tilteScene;
    [SerializeField] private GameObject _optitionMenu;
    [SerializeField] private GameObject _optitionWindow;
    #endregion

    #region private var
    private static CGameOptitonManager _instance;
    private bool _isOptitonMenuOpen = false;
    private float _previousTimescale;
    #endregion

    #region getter
    public static CGameOptitonManager Instance { get { return _instance; } }
    #endregion

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else if (_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _previousTimescale = Time.timeScale;
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (CSeceneManager.CurrentScene != _tilteScene)
            {
                ToggleOptitonMenu();
            }

            else
            {
                CloseOptionMenu();
            }
        }
    }


    private void ToggleOptitonMenu()
    {
        _isOptitonMenuOpen = !_isOptitonMenuOpen;

        if (_isOptitonMenuOpen)
        {
            _previousTimescale = Time.timeScale;
            Time.timeScale = 0;

            _optitionMenu.SetActive(true);
            CSoundManager.Instance.PlaySelectSound();
        }

        else
        {
            Time.timeScale = _previousTimescale;
            _optitionMenu.SetActive(false);
            _optitionWindow.SetActive(false);
            CSoundManager.Instance.PlayCursorSound();
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CloseOptionMenu()
    {
        _isOptitonMenuOpen = false;

        Time.timeScale = _previousTimescale;
        _optitionMenu.SetActive(false);
        _optitionWindow.SetActive(false);

        CSoundManager.Instance.PlayCursorSound();
    }

    public void OpenOptitonWindow()
    {
        _optitionMenu.SetActive(false);
        _optitionWindow.SetActive(true);
        CSoundManager.Instance.PlaySelectSound();
    }

    public void LoadTileSecene()
    {
        CSoundManager.Instance.PlaySelectSound();
        CSeceneManager.Instance.LoadScene(_tilteScene);
    }

    public void SetMasterVolume(float level)
    {
        level = Mathf.Log10(level) * 20f;
        CSoundManager.Instance.SetMasterVolume(level);
        CSoundManager.Instance.PlayCursorSound();
    }


    public void SetSFXVolume(float level)
    {
        level = Mathf.Log10(level) * 20f;
        CSoundManager.Instance.SetSFXVolume(level);
        CSoundManager.Instance.PlayCursorSound();
    }

    public void SetBGMVolume(float level)
    {
        level = Mathf.Log10(level) * 20f;
        CSoundManager.Instance.SetBGMVolume(level);
        CSoundManager.Instance.PlayCursorSound();
    }

    public void SetAmbienceVolume(float level)
    {
        level = Mathf.Log10(level) * 20f;
        CSoundManager.Instance.SetAmbienceVolume(level);
        CSoundManager.Instance.PlayCursorSound();
    }

}
