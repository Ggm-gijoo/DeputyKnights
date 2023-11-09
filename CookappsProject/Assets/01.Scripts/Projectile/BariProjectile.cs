using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BariProjectile : ProjectileObj
{
    private float nowSpeed;

    protected override void OnEnable()
    {
        base.OnEnable();
        nowSpeed = moveSpeed;
    }
    protected override void Move()
    {
        moveSpeed += Time.deltaTime;
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, target.position, nowSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            collision.GetComponent<IHittable>().OnDamage(origin.atk, 10, origin);
            Managers.Pool.PoolManaging("HitEffect", collision.transform.position, Quaternion.identity);
            Managers.Pool.Push(poolable);
        }
    }
}
