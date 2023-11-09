using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MVPBar : MonoBehaviour
{
    [SerializeField] private Image charIcon;
    [SerializeField] private Slider mvpBar;
    [SerializeField] private TextMeshProUGUI dealTxt;

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        mvpBar.value = 0;
        dealTxt.text = "0<size=80%>(0%)";
    }
    public void SetCharIcon(int num) => charIcon.sprite = TeamManager.Instance.playerTeamList[num].charIcon;
    public void SetMvpBarValue(int num)
    {
        if(CharBase.totalDealStack <= 0)
        {
            mvpBar.value = 0;
            return;
        }
        float dealPercent = TeamManager.Instance.playerTeamList[num].dealStack / CharBase.totalDealStack;
        
        mvpBar.value = dealPercent;
        dealTxt.text = $"{TeamManager.Instance.playerTeamList[num].dealStack}<size=80%>({(int)(dealPercent * 100)}%)";
    }

}
