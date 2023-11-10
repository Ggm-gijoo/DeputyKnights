using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharBase : CharBase
{
    public override void ResetTarget()
    {
        if (targetObject != null && !targetObject.IsDead) return;
        targetObject = CheckNearestPlayer();
    }
    public PlayerCharBase CheckNearestPlayer()
    {
        float distance = 100;
        PlayerCharBase resultPlayer = null;
        foreach (PlayerCharBase player in TeamManager.Instance.playerTeamList)
        {
            if (player.IsDead) continue;

            float playerDist = Vector2.Distance(player.transform.position, transform.position);
            if (distance > playerDist)
            {
                distance = playerDist;
                resultPlayer = player;
            }
        }

        return resultPlayer;
    }
}
