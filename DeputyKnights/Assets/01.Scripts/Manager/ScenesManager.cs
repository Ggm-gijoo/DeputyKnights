using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager
{
    public Base CurrentScene { get { return GameObject.FindObjectOfType<Base>(); } }

    public void LoadScene(Define.Scene type, int idx = 0)
    {
        Managers.Clear();
        SetSceneIdx(idx);
        SceneManager.LoadScene(GetSceneName(type));
    }
    public void SetSceneIdx(int idx)
    {
        Base.stageIdx = idx;
    }
    string GetSceneName(Define.Scene type)
    {
        string name = Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
