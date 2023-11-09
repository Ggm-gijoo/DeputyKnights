using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerCharBase
{
    [SerializeField] private BoxCollider2D AttackCol;

    protected override void Attack()
    {
        isAct = true;
        rigid.velocity = Vector2.zero;
        StartCoroutine(OnAttack());
    }

    private IEnumerator OnAttack()
    {
        Vector2 dir = (targetObject.transform.position - transform.position).normalized;
        targetObject.OnDamage(atk, 10, this);

        yield return new WaitForSeconds(0.8f);
        isAct = false;
    }

    public override void Skill()
    {

    }
}
