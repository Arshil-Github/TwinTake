using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyState_Patrol : EnemyState
{
    public Vector2 currentTarget;
    float toleratableDistance;
    public int currentTargetIndex = 0;
    public int indexIncrease = 1;
    public int movementfactor = 1;

    EnemyStateManager enemy;
    public Vector2 faceDir;
    public override void StartState(EnemyStateManager manager)
    {

        Debug.Log("this is patrol State");
        enemy = manager;

        currentTarget = manager.patrolMarkers[currentTargetIndex].transform.position;

        enemy.pathFind.PathFindExecute(currentTarget);

        faceDir = (currentTarget - manager.rb.position) / (currentTarget - manager.rb.position).magnitude;

        manager.sprite.color = Color.grey;
    }
    void pathFindStartDelay()
    {
    }
    public override void FixedUpdateState(EnemyStateManager manager)
    {
        //Vector2 moveDir = (currentTarget - manager.rb.position) / (currentTarget - manager.rb.position).magnitude;

        //move towards the location and change when there
        //manager.rb.MovePosition(manager.rb.position + moveDir * manager.speed * Time.deltaTime);
       // manager.rb.velocity = moveDir * manager.speed * Time.deltaTime * movementfactor;

        //manager.transform.up = faceDir;

        manager.fieldOfView.SetDirection(faceDir);

        //for when It Reached
        if((currentTarget - new Vector2(manager.transform.position.x, manager.transform.position.y)).magnitude < 0.05f && movementfactor == 1)
        {
            //Add a delay then Act

            manager.StartCoroutine(manager.markerWait());
            manager.rb.velocity = Vector2.zero;
            //Set the Rotation to the Direction facing
            

            movementfactor = 0;
        }

    }
    public override void UpdateState(EnemyStateManager manager)
    {
        if(manager.DetectionConeColide() == true)
        {
            if ((manager.transform.position - manager.player.transform.position).magnitude <= manager.instantCombatDistance)
            {
                manager.ChangeState(manager.combatState);
            }
            else { 
                manager.ChangeState(manager.detectionState);
            }
            manager.rb.velocity = Vector2.zero;
        }

        //Check if any reactivePopped Up then go to it's location for investigation

    }

}
