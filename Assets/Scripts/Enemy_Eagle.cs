using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;
    //private Animator anim;
    private Collider2D coll;
    public Transform upPoint, downPoint;
    public float speed;
    private float upY, downY;

    private bool faceUp = true;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        upY = upPoint.position.y;
        downY = downPoint.position.y;
        Destroy(upPoint.gameObject);
        Destroy(downPoint.gameObject); 
    }

    void Update()
    {
        Movement();
    }
    void Movement()
    {
        if (faceUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > upY)
            {
                faceUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < downY)
            {
                faceUp = true;
            }
        }
    }
}
