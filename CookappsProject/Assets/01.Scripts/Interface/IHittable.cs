using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    void OnDamage(float damage, float critChance = 0, CharBase from = null);
}
