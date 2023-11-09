using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoSingleton<Base>
{
    [SerializeField]
    protected Define.Scene SceneType = Define.Scene.Unknown;
    [SerializeField]
    protected Define.MapTypeFlag MapType = Define.MapTypeFlag.Default;

    public virtual void Init()
    {
    }

    public abstract void Clear();
}
