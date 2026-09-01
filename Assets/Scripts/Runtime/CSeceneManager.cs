using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CSeceneManager : MonoBehaviour
{

    [SerializeField] private CSceneTransitionUI _transitionUI;
    [SerializeField] private Slider _loadingBar;


    private static CSeceneManager _instance;
    private bool _isLoading = false;

    public static CSeceneManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if(_loadingBar == null)
        {
            Debug.LogWarning("Loading bar is not assigned");
        }

        _isLoading = false;
    }

    private void Start()
    {
        if (_transitionUI != null)
        {
            _transitionUI.Initialize();
        }

        else
        {
            Debug.LogWarning("Transition UI is not assigned");
        }
    }


    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }


    public void LoadScene(ScriptableObjectSceneData sceneData, float fadeDuration)
    {
        if (sceneData == null)
        {
            Debug.LogWarning("Scene data is null");
            return;
        }
        
        string sceneName = sceneData.SceneName;
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name is null or empty");
            return;
        }

        StartCoroutine(Co_LoadSceneWithTransition(sceneName, fadeDuration));

    }


    private IEnumerator Co_LoadSceneWithTransition(string sceneName, float fadeDuration)
    {

        if (_isLoading)
        {
            Debug.LogWarning("Scene is already loading");
            yield break;
        }

        _isLoading = true;

        Debug.Log(sceneName);

        if (_transitionUI != null)
        {
            yield return _transitionUI.Co_FadeTo(1f, fadeDuration);
        }

        //비동기 씬 로드

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            _loadingBar.value = op.progress;
            yield return null;
        }

        op.allowSceneActivation = true;

        yield return null;

        if (_transitionUI != null)
        {
            yield return _transitionUI.Co_FadeTo(0f, fadeDuration);
        }


        Debug.Log($"{sceneName}");

        _isLoading = false;

    }

    public void LoadScene(ScriptableObjectSceneData sceneData)
    {
        if (sceneData == null)
        {
            Debug.LogWarning("Scene data is null");
            return;
        }
        string sceneName = sceneData.SceneName;
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name is null or empty");
            return;
        }
        StartCoroutine(Co_LoadSceneWithTransition(sceneName, -1f));
    }


}
