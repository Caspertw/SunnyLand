using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //[SerializeField]
    private Rigidbody2D rb; 
    private Animator anim;
    public AudioSource jumpAudio,hurtAudio,collectAudio;

    public Collider2D coll;
    public LayerMask ground;
    public TextMeshProUGUI cherryNum;

    public float speed;
    public float jumpForce;
    public int cherry;

    private bool isHurt;
    private bool isGrounded;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //jumpAudio = GetComponent<AudioSource>();
        if (rb == null || anim == null)
        {
            Debug.LogError("Rigidbody2D 或 Animator 未正確初始化");
        }
    }
    void FixedUpdate()
    {
        if(!isHurt)
        {
            Movement();
        }
        if(Mathf.Abs(rb.velocity.x) < 0.1f && Mathf.Abs(rb.velocity.y) < 0.1f) //受伤后恢复
        {
            isHurt = false;
            anim.SetBool("Hurting",false);
            anim.SetBool("Idle",true);
            anim.SetFloat("Running",0);
            
        }
        isGrounded = coll.IsTouchingLayers(ground);
    }
    void Update()
    {
        Jump();
        AnimSwitch();
    }

    void Movement() //移动
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");
        if(horizontalMove != 0) //水平移动
        {
            rb.velocity = new Vector2(horizontalMove * speed , rb.velocity.y);
            anim.SetFloat("Running",Mathf.Abs(faceDirection)); //奔跑动画
        }
        if(faceDirection != 0) //角色朝向
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
    }
    void Jump() //跳跃和下落
    {
        if((Input.GetKeyDown(KeyCode.Space)) && isGrounded)//跳跃(一次)
        {
            jumpAudio.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("Jumping",true); //跳跃动画
        }
        
    }
    void AnimSwitch() //動畫切換
    {
        if(rb.velocity.y < 0) //下落
        {
            anim.SetBool("Idle",false);
            anim.SetBool("Jumping",false);
            anim.SetBool("Falling",true);
        }
        else
        {
            anim.SetBool("Falling",false);
        }             
        if(coll.IsTouchingLayers(ground)) //着地
        {
            anim.SetBool("Idle",true); //站立动画
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision) //觸發器
    {
       if(collision.gameObject.CompareTag("Collection")) //吃樱桃
       {
           Destroy(collision.gameObject);
           cherry += 1;
           cherryNum.text = cherry.ToString();
           collectAudio.Play();
       }
       if(collision.gameObject.CompareTag("DeadLine")) //死亡
       {
           //GetComponent<AudioSource>().enabled = false;
           Invoke("Restart",1f); //延迟1秒
       }
    }
    private void OnCollisionEnter2D(Collision2D collision) //碰到敌人
    {   
        if(collision.gameObject.CompareTag("Enemy")) 
        {       
            if(anim.GetBool("Falling")) //踩敌人
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>(); //調用Enemy腳本
                enemy.JumpOn();

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("Jumping",true);
                anim.SetFloat("Running",0);
                anim.SetBool("Idle",false);
                anim.SetBool("Hurting",false);
                anim.SetBool("Falling",false);
            }
            else
            {
                isHurt = true;
                anim.SetBool("Hurting",true);
                anim.SetBool("Idle",false);
                anim.SetFloat("Running",0);
                hurtAudio.Play();

                if(transform.position.x < collision.gameObject.transform.position.x) //受伤
                {
                    rb.velocity = new Vector2(-8, rb.velocity.y);
                }
                else if(transform.position.x > collision.gameObject.transform.position.x) //受伤
                {
                    rb.velocity = new Vector2(8, rb.velocity.y);
                }
            }
        }
    }
    void Restart() //重新开始
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        Debug.Log("Restart");
    }
}   