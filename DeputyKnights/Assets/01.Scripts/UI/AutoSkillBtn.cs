using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AutoSkillBtn : MonoSingleton<AutoSkillBtn>
{
    public bool isActive = false;
    [SerializeField] private Image autoIcon;
    [SerializeField] private TextMeshProUGUI autoText;

    private Animator iconAnim;
    private int _active = Animator.StringToHash("Active");

    [HideInInspector] public UnityEvent onSkillEndEvent = new UnityEvent();

    private void Awake()
    {
        iconAnim = autoIcon.GetComponent<Animator>();
    }

    public void OnClick()
    {
        isActive = !isActive;
        iconAnim.SetBool(_active, isActive);

        if (isActive) Active();
        else Disabled();
    }
    public void Active()
    {
        onSkillEndEvent.RemoveListener(AutoSkillUse);
        onSkillEndEvent.AddListener(AutoSkillUse);

        autoIcon.color = Color.yellow;
        autoText.color = Color.yellow;
    }
    public void Disabled()
    {
        onSkillEndEvent.RemoveListener(AutoSkillUse);

        autoIcon.color = Color.white;
        autoText.color = Color.white;
    }

    public void AutoSkillUse()
    {
        for(int i =0; i < SkillIconParents.Instance.skillIcons.Count; i++)
        {
            if (SkillIconParents.Instance.skillIcons[i].isCoolDowned())
                SkillIconParents.Instance.skillIcons[i].OnSkillActive(i,
                    TeamManager.Instance.playerTeamList[i].skillCool, TeamManager.Instance.playerTeamList[i].coolDownSpeed);

        }
    }
}
