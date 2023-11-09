using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharBase : CharBase
{
    [HideInInspector] public Sprite charIcon;
    [HideInInspector] public Sprite skillIcon;
    [HideInInspector] public float skillCool;
    [HideInInspector] public float coolDownSpeed = 1f;

    protected static Dictionary<CharacterJob, int> synergyDict = new Dictionary<CharacterJob, int>();

    protected override void Init()
    {
        isPlayer = true;

        base.Init();

        charIcon = charSO.CharIcon;
        skillCool = charSO.SkillCool;
        skillIcon = charSO.SkillIcon;
        SetSynergy();
    }

    private void SetSynergy()
    {
        if (synergyDict.ContainsKey(job))
        {
            synergyDict[job]++;
            Synergy();
        }
        else
            synergyDict.Add(job, 1);
    }

    protected override void ResetTarget()
    {
        if (!targetObject.IsDead) return;

        float dist = 100;

        for (int i = 0; i < TeamManager.Instance.enemyTeamList.Count; i++)
        {
            if (TeamManager.Instance.enemyTeamList[i].IsDead) continue;

            float targetDist = Vector2.Distance(TeamManager.Instance.enemyTeamList[i].transform.position, transform.position);
            if (dist > targetDist)
            {
                dist = targetDist;
                targetObject = TeamManager.Instance.enemyTeamList[i];
            }
        }
    }

    public virtual void Skill() { }
    protected virtual void Synergy() { }
}
