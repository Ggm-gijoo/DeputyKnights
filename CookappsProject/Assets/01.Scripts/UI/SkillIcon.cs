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

    [HideInInspector] public Button skillBtn;

    private float nowCooltime = 0f;

    public void SetSkillIcon(int num)
    {
        skillIcon.sprite = TeamManager.Instance.playerTeamList[num].skillIcon;
        skillBtn = skillIcon.GetComponent<Button>();
    }

    public void OnSkillActive(int num, float initCoolTime, float coolDownSpd)
    {
        if (!isCoolDowned()) return;

        TeamManager.Instance.playerTeamList[num].Skill();
        StartCoroutine(CoolDown(initCoolTime, coolDownSpd));
    }

    public bool isCoolDowned() => nowCooltime <= 0;

    private IEnumerator CoolDown(float initCoolTime, float coolDownSpd)
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
    }
}
