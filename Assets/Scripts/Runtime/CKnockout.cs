using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKnockout : MonoBehaviour
{
    #region inspector
    [SerializeField] private CUnitAnimationController _animationController;
    #endregion

    #region private var
    private const float MINHIGHT = -20;
    #endregion


    private void Awake()
    {
        if(_animationController == null)
        {
            if(TryGetComponent<CUnitAnimationController>(out  _animationController) == false)
            {
                Debug.LogWarning("Missing CUnitAnimationController");
            }
        }
        this.enabled = false;
    }

    private void OnEnable()
    {
        _animationController.TriggerFalling();
    }

    private void Update()
    {
        transform.position += Vector3.down * 9.81f * Time.deltaTime;

        if(transform.position.y < MINHIGHT)
        {
            gameObject.SetActive(false);
        }

    }

}
