using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public void Victory()
    {
        Debug.Log("�¸�!");
    }

    public void Defeat()
    {
        Debug.Log("�й�...");
    }
}
