using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //[SerializeField]
    private Rigidbody2D Rb; 
    private Animator Anim;

    public Collider2D Coll;
    public LayerMask Ground;
    public TextMeshProUGUI CherryNum;

    public float Speed;
    public float JumpForce;
    public int Cherry;

    private bool IsHurt;
    private bool IsGrounded;
    
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        if (Rb == null || Anim == null)
        {
            Debug.LogError("Rigidbody2D 或 Animator 未正確初始化");
        }
    }
    void FixedUpdate()
    {
        if(!IsHurt)
        {
            Movement();
        }
        if(Mathf.Abs(Rb.velocity.x) < 0.1f && Mathf.Abs(Rb.velocity.y) < 0.1f) //受伤后恢复
        {
            IsHurt = false;
            Anim.SetBool("Hurting",false);
            Anim.SetBool("Idle",true);
            Anim.SetFloat("Running",0);
            
        }
        IsGrounded = Coll.IsTouchingLayers(Ground);
    }
    void Update()
    {
        Jump();
        AnimSwitch();
        
    }

    void Movement() //移动
    {
        float HorizontalMove = Input.GetAxis("Horizontal");
        float FaceDirection = Input.GetAxisRaw("Horizontal");
        if(HorizontalMove != 0) //水平移动
        {
            Rb.velocity = new Vector2(HorizontalMove * Speed , Rb.velocity.y);
            Anim.SetFloat("Running",Mathf.Abs(FaceDirection)); //奔跑动画
        }
        if(FaceDirection != 0) //角色朝向
        {
            transform.localScale = new Vector3(FaceDirection, 1, 1);
        }
    }
    void Jump() //跳跃和下落
    {
        if((Input.GetKeyDown(KeyCode.Space)) && IsGrounded)//跳跃(一次)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, JumpForce);
            Anim.SetBool("Jumping",true); //跳跃动画
        }
        
    }
    void AnimSwitch() //動畫切換
    {
        if(Rb.velocity.y < 0) //下落
        {
            Anim.SetBool("Idle",false);
            Anim.SetBool("Jumping",false);
            Anim.SetBool("Falling",true);
        }
        else
        {
            Anim.SetBool("Falling",false);
        }             
        if(Coll.IsTouchingLayers(Ground)) //着地
        {
            Anim.SetBool("Idle",true); //站立动画
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision) //碰到收集物品
    {
       if(collision.gameObject.CompareTag("Collection")) 
       {
           Destroy(collision.gameObject);
           Cherry += 1;
           CherryNum.text = Cherry.ToString();
       }
    }
    private void OnCollisionEnter2D(Collision2D collision) //碰到敌人
    {   
        if(collision.gameObject.CompareTag("Enemy")) 
        {       
            if(Anim.GetBool("Falling")) //踩敌人
            {
                Destroy(collision.gameObject);
                Rb.velocity = new Vector2(Rb.velocity.x, JumpForce);
                Anim.SetBool("Jumping",true);
                Anim.SetFloat("Running",0);
                Anim.SetBool("Idle",false);
                Anim.SetBool("Hurting",false);
                Anim.SetBool("Falling",false);
            }
            else
            {
                IsHurt = true;
                Anim.SetBool("Hurting",true);
                Anim.SetBool("Idle",false);
                Anim.SetFloat("Running",0);

                if(transform.position.x < collision.gameObject.transform.position.x) //受伤
                {
                    Rb.velocity = new Vector2(-8, Rb.velocity.y);
                }
                else if(transform.position.x > collision.gameObject.transform.position.x) //受伤
                {
                    Rb.velocity = new Vector2(8, Rb.velocity.y);
                }
            }
        }
    }
}   