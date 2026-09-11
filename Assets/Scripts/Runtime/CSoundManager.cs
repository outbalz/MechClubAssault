using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CSoundManager : MonoBehaviour
{
    #region inspector
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private ScriptableObjectSFXData _sfxData;
    #endregion
 
    #region private var
    private static CSoundManager _instance;
    #endregion

    #region getter
    public static CSoundManager Instance {  get { return _instance; } }
    #endregion


    private void Awake()
    {
        if(_audioSource == null)
        {
            if(TryGetComponent<AudioSource>(out _audioSource) == false)
            {
                Debug.LogWarning("Missing AudioSource");
                Destroy(gameObject);
                return;
            }
        }

        if(_sfxData == null)
        {
            Debug.LogWarning("Missing _sfxData");
            Destroy(gameObject);
            return;
        }

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

    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public void PlaySelectSound()
    {
        SetPosToCamera();
        _audioSource.clip = _sfxData.Select;
        _audioSource.Play();
    }

    public void PlayCancelSound()
    {
        SetPosToCamera();
        _audioSource.clip = _sfxData.Cancel;
        _audioSource.Play();
    }

    public void PlayCursorSound()
    {
        SetPosToCamera();
        _audioSource.clip = _sfxData.Cursor;
        _audioSource.Play();
    }

    public void SetMasterVolume(float level)
    {
        _mixer.SetFloat("MasterVolume", level);
    }

    public void SetSFXVolume(float level)
    {
        _mixer.SetFloat("SFXVolume", level);
    }
    
    public void SetBGMVolume(float level)
    {
        _mixer.SetFloat("BGMVolume", level);
    }

    public void SetAmbienceVolume(float level)
    {
        _mixer.SetFloat("AmbienceVolume", level);
    }

    private void SetPosToCamera()
    {
        transform.position = Camera.main.transform.position;
    }
}
