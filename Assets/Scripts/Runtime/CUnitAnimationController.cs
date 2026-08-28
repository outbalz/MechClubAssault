using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CUnitAnimationController : MonoBehaviour
{
    #region inspector
    [SerializeField] private Animator _animator;
    #endregion

    #region private var
    private int _tFallingHash;
    #endregion

    private void Reset()
    {
        if(_animator == null)
        {
            if(TryGetComponent<Animator>(out _animator) == false)
            {
                Debug.LogWarning("Missing Animator");
            }
        }
    }
    private void Awake()
    {
        if(_animator == null)
        {
            if(TryGetComponent<Animator>(out _animator) == false)
            {
                Debug.LogWarning("Missing Animator");
            }
        }

        _tFallingHash = Animator.StringToHash("tFalling");
    }

    public void TriggerFalling()
    {
        _animator.SetTrigger(_tFallingHash);
    }

}
