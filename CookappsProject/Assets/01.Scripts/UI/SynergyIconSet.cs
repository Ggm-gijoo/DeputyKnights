using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyIconSet : MonoSingleton<SynergyIconSet>
{
    [SerializeField] private SerializableDictionary<CharacterJob, Sprite> jobIconDict;
    [SerializeField] private GameObject iconPrefab;


}
