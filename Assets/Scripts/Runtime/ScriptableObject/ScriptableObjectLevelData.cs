using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLevelSO", menuName = "ScriptableObjects/LevelSO")]
public class ScriptableObjectLevelData : ScriptableObject
{
    [SerializeField] private List<CEnemyUnitData> _enemyUnitDatas;
    [SerializeField] private List<Vector3> _enemySpawnPoints;
    [SerializeField] private float _rewardPrize;

    public List<CEnemyUnitData> EnemyUnitDatas { get { return new List<CEnemyUnitData>(_enemyUnitDatas); } }
    public List<Vector3> EnemySpawnPoints { get { return new List<Vector3>(_enemySpawnPoints); } }
    public float RewardPrize { get { return _rewardPrize; } }
}