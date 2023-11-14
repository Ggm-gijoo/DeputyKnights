using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DimensionSlasher : EnemyCharBase
{
    protected override void Init()
    {
        base.Init();
        ResetTarget();
    }
    public override void ResetTarget(CharBase origin = null)
    {
        base.ResetTarget();
        targetObject = GetFarthest();
    }
    private CharBase GetFarthest()
    {
        float distance = 0;
        CharBase result = null;
        foreach (CharBase target in TeamManager.Instance.playerTeamList)
        {
            if (target.IsDead) continue;

            float dist = Vector2.Distance(target.transform.position, transform.position);
            if (distance < dist)
            {
                distance = dist;
                result = target;
            }
        }
        return result;
    }
    protected override void Chase()
    {
        isAct = true;
        StartCoroutine(OnChase());
    }
    private IEnumerator OnChase()
    {
        Vector2 targetBack = targetObject.transform.position;
        targetBack += targetObject.isFlip ? Vector2.left : Vector2.right;


        Managers.Pool.PoolManaging("Teleport", transform.position, Quaternion.identity);
        Managers.Pool.PoolManaging("Teleport", targetBack, Quaternion.identity);

        yield return new WaitForSeconds(0.2f);
        transform.position = targetBack;

        Vector2 dir = (targetObject.transform.position - transform.position).normalized;
        isFlip = dir.x > 0;

        yield return new WaitForSeconds(0.2f);
        isAct = false;
    }
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

        yield return new WaitForSeconds(0.5f);
        isAct = false;
    }
}
