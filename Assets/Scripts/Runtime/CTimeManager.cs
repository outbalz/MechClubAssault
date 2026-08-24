using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTimeManager : MonoBehaviour
{
    [SerializeField] private float _timeScale = 1;

    private void OnValidate()
    {
        Time.timeScale = _timeScale;
    }
}
