using UnityEngine;
using System.Collections;
using System;

public class Player : Controller2D
{

    public float walkJumpSpeed = 22f;
    public float runJumpSpeed = 24f;
    public float runJumpThreshold = 3;
    public float walkSpeed = 4f;
    public float runSpeed = 9f;
    public float gravity = -65;
    public float maxFall = -12;
    public float accelerationTimeAirborne = .4f;
    public float accelerationTimeGrounded = .4f;
    public int invincibleLength = 5;

    private float velocityXSmoothing;
    private float targetVelocityX;
    public float moveSpeed;
    private float jumpSpeed;

    private bool grounded;
    private bool falling;
    private bool turnAround;
    private bool lookUp;
    private bool duck;
    private bool onEnemy;
    public bool pickUp;
    public bool toss;
    private bool jump;
    private bool win;
    private bool invincible;

    private int invincibleCount;

    private string activePlayer = "Player Small";

    private Animator anim;

    private BoxCollider2D collider;
    private BoxCollider2D idleCol;
    private BoxCollider2D duckCol;
    private Koopa enemy;

    public LayerMask ground;

    public Vector2 velocity;

    void Start()
    {
        SetActivePlayer();
    }

    void Update()
    {
        HandleInputs();

        Move(velocity * Time.deltaTime, collider, ground);

        PlayAnimations();
    }

    private void SetActivePlayer()
    {
        Transform active = transform.Find(activePlayer);

        active.gameObject.SetActive(true);
        anim = active.GetComponent<Animator>();
        idleCol = active.Find("Idle Collider").GetComponent<BoxCollider2D>();
        duckCol = active.Find("Duck Collider").GetComponent<BoxCollider2D>();
        collider = idleCol;
    }

    private void HandleInputs()
    {
        Vector2 input = new Vector2();

        if (!win)
        {
            if ((!duck || jump && Mathf.Abs(velocity.x) < 1))
            {
                input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }

            if (input.x < 0)
            {
                if (facingRight)
                {
                    TurnAround();
                }
            }
            else if (input.x > 0)
            {
                if (!facingRight)
                {
                    TurnAround();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && collisions.below)
            {
                Jump();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                VariableJump();
            }


            if (collisions.below)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Run();
                }
                else
                {
                    Walk();
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Duck();
            }

            if (duck && !Input.GetKey(KeyCode.DownArrow))
            {
                StandUp();
            }

            if (Input.GetKey(KeyCode.UpArrow) && Mathf.Abs(velocity.x) < 1)
            {
                lookUp = true;
            }
            else
            {
                lookUp = false;
            }

            //if (Input.GetKeyUp(KeyCode.LeftShift) && pickUp)
            //{
            //    Throw();
            //}

            targetVelocityX = input.x * moveSpeed;
        }


        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.y = Mathf.Max(velocity.y + gravity * Time.deltaTime, -maxFall);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Blue Block")
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("Pick Up", true);
                col.gameObject.GetComponent<ExplodingBlocks>().BeginExplode();
            }
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if(activePlayer == "Player Small")
            {
                if(!invincible)
                {
     //               Dead();
                }
            }
            else
            {
                anim.applyRootMotion = false;
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
                Time.timeScale = 0;

                anim.SetBool("Power Down", true);
            }
            
            //enemy = col.gameObject.GetComponent<Koopa>();
            //if (!enemy.duck || enemy.spin)
            //{
            //    enemy.Duck();
            //    velocity.y += 10;
            //}
            //else
            //{
            //    if (collider.bounds.center.x >= enemy.col.bounds.center.x)
            //    {
            //        enemy.Spin("Left");
            //    }
            //    else
            //    {
            //        enemy.Spin("Right");
            //    }
            //}
        }
        else if (col.gameObject.tag == "Death")
        {
            Dead();
        }
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if(!invincible)
            {
       //         Dead();
            }
        }
        if (col.gameObject.tag == "Mushroom")
        {
            if (activePlayer == "Player Small")
            {
                anim.applyRootMotion = false;
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
                Time.timeScale = 0;

                anim.SetTrigger("Power Up");                
            }
            Destroy(col.gameObject);
        }
    }

    void PlayAnimations()
    {
        if (collisions.left || collisions.right)
        {
            velocity.x = 0;
        }

        if (collisions.above || collisions.below)
        {
            grounded = true;
            falling = false;
            jump = false;
            velocity.y = 0;
        }

        if (velocity.y < -1f)
        {
            jump = true;
            grounded = false;
            falling = true;
        }

        if (velocity.y > 0)
        {
            jump = true;
            falling = false;
            grounded = false;
        }

        anim.SetBool("Duck", duck);
        anim.SetBool("Look Up", lookUp);
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Falling", falling);
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));
    }

    public override void Jump()
    {
        if (Mathf.Abs(velocity.x) > runJumpThreshold)
        {
            RunJump();
        }
        else
        {
            WalkJump();
        }

        velocity.y = jumpSpeed;

        anim.SetTrigger("Jump");
    }

    public void RunJump()
    {
        jumpSpeed = runJumpSpeed;
    }

    public void WalkJump()
    {
        jumpSpeed = walkJumpSpeed;

    }

    public override void Run()
    {
        moveSpeed = runSpeed;
    }


    public override void Duck()
    {
        duck = true;

        transform.position = new Vector3(transform.position.x, transform.position.y - .15f, transform.position.z);

        duckCol.gameObject.SetActive(true);
        idleCol.gameObject.SetActive(false);

        collider = duckCol;
    }

    public override void StandUp()
    {
        if (!MultiRayCast2D.Up(collider, .15f, 4, .015f, ground))
        {
            duck = false;

            transform.position = new Vector3(transform.position.x, transform.position.y + .15f, transform.position.z);

            duckCol.gameObject.SetActive(false);
            idleCol.gameObject.SetActive(true);

            collider = idleCol;
        }
    }

    public override void Walk()
    {
        moveSpeed = walkSpeed;
    }

    public override void TurnAround()
    {
        Flip();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Fast Walk") || anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            anim.SetTrigger("Turn Around");
        }
    }    

    public void VariableJump()
    {
        if (velocity.y > 8)
        {
            velocity.y = 8;
        }
    }

    public void Throw()
    {
        pickUp = false;
        toss = true;
    }

    public override void Dead()
    {
        ground = 0;
        anim.applyRootMotion = false;
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0;
        anim.SetBool("Dead", true);
    }

    public override void PickUp()
    {
        //StandUp();
        //pickUp = true;
        //velocity.y = 0;
        //anim.SetBool("Pick Up", pickUp);
    }

    public void Win()
    {
        win = true;
        if (!facingRight)
        {
            TurnAround();
        }
        targetVelocityX = 2;
        
        anim.SetBool("Win", win);
    }

    public void ChangePlayer()
    {
        anim.applyRootMotion = true;
        anim.updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = 1;

        transform.Find(activePlayer).gameObject.SetActive(false);
        if (activePlayer == "Player Small")
        {
            activePlayer = "Player Big";
        }
        else
        {
            activePlayer = "Player Small";
            InvokeRepeating("TurnOff", 0, .5f);
            InvokeRepeating("TurnOn", .25f, .5f);
            invincible = true;
        }
        
        SetActivePlayer();

        anim.SetBool("Power Down", false);

        if (duck)
        {
            Duck();
        }
    }

    public void FinishWin()
    {
        targetVelocityX = 0;
    }

    private void TurnOn()
    {
        invincibleCount++;
        transform.Find(activePlayer).GetComponent<SpriteRenderer>().enabled = true;
        if(invincibleCount >= invincibleLength)
        {
            invincibleCount = 0;
            invincible = false;
            CancelInvoke();
        }
    }

    private void TurnOff()
    {
        transform.Find(activePlayer).GetComponent<SpriteRenderer>().enabled = false;
    }

}
