using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CSceneTransitionUI : MonoBehaviour
{

    #region Inspector
    [SerializeField] private CanvasGroup _transitionUI;
    [SerializeField] private float _defultFadeDuration = 0.25f;
    #endregion

    #region privat var
    private Coroutine _fadeRoutine;
    #endregion

    public void Initialize()
    {
        if (_transitionUI == null)
        {
            Debug.LogWarning("Missing _transitionUI");
            
            return;
        }

        _transitionUI.alpha = 0.0f;
        _transitionUI.blocksRaycasts = false;
        _transitionUI.interactable = false;

    }

    public IEnumerator Co_FadeTo(float targetAlpha, float duration = -1f, bool blockRaycastWhileFading = true)
    {
        if (_transitionUI == null)
        {
            Debug.LogWarning("Missing _transitionUI");
            yield break;
        }

        if (duration < 0f)
        {
            duration = _defultFadeDuration;
        }

        if (_fadeRoutine != null)
        {
            StopCoroutine(_fadeRoutine);
            _fadeRoutine = null;
        }

        _fadeRoutine = StartCoroutine(Co_FadeInternal(targetAlpha, duration, blockRaycastWhileFading));

        yield return _fadeRoutine;

        _fadeRoutine = null;

    }

    private IEnumerator Co_FadeInternal(float targetAlpha, float duration, bool blockRaycastWhileFading)
    {
        float statAlpha = _transitionUI.alpha;

        _transitionUI.blocksRaycasts = blockRaycastWhileFading;

        _transitionUI.interactable = false;

        if (duration <= 0f)
        {
            _transitionUI.alpha = targetAlpha;

            _transitionUI.blocksRaycasts = (targetAlpha >= 0.99f);

            yield break;
        }

        float t = 0f;

        while (t < duration)
        {
            //dt 선택
            // ㄴ deltaTime 타임스케일 영향
            // ㄴ unscaled  안받음

            float dt =  Time.unscaledDeltaTime;

            t += dt;

            float lerp = Mathf.Clamp01(t / duration);

            _transitionUI.alpha = Mathf.Lerp(statAlpha, targetAlpha, lerp);

            yield return null;

        }

        _transitionUI.alpha = targetAlpha;

        _transitionUI.blocksRaycasts = (targetAlpha >= 0.99f);
    }

}
