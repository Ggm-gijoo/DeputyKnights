using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyIcon : MonoBehaviour
{
    [SerializeField] private GameObject descPanel;
    [SerializeField] private TextMeshProUGUI descNameTxt;
    [SerializeField] private TextMeshProUGUI descDescTxt;

    private TextMeshProUGUI txt;
    private Image icon;

    private void Awake()
    {
        txt = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("Icon").GetComponent<Image>();
    }
    public void SettingJob(CharacterJob character)
    {
        txt.text = PlayerCharBase.jobSynergyDict[character].ToString();
        icon.sprite = IconManager.Instance.jobIconDict[character];

        if (PlayerCharBase.jobSynergyDict[character] >= 2)
        {
            txt.color = Color.yellow;
        }

        int charCount = PlayerCharBase.jobSynergyDict[character] - 1;

        switch (character)
        {
            case CharacterJob.Warrior:
                descNameTxt.text = "전사";
                descDescTxt.text = $"시작 쿨타임 {charCount * 30}% 감소";
                break;
            case CharacterJob.Magician:
                descNameTxt.text = "마법사";
                descDescTxt.text = $"스킬 쿨다운 속도 {charCount * 25}% 증가";
                break;
            case CharacterJob.Supporter:
                descNameTxt.text = "지원가";
                descDescTxt.text = $"스킬 사용 시 {charCount * 5}% 확률로 쿨타임 초기화";
                break;
            case CharacterJob.Assassin:
                descNameTxt.text = "암살자";
                descDescTxt.text = $"치명타 시 쿨타임 {charCount * 10}% 감소";
                break;
        }
    }
    public void SettingElemental(CharacterElemental elemental)
    {
        txt.text = PlayerCharBase.elementalSynergyDict[elemental].ToString();
        icon.sprite = IconManager.Instance.elementalIconDict[elemental];

        if (PlayerCharBase.elementalSynergyDict[elemental] >= 2)
        {
            txt.color = Color.yellow;
        }

        int charCount = PlayerCharBase.elementalSynergyDict[elemental] - 1;

        switch (elemental)
        {
            case CharacterElemental.Light:
                descNameTxt.text = "광명";
                descDescTxt.text = $"이동 속도 {charCount * 10}% 증가";
                break;
            case CharacterElemental.Dark:
                descNameTxt.text = "암흑";
                descDescTxt.text = $"치명타 확률 {charCount * 10}% 증가";
                break;
            case CharacterElemental.Fire:
                descNameTxt.text = "화염";
                descDescTxt.text = $"공격력 {charCount * 10}% 증가";
                break;
            case CharacterElemental.Water:
                descNameTxt.text = "격류";
                descDescTxt.text = $"체력 {charCount * 10}% 증가";
                break;
            case CharacterElemental.Ground:
                descNameTxt.text = "대지";
                descDescTxt.text = $"방어력 {charCount * 10}% 증가";
                break;
            case CharacterElemental.Arcane:
                descNameTxt.text = "몽환";
                descDescTxt.text = $"모든 능력치 {charCount * 2.5f}% 증가";
                break;
        }
    }
    public void ActivePanel() => descPanel.SetActive(true);
    public void DisablePanel() => descPanel.SetActive(false);
}
