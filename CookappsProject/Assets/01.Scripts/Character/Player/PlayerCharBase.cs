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
    [HideInInspector] public float critCoolTimeValue = 0f;
    #endregion

    protected CharacterJob job;
    protected CharacterElemental elemental;

    public static Dictionary<CharacterJob, int> jobSynergyDict = new Dictionary<CharacterJob, int>();
    public static Dictionary<CharacterElemental, int> elementalSynergyDict = new Dictionary<CharacterElemental, int>();
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
            //���� : ���� ��Ÿ�� 30% ����
            case CharacterJob.Warrior:
                foreach (var team in TeamManager.Instance.playerTeamList)
                    StartCoroutine(team.ReduceCoolTime(skillCool * 0.3f));
                break;
            //������ : ��ų ��ٿ� 25% ����
            case CharacterJob.Magician:
                foreach (var team in TeamManager.Instance.playerTeamList)
                    team.coolDownSpeed += 0.25f;
                break;
            //������ : ��ų ���� 5% Ȯ���� ��Ÿ�� �ʱ�ȭ
            case CharacterJob.Supporter:
                foreach (var team in TeamManager.Instance.playerTeamList)
                    team.resetCoolTimeChance += 0.05f;
                break;
            //�ϻ��� : ġ��Ÿ�� ��Ÿ�� 10% ����
            case CharacterJob.Assassin:
                foreach (var team in TeamManager.Instance.playerTeamList)
                    team.critCoolTimeValue += 0.1f;
                    break;
        }
    }
    protected void ElementalSynergy()
    {
        switch (elemental)
        {
            //�� : �� �̵��ӵ� 10% ����
            case CharacterElemental.Light:
                foreach (var team in TeamManager.Instance.playerTeamList)
                {
                    team.spd += team.charSO.Spd * 0.1f;
                }
                break;
            //��� : �� ũ��Ƽ�� Ȯ�� 10% ����
            case CharacterElemental.Dark:
                foreach (var team in TeamManager.Instance.playerTeamList)
                {
                    team.crit += team.charSO.Crit * 0.1f;
                }
                break;
            //�� : �� ���ݷ� 10% ����
            case CharacterElemental.Fire:
                foreach (var team in TeamManager.Instance.playerTeamList)
                {
                    team.atk += team.charSO.Atk * 0.1f;
                }
                break;
            //�� : �� ü�� 10% ����
            case CharacterElemental.Water:
                foreach (var team in TeamManager.Instance.playerTeamList)
                {
                    team.maxHp += team.charSO.Hp * 0.1f;
                    team.hp = maxHp;
                }    
                break;
            //���� : �� ���� 10% ����
            case CharacterElemental.Ground:
                foreach (var team in TeamManager.Instance.playerTeamList)
                {
                    team.def += team.charSO.Def * 0.1f;
                }
                break;
            //��ȯ : �� ��� �ɷ�ġ 2.5% ����
            case CharacterElemental.Arcane:
                foreach (var team in TeamManager.Instance.playerTeamList)
                {
                    team.spd += team.charSO.Spd * 0.025f;
                    team.atk += team.charSO.Atk * 0.025f;
                    team.maxHp += team.charSO.Hp * 0.025f;
                    team.def += team.charSO.Def * 0.025f;
                    team.crit += team.charSO.Crit * 0.025f;

                    hp = maxHp;
                }
                break;
            default:
                break;
        }
    }
}
