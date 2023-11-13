using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kuhelm : PlayerCharBase
{
    [SerializeField] private BoxCollider2D AttackCol;
    private bool isReflect = false;

    protected override void Init()
    {
        base.Init();
        Managers.Pool.Push(Managers.Resource.Instantiate("KuhelmSkill").GetComponent<Poolable>());
    }

    protected override void Attack()
    {
        isAct = true;
        rigid.velocity = Vector2.zero;
        StartCoroutine(OnAttack());
    }
    private IEnumerator OnAttack()
    {
        Vector2 dir = (targetObject.transform.position - transform.position).normalized;
        isFlip = dir.x > 0;

        anim.SetBool(_move, false);
        anim.SetTrigger(_attack);

        targetObject.OnDamage(atk, crit, this);

        yield return new WaitForSeconds(0.8f);
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
        Managers.Pool.PoolManaging("KuhelmSkill", transform);
        CinemachineCameraShaking.Instance.CameraShake(3, 0.3f);

        isReflect = true;
        foreach (var enemy in TeamManager.Instance.enemyTeamList)
        {
            enemy.SetTarget(this);
        }

        yield return new WaitForSeconds(3f);
        isReflect = false;
    }

    public override void OnDamage(float damage, float critChance = 0, CharBase from = null, string hitEffect = null)
    {
        if (isReflect)
        {
            base.OnDamage(damage * 0.5f, critChance, from, hitEffect);
            from.OnDamage(damage * 0.5f, 0, this);
            return;
        }
        base.OnDamage(damage, critChance, from, hitEffect);

    }
}
