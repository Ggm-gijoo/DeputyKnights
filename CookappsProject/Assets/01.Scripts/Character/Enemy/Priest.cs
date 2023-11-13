using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Priest : EnemyCharBase
{
    protected override void Attack()
    {
        isAct = true;
        rigid.velocity = Vector2.zero;
        StartCoroutine(OnAttack());
    }

    private IEnumerator OnAttack()
    {
        anim.SetBool(_move, false);
        anim.SetTrigger(_attack);

        yield return new WaitForSeconds(0.5f);
        
        if(targetObject != null)
            targetObject.OnDamage(atk, crit, this);

        yield return new WaitForSeconds(1.5f);
        isAct = false;
    }
}
