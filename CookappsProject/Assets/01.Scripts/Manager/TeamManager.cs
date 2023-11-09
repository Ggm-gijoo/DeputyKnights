using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Team
{
    player,
    enemy,
}

public class TeamManager : MonoSingleton<TeamManager>
{
    public List<PlayerCharBase> playerTeamList = new List<PlayerCharBase>();
    public List<CharBase> enemyTeamList = new List<CharBase>();

    public UnityEvent onDieEvent = new UnityEvent();
    public UnityEvent OnHitEvent = new UnityEvent();

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        SetTeamPosition();
        SetTargetInit();

        onDieEvent.RemoveListener(() => CheckAllDie());
        onDieEvent.AddListener(() => CheckAllDie());
    }
    public void SetTeamPosition()
    {
        for(int i = 0; i < playerTeamList.Count; i++)
        {
            playerTeamList[i].transform.position = new Vector2(-5, playerTeamList.Count - 1 - i * 2f); 
        }
        
        for(int i = 0; i < enemyTeamList.Count; i++)
        {
            enemyTeamList[i].transform.position = new Vector2(5, enemyTeamList.Count - 1 - i * 2f);
        }
    }
    public void SetTargetInit()
    {
        for(int i = 0; i < playerTeamList.Count; i++)
        {
            playerTeamList[i].SetTarget(enemyTeamList[i % enemyTeamList.Count]);
        }
        for (int i = 0; i < enemyTeamList.Count; i++)
        {
            enemyTeamList[i].SetTarget(playerTeamList[i % playerTeamList.Count]);
        }
    }
    public void CheckAllDie()
    {
        int enemyDeadCount = 0;
        int playerDeadCount = 0;

        foreach(var player in playerTeamList)
        {
            if (player.IsDead)
            {
                playerDeadCount++;
                if (playerDeadCount >= playerTeamList.Count)
                    GameManager.Instance.Defeat();
            }
        }
        foreach(var enemy in enemyTeamList)
        {
            if (enemy.IsDead)
            {
                enemyDeadCount++;
                if (enemyDeadCount >= enemyTeamList.Count)
                    GameManager.Instance.Victory();
            }
        }
    }
}
