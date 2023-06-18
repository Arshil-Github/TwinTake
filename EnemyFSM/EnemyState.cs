using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    public abstract void StartState(EnemyStateManager manager);
    
    public abstract void UpdateState(EnemyStateManager manager);
    
    public abstract void FixedUpdateState(EnemyStateManager manager);

}
