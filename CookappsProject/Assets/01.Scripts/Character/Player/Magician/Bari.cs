using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bari : PlayerCharBase
{
    protected override void Init()
    {
        base.Init();
        Managers.Pool.Push(Managers.Resource.Instantiate("BariAttack").GetComponent<Poolable>());
        Managers.Pool.Push(Managers.Resource.Instantiate("BariSkill").GetComponent<Poolable>());
        Managers.Pool.Push(Managers.Resource.Instantiate("BariHitEffect").GetComponent<Poolable>());
    }

    protected override void Attack()
    {
        isAct = true;
        rigid.velocity = Vector2.zero;
        StartCoroutine(OnAttack());
    }

    private IEnumerator OnAttack()
    {
        Vector3 dir = targetObject.transform.position - transform.position;
        isFlip = dir.x > 0;

        anim.SetBool(_move, false);
        anim.SetTrigger(_attack);

        BariProjectile clone = Managers.Pool.PoolManaging("BariAttack", transform.position, 
        Quaternion.Euler(new Vector2(-90, -70))).GetComponent<BariProjectile>();

        clone.target = targetObject.transform;
        clone.origin = this;

        yield return new WaitForSeconds(1.25f);
        isAct = false;
    }

    public override void Skill()
    {
        base.Skill();
        StartCoroutine(OnSkill());
    }
    private IEnumerator OnSkill()
    {
        if (targetObject == null) yield break;
        Managers.Pool.PoolManaging("BariSkill", targetObject.transform);
        CinemachineCameraShaking.Instance.CameraShake(2, 0.5f);

        targetObject.isStun = true;
        targetObject.OnDamage(atk * 3f, crit, this);
        yield return new WaitForSeconds(3f);

        if(targetObject != null)
            targetObject.isStun = false;
    }
}
