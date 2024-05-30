using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    public Transform LeftPoint, RightPoint;
    public float Speed, JumpForce;
    public LayerMask Ground;

    private Rigidbody2D Rb;
    //private Animator Anim;
    private Collider2D Coll;
    private bool FaceDirection; //Left = false, Right = true
    private float LeftPointX, RightPointX;


    protected override void Start()
    {   
        base.Start();
        Rb = GetComponent<Rigidbody2D>();
        //Anim = GetComponent<Animator>();
        Coll = GetComponent<Collider2D>();
        if (Rb == null || Anim == null || Coll == null)
        {
            Debug.LogError("Rigidbody2D 或 Animator 或 Collider2D 未正確初始化");
        }   

        LeftPointX = LeftPoint.position.x; //获取左右边界
        RightPointX = RightPoint.position.x;
        Destroy (LeftPoint.transform.gameObject); //删除左右边界
        Destroy (RightPoint.transform.gameObject);
    }
    void Update()
    {
        AnimSwitch();
        
    }
    void Movement() //移动
    {
        if(!FaceDirection) //Left
        {   
            if(Coll.IsTouchingLayers(Ground))
            {
                Anim.SetBool("Jumping",true);
                Rb.velocity = new Vector2(-Speed, JumpForce);
            }
            if(transform.position.x < LeftPointX)
            {
                transform.localScale = new Vector3(-1, 1,1);
                FaceDirection = true;
                //Debug.Log("Change Direction");
            }
        }
        if(FaceDirection) //Right
        {
            if(Coll.IsTouchingLayers(Ground))
            {
                Anim.SetBool("Jumping",true);
                Rb.velocity = new Vector2(Speed, JumpForce);
            }
            if(transform.position.x > RightPointX)
            {
                transform.localScale = new Vector3(1, 1,1);
                FaceDirection = false;
            }
        } 
        
    }
    void AnimSwitch()
    {
        if(Anim.GetBool("Jumping"))
        {
            if(Rb.velocity.y < 0.1f)
            {
                Anim.SetBool("Jumping",false);
                Anim.SetBool("Falling",true);
            }
        }
        if(Coll.IsTouchingLayers(Ground) && Anim.GetBool("Falling"))
        {
            Anim.SetBool("Falling",false);
            Anim.SetBool("Idle",true);
        }
    }
    
}
