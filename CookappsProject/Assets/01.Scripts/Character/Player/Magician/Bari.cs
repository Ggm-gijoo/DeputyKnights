using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bari : PlayerCharBase
{
    protected override void Init()
    {
        base.Init();
    }
    protected override void Attack()
    {
        isAct = true;
        rigid.velocity = Vector2.zero;
        StartCoroutine(OnAttack());
    }

    private IEnumerator OnAttack()
    {
        BariProjectile clone = Managers.Pool.PoolManaging("BariAttack", transform.position, Quaternion.identity).GetComponent<BariProjectile>();
        clone.target = targetObject.transform;
        clone.origin = this;

        yield return new WaitForSeconds(1.5f);
        isAct = false;
    }
}
