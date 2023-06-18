using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy_Pathfinding : MonoBehaviour
{

    int currentWayPoint = 0;
    public float distanceForWaypoint = 0.08f;
    bool lastWaypoint = false;

    Seeker seeker;
    EnemyStateManager manager;
    Path path;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<EnemyStateManager>();
        seeker = GetComponent<Seeker>();

    }
    public void PathFindExecute(Vector2 pos)
    {
        seeker.StartPath(transform.position, pos, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    Vector2 moveDir;
    // Update is called once per frame
    void Update()
    {
        if (path == null)
            return;
        if (currentWayPoint >= path.vectorPath.Count)
        {
            lastWaypoint = true;
            return;
        }
        else
        {
            lastWaypoint = false;
        }

        moveDir = ((Vector2)path.vectorPath[currentWayPoint] - manager.rb.position).normalized;
        manager.rb.velocity = moveDir * manager.speed * Time.deltaTime;
        //manager.rb.AddForce(moveDir * manager.speed * Time.deltaTime);

        float distance = Vector2.Distance(manager.rb.position, path.vectorPath[currentWayPoint]);

        if(distance < distanceForWaypoint)
        {
            currentWayPoint++;
        }

    }
}
