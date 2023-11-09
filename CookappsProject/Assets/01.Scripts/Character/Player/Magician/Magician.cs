using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : PlayerCharBase
{
    protected override void Init()
    {
        base.Init();
    }
    protected override void Attack()
    {

    }
    protected override void Synergy()
    {
        //스택당 스킬 쿨다운 속도 50% 증가
        for(int i = 0; i < TeamManager.Instance.playerTeamList.Count; i++)
            TeamManager.Instance.playerTeamList[i].coolDownSpeed += 0.5f;
    }
}
