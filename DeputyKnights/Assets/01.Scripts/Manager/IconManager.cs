using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoSingleton<IconManager>
{
    public SerializableDictionary<CharacterJob, Sprite> jobIconDict;
    public SerializableDictionary<CharacterElemental, Sprite> elementalIconDict;
}
