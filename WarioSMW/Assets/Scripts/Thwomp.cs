using UnityEngine;
using System.Collections;
using System;

public class Thwomp : MonoBehaviour
{

    private Rigidbody2D rb2d;
    public float fallSpeed = -8;
    public float riseSpeed = 2;

    private bool grounded;
    private bool suspicious;
    private bool crush;

    private Collider2D hitboxCol;
    private Collider2D triggerCol;
    private Animator anim;

    private float top, left, right;

    // Use this for initialization
    void Start()
    {
        triggerCol = transform.Find("Trigger Collider").GetComponent<Collider2D>();
        right = triggerCol.bounds.max.x - 1;
        left = triggerCol.bounds.min.x + 1;

        hitboxCol = transform.Find("Hitbox Collider").GetComponent<Collider2D>();
        top = hitboxCol.bounds.center.y;

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > top)
        {
            rb2d.position = new Vector2(rb2d.position.x, top);
            rb2d.velocity = new Vector2();
            grounded = false;
        }

        anim.SetBool("Suspicious", suspicious);
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Crush", crush);
    }

    void MoveUp()
    {
        rb2d.velocity = new Vector2(0, riseSpeed);
    }

    void MoveDown()
    {
        rb2d.velocity = new Vector2(0, fallSpeed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            grounded = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.transform.tag == "Player")
        {
            Transform player = col.transform.parent;
            if(player.position.x < right && player.position.x > left)
            {
                if (!grounded)
                {
                    MoveDown();
                    suspicious = false;
                    crush = true;
                }
            }
            else
            {
                suspicious = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        suspicious = false;
    }

    public void Wait()
    {
        MoveUp();
        crush = false;
    }
}
