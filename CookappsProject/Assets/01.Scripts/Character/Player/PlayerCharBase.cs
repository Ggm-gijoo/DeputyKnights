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

    protected CharacterJob job;
    protected CharacterElemental elemental;

    protected static Dictionary<CharacterJob, int> jobSynergyDict = new Dictionary<CharacterJob, int>();
    protected static Dictionary<CharacterElemental, int> elementalSynergyDict = new Dictionary<CharacterElemental, int>();

    protected override void Init()
    {
        isPlayer = true;

        base.Init();

        job = charSO.Job;
        elemental = charSO.Elemental;

        charIcon = charSO.CharIcon;
        skillCool = charSO.SkillCool;
        skillIcon = charSO.SkillIcon;
        SetSynergy();
    }

    private void SetSynergy()
    {
        if (jobSynergyDict.ContainsKey(job))
            jobSynergyDict[job]++;
        else
            jobSynergyDict.Add(job, 1);
    }

    protected override void ResetTarget()
    {
        if (!targetObject.IsDead) return;

        float dist = 100;

        targetObject = null;
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
    protected void Synergy() 
    {
        switch(job)
        {
            case CharacterJob.Warrior:
                for (int i = 0; i < TeamManager.Instance.playerTeamList.Count; i++)
                    TeamManager.Instance.playerTeamList[i].atk += TeamManager.Instance.playerTeamList[i].charSO.Atk * 0.1f;
                break;
            case CharacterJob.Magician:
                for (int i = 0; i < TeamManager.Instance.playerTeamList.Count; i++)
                    TeamManager.Instance.playerTeamList[i].coolDownSpeed += 0.25f;
                break;
            case CharacterJob.Supporter:
                break;
            case CharacterJob.Special:
                break;
        }

        switch(elemental)
        {
            case CharacterElemental.Light:
                break;
            case CharacterElemental.Dark:
                break;
            case CharacterElemental.Fire:
                break;
            case CharacterElemental.Water:
                break;
            case CharacterElemental.Ground:
                break;
            case CharacterElemental.Arcane:
                break;
            default:
                break;
        }
    }
}
