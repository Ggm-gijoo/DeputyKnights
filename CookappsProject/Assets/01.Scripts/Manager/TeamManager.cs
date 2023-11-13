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

        SynergyIconSet.Instance.SetIcon();

        OnDieEvent.RemoveListener(CheckAllDie);
        OnDieEvent.AddListener(CheckAllDie);
    }

    public void SetTeamPosition()
    {
        position getPos = position.middle;
        float pos = 0;

        int[] playerPosCount = new int[3];
        int[] enemyPosCount = new int[3];

        for(int i = 0; i < playerTeamList.Count; i++)
        {
            getPos = playerTeamList[i].settedPos;
            playerPosCount[(int)getPos]++;
            switch (getPos)
            {
                case position.forward:
                    pos = -3;
                    break;
                case position.middle:
                    pos = -5;
                    break;
                case position.back:
                    pos = -8;
                    break;
            }

            playerTeamList[i].transform.position = new Vector2(pos, (playerPosCount[(int)getPos] - 1) * 1.5f);
        }
        
        for(int i = 0; i < enemyTeamList.Count; i++)
        {
            getPos = enemyTeamList[i].settedPos;
            enemyPosCount[(int)getPos]++;
            switch (getPos)
            {
                case position.forward:
                    pos = 3;
                    break;
                case position.middle:
                    pos = 5;
                    break;
                case position.back:
                    pos = 8;
                    break;
            }
            enemyTeamList[i].transform.position = new Vector2(pos, (enemyPosCount[(int)getPos] - 1) * 1.5f);
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
                GameManager.Instance.Result(character.transform, false);
        }
        else
        {
            enemyDeadCount++;
            if (enemyDeadCount >= enemyTeamList.Count)
                GameManager.Instance.Result(character.transform, true);
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
