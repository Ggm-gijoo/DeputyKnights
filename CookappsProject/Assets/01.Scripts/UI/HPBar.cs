using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private CharBase character;
    [SerializeField] private Slider slider;
    [SerializeField] private Image hpBar;

    private void OnEnable()
    {
        character = GetComponentInParent<CharBase>();
        StartCoroutine(Init());
    }
    public IEnumerator Init()
    {
        yield return null;
        hpBar.color = character.isPlayer ? Color.cyan : Color.magenta;
    }

    public void UpdateHpBar()
    {
        slider.value = character.hp / character.maxHp;
    }
}
