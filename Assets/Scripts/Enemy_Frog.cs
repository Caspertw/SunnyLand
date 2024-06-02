using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    public Transform leftPoint, rightPoint;
    public float speed, jumpForce;
    public LayerMask ground;

    private Rigidbody2D rb;
    //private Animator Anim;
    private Collider2D coll;
    private bool faceDirection; //Left = false, Right = true
    private float leftPointX, rightPointX;


    protected override void Start()
    {   
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //Anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        if (rb == null || Anim == null || coll == null)
        {
            Debug.LogError("Rigidbody2D 或 Animator 或 Collider2D 未正確初始化");
        }   

        leftPointX = leftPoint.position.x; //获取左右边界
        rightPointX = rightPoint.position.x;
        Destroy (leftPoint.transform.gameObject); //删除左右边界
        Destroy (rightPoint.transform.gameObject);
    }
    void Update()
    {
        AnimSwitch();
        
    }
    void Movement() //移动
    {
        if(!faceDirection) //Left
        {   
            if(coll.IsTouchingLayers(ground))
            {
                Anim.SetBool("Jumping",true);
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            if(transform.position.x < leftPointX)
            {
                transform.localScale = new Vector3(-1, 1,1);
                faceDirection = true;
                //Debug.Log("Change Direction");
            }
        }
        if(faceDirection) //Right
        {
            if(coll.IsTouchingLayers(ground))
            {
                Anim.SetBool("Jumping",true);
                rb.velocity = new Vector2(speed, jumpForce);
            }
            if(transform.position.x > rightPointX)
            {
                transform.localScale = new Vector3(1, 1,1);
                faceDirection = false;
            }
        } 
        
    }
    void AnimSwitch()
    {
        if(Anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < 0.1f)
            {
                Anim.SetBool("Jumping",false);
                Anim.SetBool("Falling",true);
            }
        }
        if(coll.IsTouchingLayers(ground) && Anim.GetBool("Falling"))
        {
            Anim.SetBool("Falling",false);
            Anim.SetBool("Idle",true);
        }
    }
    
}
