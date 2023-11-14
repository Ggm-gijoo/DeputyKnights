using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoSingleton<ResultPanel>
{
    [SerializeField] private Image charIcon;
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI mvpValueText;
    [SerializeField] private Slider mvpValue;

    [SerializeField] private TextMeshProUGUI mainText;

    public void SetMVPIcon()
    {
        PlayerCharBase mvp = TeamManager.Instance.GetMVP();

        charIcon.sprite = mvp.charIcon;
        charName.text = mvp.charSO.CharName;
        mvpValueText.text = $"업무 성과| {(int)mvp.mvpStack}({(int)((mvp.mvpStack / CharBase.totalMvpStack) * 100)}%)";
        mvpValue.value = mvp.mvpStack / CharBase.totalMvpStack;
    }
    public void SetMainText(bool result)
    {
        mainText.color = result ? Color.yellow : Color.blue;
        mainText.text = result ? "VICTORY!" : "DEFEAT...";
    }
}
