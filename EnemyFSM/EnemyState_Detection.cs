using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Detection : EnemyState
{
    Vector2 posToInv;
    Vector2 posToMove;
    float randomFactor;
    int cycles = 7;
    int currentCycle = 0;
    public Vector2 faceDir;
    int movementfactor = 1;

    float waitAtEachPos = 1f;
    EnemyStateManager enemy;

    public override void StartState(EnemyStateManager manager)
    {
        randomFactor = manager.randomFactor;
        Debug.Log("this is alert State");

        //Setting up a Random Position somwhere near the player last known location to move. 
        posToInv = manager.alert_playerPos;
        posToMove = new Vector2(posToInv.x + Random.Range(-randomFactor, randomFactor), posToInv.y + Random.Range(-randomFactor, randomFactor));
        enemy = manager;

        manager.pathFind.PathFindExecute(posToMove);

        //For the direction
        faceDir = (posToMove - manager.rb.position) / (posToMove - manager.rb.position).magnitude;

        manager.sprite.color = Color.yellow;

    }
    public override void FixedUpdateState(EnemyStateManager manager)
    {
        //Vector2 moveDir = (posToMove - manager.rb.position) / (posToMove - manager.rb.position).magnitude;

        //move towards the location and change when there
        //manager.rb.MovePosition(manager.rb.position + moveDir * manager.speed * Time.deltaTime);
        //manager.rb.velocity = moveDir * manager.speed * Time.deltaTime * movementfactor;

        manager.fieldOfView.SetDirection(faceDir);
        //for when It Reached
        if ((posToMove - new Vector2(manager.transform.position.x, manager.transform.position.y)).magnitude < 0.05f && currentCycle < cycles && movementfactor == 1)
        {
            //Add a delay then Act
            manager.CoroutineExecutor(this.delayForChecking());
            manager.rb.velocity = Vector2.zero;

            currentCycle += 1;
            movementfactor = 0;
        }
        else if(currentCycle >= cycles)
        {
            //Return to Patrol
            currentCycle = 0;
            manager.ChangeState(manager.patrolState);
            movementfactor = 1;
        }
    }
    
    public override void UpdateState(EnemyStateManager manager)
    {
        //If Detected Again Combat State
        if (manager.DetectionConeColide() == true)
        {
            manager.ChangeState(manager.combatState);

        }

    }
    public IEnumerator delayForChecking()
    {
        //For Adding a Delay at each location of Checking. Can later add effects
        yield return new WaitForSeconds(waitAtEachPos);
        //Changing Marker
        posToMove = new Vector2(posToInv.x + Random.Range(-randomFactor, randomFactor), posToInv.y + Random.Range(-randomFactor, randomFactor));

        enemy.pathFind.PathFindExecute(posToMove);


        //For the direction
        faceDir = (posToMove - enemy.rb.position) / (posToMove - enemy.rb.position).magnitude;
        movementfactor = 1;

    }
}
