using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Base
{
    public override void Init()
    {
        base.Init();

        SetManager.Instance.SetFinalPos();
        GameObject clone = Managers.Resource.Instantiate("HitEffect");
        Managers.Pool.Push(clone.GetComponent<Poolable>());
    }
    public override void Clear()
    {
        PlayerCharBase.jobSynergyDict.Clear();
        PlayerCharBase.elementalSynergyDict.Clear();

        foreach(Poolable pool in FindObjectsOfType<Poolable>())
            Managers.Pool.Push(pool);
    }
}
