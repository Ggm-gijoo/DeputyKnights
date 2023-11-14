using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class SkillIcon : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image coolDownImg;
    [SerializeField] private TextMeshProUGUI coolDownText;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDesc;

    [SerializeField] private GameObject disablePanel;
    [SerializeField] private GameObject skillDescPanel;

    [HideInInspector] public Button skillBtn;
    private bool isCanUseSkill = true;

    private float nowCooltime = 0f;

    public void SetSkillIcon(int num)
    {
        skillIcon.sprite = TeamManager.Instance.playerTeamList[num].skillIcon;
        skillBtn = skillIcon.GetComponent<Button>();
    }
    public void SetSkillDesc(int num)
    {
        skillName.text = TeamManager.Instance.playerTeamList[num].charSO.SkillName;
        skillDesc.text = TeamManager.Instance.playerTeamList[num].charSO.SkillDesc;
    }

    public void OnSkillActive(int num, float initCoolTime, float coolDownSpd)
    {
        if (!isCoolDowned() || !isCanUseSkill) return;

        TeamManager.Instance.playerTeamList[num].Skill();
        StartCoroutine(CoolDown(initCoolTime, coolDownSpd));
    }
    public bool isCoolDowned() => nowCooltime <= 0;

    public IEnumerator CoolDown(float initCoolTime, float coolDownSpd)
    {
        nowCooltime = initCoolTime;
        coolDownText.gameObject.SetActive(true);
        coolDownImg.gameObject.SetActive(true);

        while(nowCooltime > 0f)
        {
            nowCooltime -= Time.deltaTime * coolDownSpd;
            
            coolDownImg.fillAmount = nowCooltime / initCoolTime;
            coolDownText.text = Mathf.RoundToInt(nowCooltime).ToString();

            yield return null;
        }

        coolDownText.gameObject.SetActive(false);
        coolDownImg.gameObject.SetActive(false);

        yield return null;
        AutoSkillBtn.Instance.onSkillEndEvent.Invoke();
    }

    public void ResetCoolTime()
    {
        nowCooltime = 0.01f;
    }
    public void ReduceCoolTime(float value)
    {
        nowCooltime -= value;
    }

    public void DisableSkill()
    {
        isCanUseSkill = false;
        disablePanel.SetActive(true);
    }

    public void OnTouchSkill() => skillDescPanel.SetActive(true);
    public void OnExitSkill() => skillDescPanel.SetActive(false);
}
