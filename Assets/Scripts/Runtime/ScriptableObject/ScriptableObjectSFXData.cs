using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSfxSO", menuName = "ScriptableObjects/SfxSO")]
public class ScriptableObjectSFXData : ScriptableObject
{
    [SerializeField] private AudioClip _select;
    [SerializeField] private AudioClip _Cancel;
    [SerializeField] private AudioClip _Cursor;

    public AudioClip Select { get { return _select; } }
    public AudioClip Cancel { get { return _Cancel; } }
    public AudioClip Cursor { get { return _Cursor; } }
}
