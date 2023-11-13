using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoSingleton<StageSelect>
{
    [SerializeField] private GameObject settingPanel;
    private static List<Button> stageSelectBtnList = new List<Button>();
    private static Dictionary<int, bool> isCleared = new Dictionary<int, bool>();
    private int idx = 0;

    private void Awake()
    {
        Init();
        SetButtonEnable();
    }

    private void Init()
    {
        stageSelectBtnList.Clear();
        for (int i = 0; i < GetComponentsInChildren<Button>().Length; i++)
        {
            int idx = i;

            stageSelectBtnList.Add(GetComponentsInChildren<Button>()[idx]);

            stageSelectBtnList[idx].onClick.RemoveListener(() => ActiveCharSet(idx + 1));
            stageSelectBtnList[idx].onClick.AddListener(() => ActiveCharSet(idx + 1));
        }
    }
    public void SetClear(int num)
    {
        if (isCleared.ContainsKey(num)) return;
        isCleared.Add(num, true);
    }
    public void SetButtonEnable()
    {
        for(int i = 0; i < stageSelectBtnList.Count; i++)
        {
            TextMeshProUGUI stageText = stageSelectBtnList[i].GetComponentInChildren<TextMeshProUGUI>();
            if (isCleared.ContainsKey(i))
            {
                stageText.color = Color.yellow;
            }
            else
            {
                if (i == 0) continue;
                if (isCleared.ContainsKey(i - 1))
                    stageText.color = Color.white;
                else
                    stageText.color = Color.gray;
            }
        }
    }

    public void ActiveCharSet(int num)
    {
        if (num != 1 && !isCleared.ContainsKey(num-2)) return;
        Base.stageIdx = num;
        idx = Base.stageIdx;

        settingPanel.SetActive(true);
    }
    public void ToStage()
    {
        if (StageSetting.Instance.CheckNotMatchedPlayer()) return;

        StageSetting.Instance.GetPlayer();
        StageSetting.Instance.GetEnemy();
        Managers.Scene.LoadScene(Define.Scene.Ingame, idx);
    }
}
