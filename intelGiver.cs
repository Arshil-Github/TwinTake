using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class intelGiver : MonoBehaviour
{
    public Transform target;
    bool interact = false;
    public LineRenderer lr;

    List<Vector3> waypoints;
    Seeker seeker;
    Path path;
    GUIManager guiManager;
    // Start is called before the first frame update
    void Start()
    {
        waypoints = new List<Vector3>();
        seeker = GetComponent<Seeker>();

        guiManager = FindObjectOfType<GUIManager>();
    }
    void onPathDetectComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            waypoints = path.vectorPath;
            pathLineMaker();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && interact == true)
        {
            //Add fuck ton of GUI here

            seeker.StartPath(transform.position, target.position, onPathDetectComplete);
            interact = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Add instructions here
        if(collision.CompareTag("Player"))
        {
            interact = true;
            guiManager.setInfoText("Press X to get intel");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            guiManager.nullText();
        }
    }
    private void pathLineMaker()
    {
        lr.positionCount = waypoints.Count;


        lr.SetPositions(waypoints.ToArray());

    }
}
