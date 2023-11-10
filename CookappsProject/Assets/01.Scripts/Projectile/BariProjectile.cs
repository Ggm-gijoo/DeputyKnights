using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BariProjectile : ProjectileObj
{
    private float nowSpeed;
    private string hitEffect;

    private void Start()
    {
        hitEffect = "BariHitEffect";
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        nowSpeed = moveSpeed;
    }
    protected override void Move()
    {
        if(!target.gameObject.activeSelf)
        {
            Managers.Pool.Push(poolable);
            return;
        }
        nowSpeed += Time.deltaTime * 50;
        Vector3 dir = (target.transform.position - transform.position).normalized;

        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        transform.position += dir * Time.deltaTime * nowSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            collision.GetComponent<IHittable>().OnDamage(origin.atk, origin.crit, origin, hitEffect);
            Managers.Pool.Push(poolable);
        }
    }
}
