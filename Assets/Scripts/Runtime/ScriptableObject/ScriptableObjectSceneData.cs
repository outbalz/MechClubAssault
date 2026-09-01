using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSceneSO", menuName = "ScriptableObjects/SceneSO")]
public class ScriptableObjectSceneData : ScriptableObject
{
    [SerializeField] private string _sceneName;

    public string SceneName { get { return _sceneName; } }
}
