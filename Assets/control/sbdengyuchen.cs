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
    public bool isHurt;//Ĭ����false
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()//fixedupdate���Ը���
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
            rb.velocity = new Vector2(Horizontalmove*speed, rb.velocity.y);//�ƶ�����
            anim.SetFloat("running", Mathf.Abs(facedirection));                                       //time.deltatime���Ը�ƽ������֡
        }
        if (facedirection != 0)
        {
            transform.localScale=new Vector3(facedirection,1,1);//ת�����
        }
    }
    void JumpDone()
    {
        // ��Ծ�����������ʱ��animatorִ���ⲿ������
        anim.SetBool("jumping", false); // ����Ծ״̬����Ϊfalse�����𶯻���jump->idel��transitition
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump")&& coll.IsTouchingLayers(groud))
        {
            // ����������Բ���time.deltatime����Ϊ(���Ͼ��Ѿ����ٶȵĻ�unity��ƽ������ģ�ֱ���޸�����������time.deltaTime�ȽϺ���)
            rb.velocity = new Vector2(rb.velocity.x, forcejump);//��Ծ����
            anim.SetBool("jumping", true);  //x�ᾲֹ(׼ȷ˵�ǲ��ı䣬��ֹΪ0)
            //�����Ծ��ťʱ����ж���Ч����set�����ö���Ч��
        }
        Switch();
    }
    void Switch()
    {
        //? ���������ȫû�б����ÚG ��Ȼ��ȫ������Ч��
        if (coll.IsTouchingLayers(groud))//��������˵ذ��վ��
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
        if (collision.tag == "collection")//���������tag��collection������ ������ʧ
        {
            Destroy(collision.gameObject);
            acorn += 1;
        }
    }

    //�ڳ�����������֣�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"encounter:{collision.gameObject.tag},{collision.contacts[0].normal.y}");
        if (collision.gameObject.tag == "enemies")
        {
            if (collision.contacts[0].normal.y == 1)//��Ϊ����һ������Ŀ ������gameObject
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
