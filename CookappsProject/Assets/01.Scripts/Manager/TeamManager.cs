using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeamManager : MonoSingleton<TeamManager>
{
    public List<PlayerCharBase> playerTeamList = new List<PlayerCharBase>();
    public List<EnemyCharBase> enemyTeamList = new List<EnemyCharBase>();

    public UnityEvent<CharBase> OnDieEvent = new UnityEvent<CharBase>();
    public UnityEvent OnHitEvent = new UnityEvent();

    private int enemyDeadCount = 0;
    private int playerDeadCount = 0;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        SetTeamPosition();
        SetTargetInit();

        OnDieEvent.RemoveListener(CheckAllDie);
        OnDieEvent.AddListener(CheckAllDie);
    }
    public void SetTeamPosition()
    {
        for(int i = 0; i < playerTeamList.Count; i++)
        {
            playerTeamList[i].transform.position = new Vector2(-5, playerTeamList.Count - 1 - i * 1.5f); 
        }
        
        for(int i = 0; i < enemyTeamList.Count; i++)
        {
            enemyTeamList[i].transform.position = new Vector2(5, enemyTeamList.Count - 1 - i * 1.5f);
        }
    }
    public void SetTargetInit()
    {
        for(int i = 0; i < playerTeamList.Count; i++)
        {
            playerTeamList[i].ResetTarget();
        }
        for (int i = 0; i < enemyTeamList.Count; i++)
        {
            enemyTeamList[i].ResetTarget();
        }
    }
    public void CheckAllDie(CharBase character)
    { 
        if (character.isPlayer)
        {
            playerDeadCount++;
            if (playerDeadCount >= playerTeamList.Count)
                GameManager.Instance.Defeat(character.transform);
        }
        else
        {
            enemyDeadCount++;
            if (enemyDeadCount >= enemyTeamList.Count)
                GameManager.Instance.Victory(character.transform);
        }
    }

    public PlayerCharBase GetMVP()
    {
        PlayerCharBase resultChar = null;
        float mvpCount = 0;

        foreach(var team in playerTeamList)
        {
            if(team.mvpStack >= mvpCount)
            {
                resultChar = team;
                mvpCount = team.mvpStack;
            }
        }
        return resultChar;
    }
}
