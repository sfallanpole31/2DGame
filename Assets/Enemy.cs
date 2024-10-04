using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject enemyPack;
    public Transform PosA, PosB;
    public float speed;
    Vector2 nextPos;
    public float HP ;

    public Animator cam;

    //擊飛
    public Rigidbody2D rb;
    public float knockbackForce;  // 力的大小
    public bool knockingBack;
    public bool isKnockBackRight;


    // Start is called before the first frame update
    void Start()
    {
        float number = Random.Range(0, 2);

        if (number == 0)
        {
            nextPos = PosA.position;
        }
        else
        {
            nextPos = PosB.position;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x == PosA.position.x)
        { 
            nextPos = PosB.position;
        }
        if (transform.position.x == PosB.position.x)
        { 
            nextPos = PosA.position;
        }

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(nextPos.x, transform.position.y), speed * Time.deltaTime);

        //被擊飛
        if (knockingBack)
        {
            print("被擊飛" + rb.ToString());
            if (isKnockBackRight == true)
            { rb.AddForce(new Vector2(knockbackForce  * transform.localScale.x, knockbackForce), ForceMode2D.Impulse); }
            else
                rb.AddForce(new Vector2(-knockbackForce  * transform.localScale.x, knockbackForce), ForceMode2D.Impulse);

            knockingBack = false;
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision) //當有觸發器碰到此物件
    {
        if (collision.tag == "Weapon")
        {
            //破壞火球
            if (collision.GetComponent<Weapon>().isMagic)
            {
                Destroy(collision.gameObject);
            }    

            float dmg = collision.GetComponent<Weapon>().damage;
            GetHurt(dmg);

            // 判斷碰撞發生在左側還是右側
            if (collision.transform.position.x < transform.position.x)
            {
                isKnockBackRight = true;
            }
            else if (collision.transform.position.x > transform.position.x)
            {
                isKnockBackRight = false;
            }

        }
    }

    public void GetHurt(float damage)
    {
        knockingBack = true;
        cam.Play("Cam_Shake");
        HP -= damage;
        FindObjectOfType<AudioSet>().PlaySfX(0);
        if (HP <= 0)
        {
            FindObjectOfType<AudioSet>().PlaySfX(1);
            Destroy(enemyPack);
        }
    }
}
