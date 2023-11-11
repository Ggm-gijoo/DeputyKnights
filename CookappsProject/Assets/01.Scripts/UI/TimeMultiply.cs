using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeMultiply : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField] private Image mulIcon;
    [SerializeField] private TextMeshProUGUI mulText;

    public void OnClick()
    {
        isActive = !isActive;

        if (isActive) Active();
        else Disabled();
    }
    public void Active()
    { 
        mulIcon.color = Color.yellow;
        mulText.color = Color.yellow;
        GameManager.Instance.SetTimeScale(2f);
    }
    public void Disabled()
    {
        mulIcon.color = Color.white;
        mulText.color = Color.white;
        GameManager.Instance.SetTimeScale(1f);
    }
}
