using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sbdengyuchen : MonoBehaviour
{
    public Rigidbody2D rb; //ok
    public float speed;
    public float forcejump;
    public Animator anim;
    public LayerMask groud;
    public Collider2D coll;
    public int acorn=0;
    public bool isHurt;//默认是false
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()//fixedupdate可以更好
    {
        if (!isHurt) 
        {
            Movement(); 
        }
        
    }
    void Movement() {
        float Horizontalmove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");
        if (Horizontalmove!=0) 
        {
            rb.velocity = new Vector2(Horizontalmove*speed, rb.velocity.y);//移动代码
            anim.SetFloat("running", Mathf.Abs(facedirection));                                       //time.deltatime可以更平滑不跳帧
        }
        if (facedirection != 0)
        {
            transform.localScale=new Vector3(facedirection,1,1);//转身代码
        }
    }
    void JumpDone()
    {
        // 跳跃动画播放完毕时让animator执行这部分内容
        anim.SetBool("jumping", false); // 把跳跃状态重置为false，引起动画机jump->idel的transitition
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump")&& coll.IsTouchingLayers(groud))
        {
            // 或许这里可以不用time.deltatime，因为(唔，毕竟已经是速度的话unity会平滑处理的，直接修改坐标那种用time.deltaTime比较合适)
            rb.velocity = new Vector2(rb.velocity.x, forcejump);//跳跃代码
            anim.SetBool("jumping", true);  //x轴静止(准确说是不改变，静止为0)
            //点击跳跃按钮时会进行动画效果，set是设置动画效果
        }
        Switch();
    }
    void Switch()
    {
        //? 这个函数完全没有被调用欸 自然完全不会起效果
        if (coll.IsTouchingLayers(groud))//如果碰到了地板就站立
        {
            anim.SetBool("jumping",false);
            anim.SetBool("idel", true);
        }  
        if (isHurt)
        {
            anim.SetBool("hurt",true);
            anim.SetFloat("running", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.5f)
            {
                anim.SetBool("hurt", false);
                isHurt = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "collection")//如果碰到了tag是collection的物体 物体消失
        {
            Destroy(collision.gameObject);
            acorn += 1;
        }
    }

    //钰晨防身术（打怪）
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"encounter:{collision.gameObject.tag},{collision.contacts[0].normal.y}");
        if (collision.gameObject.tag == "enemies")
        {
            if (collision.contacts[0].normal.y == 1)//因为调用一整个项目 所以先gameObject
            {

                Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, forcejump);
                anim.SetBool("jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                Debug.Log($"encounter:{collision.gameObject.tag},{collision.gameObject.name}");
                rb.velocity = new Vector2(-2, rb.velocity.y);
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                Debug.Log($"encounter:{collision.gameObject.tag},{collision.gameObject.name}");
                rb.velocity = new Vector2(2, rb.velocity.y);
                isHurt = true;
            }
        }
    }
}
