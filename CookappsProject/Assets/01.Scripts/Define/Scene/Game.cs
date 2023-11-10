using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Base
{
    public override void Init()
    {
        base.Init();
        StageSO stageSO = Managers.Resource.Load<StageSO>($"Stage_{stageIdx}");
        foreach (var enemy in stageSO.EnemyList)
            Instantiate(enemy.Character);
    }
    public override void Clear()
    {

    }
}
