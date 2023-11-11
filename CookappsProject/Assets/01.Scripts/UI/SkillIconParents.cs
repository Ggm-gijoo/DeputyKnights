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
            skillIcons[i].SetSkillDesc(i);

            int idx = i;
            PlayerCharBase team = TeamManager.Instance.playerTeamList[idx];
            SkillIcon skillIcon = skillIcons[idx];

            skillIcon.skillBtn.onClick.AddListener(() => skillIcon.OnSkillActive(idx, team.skillCool, team.coolDownSpeed));

            team.resetCoolTimeEvents.AddListener(skillIcon.ResetCoolTime);
            team.reduceCoolTimeEvents.AddListener(skillIcon.ReduceCoolTime);
            team.playerDieEvents.AddListener(skillIcon.DisableSkill);

            StartCoroutine(skillIcon.CoolDown(team.skillCool, team.coolDownSpeed));
            StartCoroutine(team.ReduceCoolTime());
        }
    }
}
