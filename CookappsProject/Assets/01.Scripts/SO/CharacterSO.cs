using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterJob
{
    None = 0,
    Warrior,
    Magician,
    Supporter,
    Assassin
}
public enum CharacterElemental
{
    None = 0,
    Light,
    Dark,
    Fire,
    Water,
    Ground,
    Arcane
}

[CreateAssetMenu(fileName = "SO/CharSO", menuName = "SO/CharSO")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] private GameObject character;

    [SerializeField] private string charName;
    [SerializeField] private Sprite charIcon;
    [SerializeField] private Sprite skillIcon;

    [SerializeField] private CharacterJob job;
    [SerializeField] private CharacterElemental elemental;

    [SerializeField] private float hp;
    [SerializeField] private float atk;
    [SerializeField] private float spd;
    [SerializeField] private float crit;
    [SerializeField] private float skillCool;

    public GameObject Character { get { return character; }}

    public string CharName { get { return charName; }}
    public Sprite CharIcon { get { return charIcon; }}
    public Sprite SkillIcon { get { return skillIcon; }}

    public CharacterJob Job { get { return job; }}
    public CharacterElemental Elemental { get { return elemental; } }

    public float Hp { get { return hp; }}
    public float Atk { get { return atk; }}
    public float Spd { get { return spd; }}
    public float Crit { get { return crit; }}
    public float SkillCool { get { return skillCool; }}
}
