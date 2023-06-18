using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Idle : EnemyState
{
    public override void StartState(EnemyStateManager manager)
    {

    }
    public override void UpdateState(EnemyStateManager manager)
    {
        if (manager.DetectionConeColide() == true)
        {
            if ((manager.transform.position - manager.player.transform.position).magnitude <= manager.instantCombatDistance)
            {
                manager.ChangeState(manager.combatState);
            }
            else
            {
                manager.ChangeState(manager.detectionState);
            }
            manager.rb.velocity = Vector2.zero;
        }

    }
    public override void FixedUpdateState(EnemyStateManager manager)
    {

    }
}
