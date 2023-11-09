using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private CharBase character;
    [SerializeField] private Slider slider;
    [SerializeField] private Image hpBar;

    private void Start()
    {
        character = GetComponentInParent<CharBase>();
        Init();
    }
    public void Init()
    {
        hpBar.color = character.isPlayer ? Color.cyan : Color.magenta;
    }

    public void UpdateHpBar()
    {
        slider.value = character.hp / character.charSO.Hp;
    }
}
