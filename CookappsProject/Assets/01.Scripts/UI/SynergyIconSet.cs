using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyIconSet : MonoSingleton<SynergyIconSet>
{
    [SerializeField] private GameObject iconPrefab;

    public void SetIcon()
    {
        foreach(CharacterJob icon in PlayerCharBase.jobSynergyDict.Keys)
        {
            if (PlayerCharBase.jobSynergyDict[icon] <= 1) continue;

            GameObject clone = Instantiate(iconPrefab, transform);
            clone.GetComponent<SynergyIcon>().SettingJob(icon);

        }
        foreach (CharacterElemental icon in PlayerCharBase.elementalSynergyDict.Keys)
        {
            if (PlayerCharBase.elementalSynergyDict[icon] <= 1) continue;

            GameObject clone = Instantiate(iconPrefab, transform);
            clone.GetComponent<SynergyIcon>().SettingElemental(icon);

        }
    }


}
