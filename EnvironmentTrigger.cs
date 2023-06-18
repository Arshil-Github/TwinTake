using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnvironmentTrigger : MonoBehaviour
{
    private Player_Movement player;
    private EnemyStateManager[] enemies;
    private GUIManager guiManager;

    public float distanceForInteraction;
    public bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player_Movement>();
        enemies = FindObjectsOfType<EnemyStateManager>();

        guiManager = FindObjectOfType<GUIManager>();
    }

    bool isGUIShowing = false;
    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= distanceForInteraction && !isGUIShowing)
        {
            guiManager.setInfoText("Press X to turn the sound up");
            isGUIShowing = true;
        }else if (Vector2.Distance(transform.position, player.transform.position) > distanceForInteraction && isGUIShowing)
        {
            guiManager.nullText();
        }

        if(Vector2.Distance(transform.position, player.transform.position) <= distanceForInteraction && triggered == false && Input.GetKeyDown(KeyCode.X))
        {
            triggered = true;
            Debug.Log("a");
            foreach(EnemyStateManager esm in enemies)
            {
                esm.EnrmTrigger(transform.position);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanceForInteraction);
    }

}
