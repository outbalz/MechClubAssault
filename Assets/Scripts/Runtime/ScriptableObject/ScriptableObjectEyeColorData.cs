using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEyeColor", menuName = "ScriptableObjects/EyeColor")]
public class ScriptableObjectEyeColorData : ScriptableObject
{
    [SerializeField] private Material _eyeMaterial;

    public Material EyeMaterial { get { return _eyeMaterial; } }
}
