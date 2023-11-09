using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconParents : MonoSingleton<SkillIconParents>
{
    [SerializeField] private GameObject skillIconPrefab;
    public List<SkillIcon> skillIcons = new List<SkillIcon>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        for(int i = 0; i < TeamManager.Instance.playerTeamList.Count; i++)
        {
            Instantiate(skillIconPrefab, transform);
        }

        for (int i = 0; i < GetComponentsInChildren<SkillIcon>().Length; i++)
        {
            skillIcons.Add(GetComponentsInChildren<SkillIcon>()[i]);
            skillIcons[i].SetSkillIcon(i);

            int idx = i;
            skillIcons[idx].skillBtn.onClick.AddListener(() => skillIcons[idx].OnSkillActive(idx,
                TeamManager.Instance.playerTeamList[idx].skillCool, TeamManager.Instance.playerTeamList[idx].coolDownSpeed));
        }
    }
}
