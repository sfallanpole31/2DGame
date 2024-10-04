using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpforce;
    Rigidbody2D MyRig; //宣告物件的Rigbody
    Animator MyAnimator;//動畫

    //判斷有無在地面上
    public bool IsGround; //宣告布林 
    public Transform Checker; // 檢查器的Transform
    float chackRadius = 1f; //檢查器的範圍大小
    public LayerMask GroundLayer; //地板的圖層


    //連擊
    bool combo1, combo2;
    public bool bComboCold;
    public float fColdTime;
    public BoxCollider2D boxCollider;

    //受傷狀態
    public bool isHurt;

    //血量
    public float HP;
    public Sprite[] Hp_sprite;
    public Image UI_HP;

    //螢幕晃動
    public Animator Cam_anim;


    //火球法術
    public GameObject fireBall;
    public Transform shootPoint;

    //被擊飛 
    public Rigidbody2D rb;
    public float knockbackForce;  // 力的大小
    public bool knockingBack;
    private float knockbackEndTime;
    public float knockbackDuration = 3f;  // 击飞持续时间

    void Start()
    {
        CheckHPUI();
        speed = 5;
        jumpforce = 5;
        MyRig = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGround) //跳躍功能
        {
            MyRig.velocity = Vector2.up * jumpforce;
            MyAnimator.SetBool("IsGround", false);
        }

        #region 火球法術
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject fireBall_ = Instantiate(fireBall, shootPoint.transform.position, fireBall.gameObject.transform.rotation);
            fireBall_.GetComponent<Weapon>().FallSpeed = transform.localScale.x * 10;
            fireBall_.transform.localScale = new Vector3(transform.localScale.x, fireBall.transform.localScale.y, fireBall.transform.localScale.z);
        }
        #endregion

        #region Combo普通攻擊
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Z))
        {

            if (isHurt)
            { return; }

            if (!combo1 && !combo2)
            {
                MyAnimator.Play("Player Attack");
                combo1 = true;
                bComboCold = true;

            }
            else if (combo1 && !combo2)
            {
                MyAnimator.Play("Player Attack 2");
                combo2 = true;
                bComboCold = true;
                fColdTime = 0;
            }
            else if (combo1 && combo2)
            {
                MyAnimator.Play("Player Attack 3");

                bComboCold = false;
                fColdTime = 0;
                combo1 = false;
                combo2 = false;
            }
        }
        if (bComboCold)
        {
            fColdTime += Time.deltaTime;
            if (fColdTime > 0.5f)
            {
                combo1 = false;
                combo2 = false;
                fColdTime = 0;
                bComboCold = false;

            }
        }
        #endregion

    }

    private void FixedUpdate()
    {
        IsGround = Physics2D.OverlapCircle(Checker.position, chackRadius, GroundLayer);

        MyAnimator.SetBool("IsGround", IsGround);

        float move = Input.GetAxis("Horizontal");
        float face = Input.GetAxisRaw("Horizontal");
        MyRig.velocity = new Vector2(speed * move, MyRig.velocity.y);  //角色移動

        if (face != 0) //角色面向
        {
            transform.localScale = new Vector3(face, 1, 1);
        }

        MyAnimator.SetFloat("move", Mathf.Abs(move)); //待機 走路切換 

        #region 被擊飛
        if (knockingBack)
        {
            if (rb != null)
            {
                print("被擊飛" + rb.ToString());
                knockbackEndTime = Time.time + knockbackDuration;

                // 设置新的速度，击飞角色
                rb.AddForce(new Vector2(-knockbackForce * 20 * transform.localScale.x, knockbackForce), ForceMode2D.Impulse);
                // 调试信息：打印新的速度

            }
            // 检查是否应该结束击飞状态
            if (Time.time >= knockbackEndTime)
            {
                knockingBack = false;
            }
        }
        #endregion

    }

    private void OnCollisionEnter2D(Collision2D collision) //collision可碰撞 、Trigger是穿透
    {

        if (collision.gameObject.tag == "Enemy")
        {
            knockingBack = true;

            MyAnimator.Play("Player hurt");
            PlayerHurt();
            Cam_anim.Play("Cam_Shake");

        }
    }


    public void HurtLayer()
    {

        // Debug.Log("執行HurtLayer");
        isHurt = true;
        MyAnimator.SetBool("isHurt", true);
        Cam_anim.SetBool("isHurt", true);
        this.gameObject.layer = 9;

    }

    public void ResetLayer()
    {
        //Debug.Log("執行ResetLayer");
        isHurt = false;
        MyAnimator.SetBool("isHurt", false);
        Cam_anim.SetBool("isHurt", false);
        this.gameObject.layer = 7;
    }

    // 方法用於開啟BoxCollider
    public void EnableBoxCollider()
    {
        boxCollider.enabled = true;
    }

    // 方法用於關閉BoxCollider
    public void DisableBoxCollider()
    {
        boxCollider.enabled = false;
    }

    //腳色受傷
    public void PlayerHurt()
    {
        HP -= 1;
        Debug.Log("腳色遭到1點傷害");
        CheckHPUI();
        if (HP <= 0)
        {
            Debug.Log("腳色死亡");
            Destroy(this.gameObject);
            FindObjectOfType<GameManager>().GameOverPanelShow();
            //GAME OVER
        }
    }

    public void CheckHPUI()
    {
        UI_HP.sprite = Hp_sprite[(int)HP];
    }

}
