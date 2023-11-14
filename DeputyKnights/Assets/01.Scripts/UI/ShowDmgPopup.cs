using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ShowDmgPopup : MonoSingleton<ShowDmgPopup>
{
    [SerializeField]
    public GameObject displayDamageTMP = null;

    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

    public void ShowDmg(float damage, GameObject damagedObj, bool isCrit = false)
    {
        StartCoroutine(IEShowDamage(damage, damagedObj, isCrit));
    }

    private IEnumerator IEShowDamage(float damage, GameObject damagedObj, bool isCrit = false)
    {
        Poolable damageTMP = Managers.Pool.Pop(displayDamageTMP);
        TextMeshProUGUI damageText = damageTMP.GetComponentInChildren<TextMeshProUGUI>();


        damageText.text = Mathf.RoundToInt(-damage).ToString();
        if (isCrit)
        {
            damageText.color = Color.yellow;
        }
        else
        {
            damageText.color = Color.white;
        }

        if(damage < 0)
        {
            damageText.color = Color.green;
        }

        damageTMP.transform.position = damagedObj.transform.position;
        damageTMP.transform.DOScale(0.01f, 0.1f);

        damageTMP.transform.position = new Vector3(damagedObj.transform.position.x, damagedObj.transform.position.y, 0);
        damageTMP.transform.DOMoveZ(damagedObj.transform.position.z + Random.Range(-2.5f, -1.5f), 0.5f).SetEase(Ease.OutQuart).SetUpdate(true);

        StartCoroutine(PoolDamageTMP(damageTMP, damageText));
        yield return null;
    }

    private IEnumerator PoolDamageTMP(Poolable damageTMP, TextMeshProUGUI damageText)
    {
        damageTMP.transform.DOScale(0.005f, 1f);
        damageText.DOFade(0f, 1f);
        yield return waitForSeconds;
        Managers.Pool.Push(damageTMP);
    }
}
