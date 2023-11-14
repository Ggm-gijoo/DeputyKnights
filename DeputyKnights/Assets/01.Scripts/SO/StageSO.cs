using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO/StageSO", menuName = "SO/StageSO")]
public class StageSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<position, List<CharacterSO>> enemyPositionDict;
    public SerializableDictionary<position, List<CharacterSO>> EnemyPositionDict { get { return enemyPositionDict; } }
}
