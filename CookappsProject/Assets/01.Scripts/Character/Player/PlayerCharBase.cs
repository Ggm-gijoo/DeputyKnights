using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCharBase : CharBase
{

    #region Icon
    [HideInInspector] public Sprite charIcon;
    [HideInInspector] public Sprite skillIcon;
    #endregion
    #region skill
    [HideInInspector] public float skillCool;
    [HideInInspector] public float coolDownSpeed = 1f;
    [HideInInspector] public float resetCoolTimeChance = 0f;
    [HideInInspector] public float reduceCoolTimeValue = 0f;
    #endregion

    protected bool isUsedFirstSkill = false;

    protected CharacterJob job;
    protected CharacterElemental elemental;

    protected static Dictionary<CharacterJob, int> jobSynergyDict = new Dictionary<CharacterJob, int>();
    protected static Dictionary<CharacterElemental, int> elementalSynergyDict = new Dictionary<CharacterElemental, int>();
    #region events
    [HideInInspector] public UnityEvent<float> reduceCoolTimeEvents = new UnityEvent<float>();
    [HideInInspector] public UnityEvent resetCoolTimeEvents = new UnityEvent();
    #endregion

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
        {
            jobSynergyDict[job]++;
            Synergy();
        }
        else
            jobSynergyDict.Add(job, 1);
    }

    public override void ResetTarget()
    {
        if (targetObject != null && !targetObject.IsDead) return;
        targetObject = CheckNearestEnemy();
    }
    public EnemyCharBase CheckNearestEnemy()
    {
        float distance = 100;
        EnemyCharBase resultEnemy = null;
        foreach(EnemyCharBase enemy in TeamManager.Instance.enemyTeamList)
        {
            if (enemy.IsDead) continue;

            float enemyDist = Vector2.Distance(enemy.transform.position, transform.position);
            if (distance > enemyDist)
            {
                distance = enemyDist;
                resultEnemy = enemy;
            }
        }

        return resultEnemy;
    }

    public IEnumerator ResetCoolTime()
    {
        yield return null;

        if (resetCoolTimeChance <= 0) yield break;

        if (resetCoolTimeChance > Random.Range(0f,1f))
        {
            resetCoolTimeEvents.Invoke();
        }
    }
    public IEnumerator ReduceCoolTime()
    {
        yield return null;

        if (reduceCoolTimeValue <= 0) yield break;
        reduceCoolTimeEvents.Invoke(reduceCoolTimeValue);

        if (jobSynergyDict[CharacterJob.Warrior] >= 2 && !isUsedFirstSkill)
        {
            isUsedFirstSkill = true;
            reduceCoolTimeValue -= (skillCool * 0.3f) * (jobSynergyDict[CharacterJob.Warrior] - 1);
        }
    }

    public virtual void Skill() 
    {
        if (IsDead) return;
        StartCoroutine(ReduceCoolTime());
        StartCoroutine(ResetCoolTime());
    }
    protected void Synergy() 
    {
        switch(job)
        {
            //전사 : 시작 쿨타임 30% 감소
            case CharacterJob.Warrior:
                foreach(var team in TeamManager.Instance.playerTeamList)
                    team.reduceCoolTimeValue += skillCool * 0.3f;
                break;
            //마법사 : 스킬 쿨다운 25% 증가
            case CharacterJob.Magician:
                foreach (var team in TeamManager.Instance.playerTeamList)
                    team.coolDownSpeed += 0.25f;
                break;
            // 서포터 : 스킬 사용시 5% 확률로 쿨타임 초기화
            case CharacterJob.Supporter:
                foreach (var team in TeamManager.Instance.playerTeamList)
                {
                    team.resetCoolTimeChance += 0.05f;
                }
                break;
            case CharacterJob.Assassin:
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
