using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    virtual void OnDamage(float damage, float critChance = 0, CharBase from = null, string hitEffect = null) { }
}
