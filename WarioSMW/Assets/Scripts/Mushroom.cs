using UnityEngine;
using System.Collections;
using System;

public class Mushroom : Controller2D {

    public float moveSpeed = 2;
    public float gravity = -65;
    public float maxFall = 12;

    private Collider2D col;

    public LayerMask ground;

    private Vector2 velocity;

    // Use this for initialization
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collisions.right)
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

        Move(velocity * Time.smoothDeltaTime, col, ground);
    }

    public override void TurnAround()
    {
        moveSpeed = -moveSpeed;
    }

    public override void Walk()
    {
        velocity.x = moveSpeed;
    }

    public override void Dead()
    {
        throw new NotImplementedException();
    }

    public override void Duck()
    {
        throw new NotImplementedException();
    }

    public override void Jump()
    {
        throw new NotImplementedException();
    }

    public override void PickUp()
    {
        throw new NotImplementedException();
    }

    public override void Run()
    {
        throw new NotImplementedException();
    }

    public override void StandUp()
    {
        throw new NotImplementedException();
    }        
}
