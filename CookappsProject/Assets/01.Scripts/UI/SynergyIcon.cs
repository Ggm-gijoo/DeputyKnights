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
                descNameTxt.text = "����";
                descDescTxt.text = $"���� ��Ÿ�� {charCount * 30}% ����";
                break;
            case CharacterJob.Magician:
                descNameTxt.text = "������";
                descDescTxt.text = $"��ų ��ٿ� �ӵ� {charCount * 25}% ����";
                break;
            case CharacterJob.Supporter:
                descNameTxt.text = "������";
                descDescTxt.text = $"��ų ��� �� {charCount * 5}% Ȯ���� ��Ÿ�� �ʱ�ȭ";
                break;
            case CharacterJob.Assassin:
                descNameTxt.text = "�ϻ���";
                descDescTxt.text = $"ġ��Ÿ �� ��Ÿ�� {charCount * 10}% ����";
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
                descNameTxt.text = "����";
                descDescTxt.text = $"�̵� �ӵ� {charCount * 10}% ����";
                break;
            case CharacterElemental.Dark:
                descNameTxt.text = "����";
                descDescTxt.text = $"ġ��Ÿ Ȯ�� {charCount * 10}% ����";
                break;
            case CharacterElemental.Fire:
                descNameTxt.text = "ȭ��";
                descDescTxt.text = $"���ݷ� {charCount * 10}% ����";
                break;
            case CharacterElemental.Water:
                descNameTxt.text = "�ݷ�";
                descDescTxt.text = $"ü�� {charCount * 10}% ����";
                break;
            case CharacterElemental.Ground:
                descNameTxt.text = "����";
                descDescTxt.text = $"���� {charCount * 10}% ����";
                break;
            case CharacterElemental.Arcane:
                descNameTxt.text = "��ȯ";
                descDescTxt.text = $"��� �ɷ�ġ {charCount * 2.5f}% ����";
                break;
        }
    }
    public void ActivePanel() => descPanel.SetActive(true);
    public void DisablePanel() => descPanel.SetActive(false);
}
