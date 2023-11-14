using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Uee : PlayerCharBase
{
    protected override void Init()
    {
        base.Init();
        Managers.Pool.Push(Managers.Resource.Instantiate("UeeHeal").GetComponent<Poolable>());
    }

    protected override void Attack()
    {
        isAct = true;
        rigid.velocity = Vector2.zero;

        anim.SetBool(_move, false);
        StartCoroutine(OnAttack());
    }
    private IEnumerator OnAttack()
    {
        float hp = 1f;
        foreach(CharBase character in TeamManager.Instance.playerTeamList)
        {
            if (character.IsDead) continue;
            float charHp = character.hp / character.maxHp;
            if (charHp < hp)
            {
                targetObject = character;
                hp = charHp;
            }
        }
        if (hp == 1)
        {
            isAct = false;
            yield break;
        }
        anim.SetTrigger(_attack);
        Managers.Pool.PoolManaging("UeeHeal", targetObject.transform.position, Quaternion.identity);
        targetObject.OnHeal(maxHp * 0.1f, this);
        
        yield return new WaitForSeconds(1.5f);
        
        SetTarget(CheckNearestEnemy());
        isAct = false;
    }

    public override void Skill()
    {
        if (IsDead) return;
        base.Skill();
        StartCoroutine(OnSkill());
    }
    private IEnumerator OnSkill()
    {
        foreach(var team in TeamManager.Instance.playerTeamList)
        {
            if (team.IsDead) continue;
            team.OnHeal(maxHp * 0.2f, this);
            team.atk += team.charSO.Atk * 0.5f;
            Managers.Pool.PoolManaging("UeeHeal", team.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(3f);

        foreach (var team in TeamManager.Instance.playerTeamList)
            team.atk -= team.charSO.Atk * 0.2f;
    }

}
