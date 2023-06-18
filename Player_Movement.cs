using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public Vector2 moveSpeed;
    public float timeForMax;

    public float dashingPower;
    public float dashingTime;
    public float dashCooldownTime;
    public TrailRenderer trailRenderer;

    Rigidbody2D rb;

    Vector2 moveDir;
    float currentSpeed;
    float moveAccn;

    [HideInInspector] public bool movementfactor;
    bool canDash = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        moveDir = new Vector2();

        currentSpeed = moveSpeed.x;
        moveAccn = (moveSpeed.y - moveSpeed.x) / timeForMax;
        trailRenderer.gameObject.SetActive(false);

        movementfactor = true;
    }

    private void Update()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyUp(KeyCode.Space) && movementfactor && canDash)
        {
            StartCoroutine(Dash());
        }

        /*if(Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(rb.velocity);
        }*/
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
        //rb.AddForce(moveDir * moveSpeed, ForceMode2D.Impulse);
        if(movementfactor)
        {
            rb.velocity = moveDir * currentSpeed * Time.deltaTime;


            if (currentSpeed < moveSpeed.y && moveDir.magnitude != 0)
            {
                currentSpeed += moveAccn * 0.02f;
            }
            else if (moveDir.magnitude == 0)
            {
                currentSpeed = moveSpeed.x;
            }
        }
    }
    IEnumerator Dash()
    {
        movementfactor = false;
        trailRenderer.gameObject.SetActive(true);
        rb.velocity = moveDir * dashingPower;

        StartCoroutine(DashCoolDown());

        yield return new WaitForSeconds(dashingTime);

        trailRenderer.gameObject.SetActive(false);
        movementfactor = true;
    }
    IEnumerator DashCoolDown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldownTime);
        canDash = true;
    }
}
