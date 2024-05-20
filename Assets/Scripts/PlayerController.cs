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
    
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        if (Rb == null || Anim == null)
        {
            Debug.LogError("Rigidbody2D 或 Animator 未正確初始化");
            return;
        }
    }
    void FixedUpdate()
    {
        Movement();
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
        if((Input.GetKeyDown(KeyCode.Space)) && (Coll.IsTouchingLayers(Ground)))//跳跃(一次)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, JumpForce);
            Anim.SetBool("Jumping",true); //跳跃动画
        }
        
    }
    void AnimSwitch() //動畫切換
    {
        if(Rb.velocity.y < 0) //下落
        {
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
        if(Anim.GetBool("Falling"))
        {       
            if(collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
                Anim.SetBool("Jumping",true);
                Rb.velocity = new Vector2(Rb.velocity.x, JumpForce);
            }
        }
    }
}