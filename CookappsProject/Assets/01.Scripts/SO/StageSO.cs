using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO/StageSO", menuName = "SO/StageSO")]
public class StageSO : ScriptableObject
{
    [SerializeField] private List<CharacterSO> enemyList;
    public List<CharacterSO> EnemyList { get { return enemyList; } }
}
