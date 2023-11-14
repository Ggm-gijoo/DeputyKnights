using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectIcon : MonoBehaviour
{
    private CharacterSO charSO;
    [SerializeField] private Image jobIcon;
    [SerializeField] private Image elementalIcon;

    [SerializeField] private GameObject statPanel;

    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI statTxt;
    [SerializeField] private TextMeshProUGUI skillTxt;

    public void SetCharSO(CharacterSO SO)
    {
        charSO = SO;

        if (SO.Job == CharacterJob.None) jobIcon.sprite = null;
        if (SO.Elemental == CharacterElemental.None) elementalIcon.sprite = null;

        jobIcon.sprite = IconManager.Instance.jobIconDict[charSO.Job];
        elementalIcon.sprite = IconManager.Instance.elementalIconDict[charSO.Elemental];

        nameTxt.text = charSO.CharName;
        statTxt.text = $"ü�� {charSO.Hp} ���ݷ� {charSO.Atk} ���� {charSO.Def}\n" +
            $"�ӵ� {charSO.Spd} ġ��Ÿ Ȯ�� {charSO.Crit}%";
        skillTxt.text = $"{charSO.SkillName}<size=70%>\n{charSO.SkillDesc}";
    }
    public CharacterSO GetCharSO()
    {
        return charSO;
    }

    public void ActiveStatPanel() => statPanel.SetActive(true);
    public void DisableStatPanel() => statPanel.SetActive(false);
    public void ResetStatPanel() => statPanel.SetActive(!statPanel.activeSelf);
}
