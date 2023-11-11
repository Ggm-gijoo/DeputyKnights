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

    protected CharacterJob job;
    protected CharacterElemental elemental;

    protected static Dictionary<CharacterJob, int> jobSynergyDict = new Dictionary<CharacterJob, int>();
    protected static Dictionary<CharacterElemental, int> elementalSynergyDict = new Dictionary<CharacterElemental, int>();
    #region events
    [HideInInspector] public UnityEvent playerDieEvents = new UnityEvent();
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
        SetJobSynergy();
        SetElementalSynergy();
    }

    private void SetJobSynergy()
    {
        if (jobSynergyDict.ContainsKey(job))
        {
            jobSynergyDict[job]++;
            JobSynergy();
        }
        else
            jobSynergyDict.Add(job, 1);
    }
    private void SetElementalSynergy()
    {
        if (elementalSynergyDict.ContainsKey(elemental))
        {
            elementalSynergyDict[elemental]++;
            ElementalSynergy();
        }
        else
            elementalSynergyDict.Add(elemental, 1);
    }

    public override void ResetTarget(CharBase origin = null)
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
    public IEnumerator ReduceCoolTime(float value = 0f)
    {
        yield return null;
        reduceCoolTimeEvents.Invoke(reduceCoolTimeValue + value);
    }

    public virtual void Skill() 
    {
        if (IsDead) return;
        StartCoroutine(ReduceCoolTime());
        StartCoroutine(ResetCoolTime());
    }
    protected void JobSynergy() 
    {
        switch(job)
        {
            //전사 : 시작 쿨타임 30% 감소
            case CharacterJob.Warrior:
                foreach (var team in TeamManager.Instance.playerTeamList)
                    StartCoroutine(team.ReduceCoolTime(skillCool * 0.3f));
                break;
            //마법사 : 스킬 쿨다운 25% 증가
            case CharacterJob.Magician:
                foreach (var team in TeamManager.Instance.playerTeamList)
                    team.coolDownSpeed += 0.25f;
                break;
            //서포터 : 스킬 사용시 5% 확률로 쿨타임 초기화
            case CharacterJob.Supporter:
                foreach (var team in TeamManager.Instance.playerTeamList)
                    team.resetCoolTimeChance += 0.05f;
                break;
            //암살자 : 치명타시 쿨타임 10% 감소
            case CharacterJob.Assassin:
                break;
        }
    }
    protected void ElementalSynergy()
    {
        switch (elemental)
        {
            case CharacterElemental.Light:
                break;
            case CharacterElemental.Dark:
                break;
            case CharacterElemental.Fire:
                break;
            //물 : 팀 체력 10% 증가
            case CharacterElemental.Water:
                foreach (var team in TeamManager.Instance.playerTeamList)
                {
                    team.maxHp += team.charSO.Hp * 0.1f;
                    team.hp = maxHp;
                }    
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
