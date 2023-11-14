using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum position
{
    forward = 0,
    middle,
    back,
}
public class SetManager : MonoSingleton<SetManager>
{
    public static Dictionary<position, List<CharacterSO>> positionDict = new Dictionary<position, List<CharacterSO>>();
    
    public static List<CharacterSO> forwardList = new List<CharacterSO>();
    public static List<CharacterSO> middleList = new List<CharacterSO>();
    public static List<CharacterSO> backList = new List<CharacterSO>();

    public int settedCount = 0;

    public void Clear()
    {
        positionDict.Clear();
        forwardList.Clear();
        middleList.Clear();
        backList.Clear();
    }
    public void SetPosition(position pos, CharacterSO character)
    {
        switch(pos)
        {
            case position.forward:
                forwardList.Add(character);
                break;
            case position.middle:
                middleList.Add(character);
                break;
            case position.back:
                backList.Add(character);
                break;
        }
    }
    public void SetFinalPos()
    {
        positionDict.Add(position.forward, forwardList);
        positionDict.Add(position.middle, middleList);
        positionDict.Add(position.back, backList);

        foreach(var pos in positionDict.Keys)
        {
            foreach(var character in positionDict[pos])
            {
                GameObject clone = Instantiate(character.Character);
                clone.GetComponent<CharBase>().settedPos = pos;
            }
        }
        positionDict.Clear();
    }
}
