using UnityEngine;
using System.Collections;
using System;

public class Koopa : Controller2D
{
    public float moveSpeed = 2;    
    public float gravity = -65;
    public float maxFall = 12;

    private float beforeSpeed;

    public Collider2D col;
    public Collider2D killCol;
    private Collider2D idleCol;
    private Collider2D duckCol;
    private Collider2D killIdleCol;
    private Collider2D killDuckCol;
    private Animator anim;

    public LayerMask ground;
    public LayerMask playerLayer;

    public Vector2 velocity;

    public GameObject player;

    private bool turn;
    public bool duck;
    public bool spin;

    // Use this for initialization
    void Start()
    {
        idleCol = transform.Find("Idle Collider").GetComponent<Collider2D>();
        duckCol = transform.Find("Duck Collider").GetComponent<Collider2D>();
        killIdleCol = transform.Find("Idle Collider").Find("Kill Idle Collider").GetComponent<Collider2D>();
        killDuckCol = transform.Find("Duck Collider").Find("Kill Duck Collider").GetComponent<Collider2D>();
        killCol = killIdleCol;
        col = idleCol;
        anim = transform.GetComponent<Animator>();

    }

    void Update()
    {

        if(collisions.right)
        {
            if (facingRight)
            {
                TurnAround();
            }                        
        }
        
        else if (collisions.left)
        {
            if (!facingRight)
            {
                TurnAround();
            }
        }
       
        Walk();

        velocity.y = Mathf.Max(velocity.y + gravity * Time.deltaTime, -maxFall);

        Move(velocity*Time.smoothDeltaTime, col, ground);
    }


    public void Spin(string direction)
    {
        if(direction == "Right")
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }

        spin = true;

        moveSpeed = (direction == "Right") ? 10 : -10;
        beforeSpeed = moveSpeed;
        killCol.gameObject.SetActive(true);
    }

    public override void Jump()
    {
        throw new NotImplementedException();
    }

    public override void Run()
    {
        throw new NotImplementedException();
    }

    public override void Walk()
    {
        velocity.x = moveSpeed;
        anim.SetFloat("Speed", Mathf.Abs(moveSpeed));
    }

    public override void Duck()
    {
        spin = false;
        beforeSpeed = moveSpeed;
        moveSpeed = 0;

        col = duckCol;
        killCol = killDuckCol;

        killCol.gameObject.SetActive(false);

        idleCol.gameObject.SetActive(false);
        duckCol.gameObject.SetActive(true);

        duck = true;

        anim.SetBool("Duck", duck);
    }

    public override void StandUp()
    {
        throw new NotImplementedException();
    }

    public override void TurnAround()
    {
        Flip();
        if(!spin)
        {
            moveSpeed = -beforeSpeed;
        }
        else
        {
            moveSpeed = -moveSpeed;
        }
    }

    public override void Dead()
    {
        throw new NotImplementedException();
    }

    public override void PickUp()
    {
        throw new NotImplementedException();
    }
    
}
