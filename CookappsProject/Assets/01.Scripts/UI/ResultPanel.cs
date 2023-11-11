using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoSingleton<ResultPanel>
{
    [SerializeField] private Image charIcon;
    [SerializeField] private TextMeshProUGUI charName;

    public void OnExit()
    {
        Managers.Scene.LoadScene(Define.Scene.Main);
    }

    public void SetMVPIcon()
    {
        charIcon.sprite = TeamManager.Instance.GetMVP().charIcon;
        charName.text = TeamManager.Instance.GetMVP().charSO.CharName;
    }
}
