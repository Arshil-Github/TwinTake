using UnityEngine;

public class EnemyState_Combat : EnemyState
{
    public override void StartState(EnemyStateManager manager)
    {
        Debug.Log("Combat Bitches");
        manager.ChangeState(manager.patrolState);

        manager.sprite.color = Color.red;

    }
    public override void UpdateState(EnemyStateManager manager)
    {

    }
    public override void FixedUpdateState(EnemyStateManager manager)
    {

    }
}
