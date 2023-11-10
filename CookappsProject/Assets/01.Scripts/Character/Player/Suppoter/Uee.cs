using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Uee : PlayerCharBase
{
    protected override void Attack()
    {
        isAct = true;
        rigid.velocity = Vector2.zero;
        StartCoroutine(OnAttack());
    }
    private IEnumerator OnAttack()
    {
        float hp = 1f;
        foreach(CharBase character in TeamManager.Instance.playerTeamList)
        {
            float charHp = character.hp / character.charSO.Hp;
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
        targetObject.OnHeal(charSO.Hp * 0.2f, this);
        
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
            team.OnHeal(charSO.Hp * 0.2f, this);
            team.atk += team.charSO.Atk * 0.2f;
        }

        yield return new WaitForSeconds(3f);

        foreach (var team in TeamManager.Instance.playerTeamList)
            team.atk -= team.charSO.Atk * 0.2f;
    }

}
