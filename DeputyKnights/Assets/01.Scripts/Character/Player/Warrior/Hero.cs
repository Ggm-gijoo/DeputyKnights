using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : PlayerCharBase
{
    [SerializeField] private BoxCollider2D AttackCol;

    protected override void Init()
    {
        base.Init();
        Managers.Pool.Push(Managers.Resource.Instantiate("HeroSkill").GetComponent<Poolable>());
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
        isAct = true;
        Managers.Pool.PoolManaging("HeroSkill", transform.position, Quaternion.AngleAxis(90, Vector3.right));
        CinemachineCameraShaking.Instance.CameraShake(7, 0.1f);

        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 6.5f, 1 << 7);
        foreach(var col in cols)
        {
            col.GetComponent<IHittable>().OnDamage(atk * 1.5f, crit, this);
        }

        yield return new WaitForSeconds(0.5f);
        isAct = false;
    }
}
