using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CGameOptionManager : MonoBehaviour
{
    #region inspector
    [SerializeField] private ScriptableObjectSceneData _tilteScene;
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private GameObject _optionWindow;
    [SerializeField] private GameObject _bockerPanel;
    [SerializeField] private TMP_Dropdown _resolutionDropDown;
    #endregion

    #region private var
    private static CGameOptionManager _instance;
    private bool _isOptitonMenuOpen = false;
    private float _previousTimescale;
    #endregion

    #region getter
    public static CGameOptionManager Instance { get { return _instance; } }
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

        if(_optionMenu == null || _optionWindow == null || _bockerPanel == null)
        {
            Debug.LogWarning("Missing Option Menu element");
        }

        if(_resolutionDropDown == null)
        {
            Debug.LogWarning("Missing DropDown element");
        }

        _previousTimescale = Time.timeScale;
    }

    private void Start()
    {
        Resolution[] resolutionOptions = Screen.resolutions;
        List<string> resolutionOptionStrings = new List<string>();

        int currentIndex = 0;
        for (int i = 0; i < resolutionOptions.Length; i++)
        {
            resolutionOptionStrings.Add(resolutionOptions[i].width + "x" + resolutionOptions[i].height + "@" + resolutionOptions[i].refreshRateRatio.value);
            if (
                resolutionOptions[i].width == Screen.currentResolution.width && 
                resolutionOptions[i].height == Screen.currentResolution.height && 
                resolutionOptions[i].refreshRateRatio.value == Screen.currentResolution.refreshRateRatio.value
                )
            {
                currentIndex = i;
            }
        }

        _resolutionDropDown.ClearOptions();
        _resolutionDropDown.AddOptions(resolutionOptionStrings);
        _resolutionDropDown.SetValueWithoutNotify(currentIndex);
        _resolutionDropDown.RefreshShownValue();

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

    public void SetResolution(int index)
    {
        Resolution[] resolutionOptions = Screen.resolutions;
        Resolution targetResolution = resolutionOptions[index];

        Screen.SetResolution(targetResolution.width, targetResolution.height, Screen.fullScreenMode, targetResolution.refreshRateRatio);
    }

    public void SetFullscreenMode(int index)
    {

        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                break;
        }

    }

    private void ToggleOptitonMenu()
    {
        _isOptitonMenuOpen = !_isOptitonMenuOpen;

        if (_isOptitonMenuOpen)
        {
            _previousTimescale = Time.timeScale;
            Time.timeScale = 0;

            _optionMenu.SetActive(true);
            _bockerPanel.SetActive(true);
            CSoundManager.Instance.PlaySelectSound();
        }

        else
        {
            Time.timeScale = _previousTimescale;
            _optionMenu.SetActive(false);
            _optionWindow.SetActive(false);
            _bockerPanel.SetActive(false);
            CSoundManager.Instance.PlayCursorSound();
        }

    }

    public void ExitGame()
    {
        CSaveAndLoadManager.SaveGame();
        Application.Quit();
    }

    public void CloseOptionMenu()
    {
        _isOptitonMenuOpen = false;

        Time.timeScale = _previousTimescale;
        _optionMenu.SetActive(false);
        _optionWindow.SetActive(false);
        _bockerPanel.SetActive(false);

        CSoundManager.Instance.PlayCursorSound();
    }

    public void OpenOptitonWindow()
    {
        _optionMenu.SetActive(false);
        _optionWindow.SetActive(true);
        _bockerPanel.SetActive(true);
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
