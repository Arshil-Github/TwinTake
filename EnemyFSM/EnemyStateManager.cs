using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{

    EnemyState currentState;
    public EnemyState_Patrol patrolState = new EnemyState_Patrol();
    public EnemyState_Detection detectionState = new EnemyState_Detection();
    public EnemyState_Combat combatState = new EnemyState_Combat();
    public EnemyState_Idle idleState = new EnemyState_Idle();
    
    public float speed;
    public float delayAtMarkers;
    public Collider2D detectionCone;
    public float instantCombatDistance;
    public float reactiveRadius;
    public MeshCreator fieldOfView;
    public float randomFactor = -0.5f;
    public float timeBtwStates = 3f;

    public List<GameObject> patrolMarkers;

    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector] public Vector2 alert_playerPos;
    public GameObject player;
    [HideInInspector] public SpriteRenderer sprite;
    [HideInInspector] public Enemy_Pathfinding pathFind;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<Player_Movement>().gameObject;
        rb = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        pathFind = GetComponent<Enemy_Pathfinding>();

    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(patrolState);
    }
    public void Update()
    {

        currentState.UpdateState(this);

        if(Input.GetKeyDown(KeyCode.K))
        {
            /*Debug.Log(DetectionConeColide());*/

        }
        fieldOfView.SetOrigin(transform.position);
    }

    public void ChangeState(EnemyState state)
    {
        //A smooth way of changing the State. Call it when Changing States
        StartCoroutine(stateChangeDelay(state));

    }
    IEnumerator stateChangeDelay(EnemyState state)
    {
        //Add the Delay of 2 seconds before changing the state
        rb.velocity = Vector2.zero;
        currentState = idleState;
        yield return new WaitForSeconds(timeBtwStates);
        currentState = state;
        state.StartState(this);
    }

    public void CoroutineExecutor(IEnumerator e)
    {
        StartCoroutine(e);
    }

    public void EnrmTrigger(Vector3 pos)
    {
        if(Vector2.Distance(pos, transform.position) <= reactiveRadius)
        {
            alert_playerPos = pos;
            ChangeState(detectionState);
        }
    }

    //Patrol Stuff--------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }
    public bool DetectionConeColide()
    {
        alert_playerPos = player.transform.position;
        return fieldOfView.playerBool;
       /* ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));

        contactFilter2D.useLayerMask = true;

        List<Collider2D> collisions = new List<Collider2D>();
        int colCount = detectionCone.OverlapCollider(contactFilter2D, collisions);
        bool final = false;


        foreach (Collider2D col2d in collisions)
        {
            if (col2d.CompareTag("Player"))
            {
                final = true;
                player = col2d.gameObject;
                alert_playerPos = new Vector2(col2d.transform.position.x, col2d.transform.position.y);
            }
        }
        return final;*/
    }

    public IEnumerator markerWait()
    {
        yield return new WaitForSeconds(delayAtMarkers);

        if(patrolMarkers.Count == 1)
        {
            patrolState.indexIncrease = 0;
            ChangeState(idleState);
            yield break;
        }

        //Changing Marker
        patrolState.currentTargetIndex += patrolState.indexIncrease;
        patrolState.currentTarget = patrolMarkers[patrolState.currentTargetIndex].transform.position;
        if (patrolState.currentTargetIndex >= patrolMarkers.Count - 1 || patrolState.currentTargetIndex <= 0)
        {
            patrolState.indexIncrease *= -1;
        }

        pathFind.PathFindExecute(patrolState.currentTarget);


        patrolState.faceDir = (patrolState.currentTarget - rb.position) / (patrolState.currentTarget - rb.position).magnitude;
        patrolState.movementfactor = 1;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, reactiveRadius);
    }
}
