using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoSingleton<Base>
{
    public static int stageIdx = 1;
    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
    }

    public virtual void Clear()
    {
        GameManager.Instance.SetTimeScale(1f);
    }
}
