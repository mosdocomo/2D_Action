using System;
using System.Collections;
using System.Collections.Generic;
using TouchSystemManager;
using UnityEngine;

public abstract class PlayerController :MonoBehaviour
{
    [Header("[Setting]")]
    public float MoveSpeed = 6;
    public int JumpCount = 2;
    public float jumpForce = 15f;
    public Vector2 speedLv;     // 移動速度
    public Vector2 jumpLv;      // ジャンプ力

    TouchManager touchManager;
    TouchSystemManagerProperty[] touchPropertys;
    public Animator anime_Swordman;

    public bool isSit;
    public bool isAttack;
    public bool isMove;
    public bool isJump;
    public bool isGrounded;
    public bool isOnceJump;
    public bool isDownJump;   // 落下或いは着地の判定結果


    public int currentJumpCount = 0;

    protected float m_MoveX;
    public Rigidbody2D m_rigidbody;
    protected CapsuleCollider2D m_CapsulleCollider;




    // Start is called before the first frame update
    void Start()
    {
        speedLv = new Vector2(2.0f, 0.0f);  // コンポーネントに代入 
        jumpLv = new Vector2(300.0f, 0.0f); // addforceで飛ぶ

        touchManager = new TouchManager();
        touchPropertys = touchManager.getTouchManager();

        isSit = false;              // 座っている
        isAttack = false;           // 攻撃している
        isMove = false;             // 移動している
        isJump = false;             // ジャンプしている
        isGrounded = false;         // 地に足がついてる
        isOnceJump = false;         // 2段ジャンプができる
        isDownJump = false;         // 落下或いは着地の判定結果

}

    //********************************************************************************************
    //********************************************************************************************

    void Update()
    {
        touchManager.TouchUpdate();
        touchPropertys = touchManager.getTouchManager();

        for (var i = 0; i < touchManager.getMaxTouches(); i++)
        {
            if (touchPropertys[i].touchFlag)
            {
                if (touchPropertys[i].nowTouchPhase == TouchPhase.Began)
                {

                }
                else if (touchPropertys[i].nowTouchPhase == TouchPhase.Moved)
                {
                    isMove = true;
                }
                else if (touchPropertys[i].nowTouchPhase == TouchPhase.Ended)
                {

                }
            }
        }
    }

    private void FixedUpdate()
    {
        for (var i = 0; i < touchManager.getMaxTouches(); i++)
        {
            if (touchPropertys[i].nowTouchPhase == TouchPhase.Began && isMove)
            {
            }
            else if (touchPropertys[i].nowTouchPhase == TouchPhase.Moved && isMove)
            {
                // 移動ベクトル方向取得
                Vector2 vct = (touchPropertys[i].endTouchPosition - touchPropertys[i].startTouchPosition).normalized;
                float rad = Mathf.Rad2Deg * (Mathf.Atan2(vct.x, vct.y));    // 入力したベクトルを弧度法radとして再取得
                if (rad < 85.0f                         // X軸正移動
                    || rad > 275.0f                     // X軸正移動
                    || rad >= 95.0f && rad <= 265.0f)   // X軸負移動
                {
                    if (rad > 80.0f && rad < 100.0f)
                    {// 上入力判定（ジャンプ）
                        this.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpLv);
                    }
                    else if (rad > 260.0f && rad < 280.0f)
                    {// 下入力判定

                    }// 
                }
                this.gameObject.GetComponent<Rigidbody2D>().velocity = vct * 2.0f;

                Debug.Log("マウス長押し　vecter：" + vct);
            }
            else if (touchPropertys[i].nowTouchPhase == TouchPhase.Ended && isMove)
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                isMove = false;
            }
        }
    }

    //********************************************************************************************
    //********************************************************************************************


    protected void AnimUpdate()
    {
        if (!anime_Swordman.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {// アニメーションステートが"Attack"でない

            if (isAttack)
            {// 攻撃ボタンを押した

                anime_Swordman.Play("Attack");
            }
            else
            {// 攻撃ボタンを押してない

                if (isMove)
                {// 移動してる

                    anime_Swordman.Play("Run");
                }
                else
                {// 移動してない

                    if (!isJump) // ジャンプしてない
                        anime_Swordman.Play("Idle");

                }
            }
        }
    }

    protected void Filp(bool bLeft)
    {// キャラ向き反転

        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }


    protected void prefromJump()
    {// わからん
        anime_Swordman.Play("Jump");

        m_rigidbody.velocity = Vector2.zero;

        m_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        isOnceJump = true;
        isGrounded = false;


        currentJumpCount++;

    }

    protected void DownJump()
    {// わからん
        if (!isGrounded)
            return;


        if (!isDownJump)
        {
            anime_Swordman.Play("Jump");

            m_rigidbody.AddForce(-Vector2.up * 10);
            isGrounded = false;

            m_CapsulleCollider.enabled = false;

            StartCoroutine(GroundCapsulleColliderTimmerFuc());

        }
    }

    IEnumerator GroundCapsulleColliderTimmerFuc()
    {// わからん
        yield return new WaitForSeconds(0.3f);
        m_CapsulleCollider.enabled = true;
    }


    //////着地判定 
    // わからん
    Vector2 RayDir = Vector2.down;


    float PretmpY;
    float GroundCheckUpdateTic = 0;
    float GroundCheckUpdateTime = 0.01f;
    protected void GroundCheckUpdate()
    {
        if (!isOnceJump)
            return;

        GroundCheckUpdateTic += Time.deltaTime;

        if (GroundCheckUpdateTic > GroundCheckUpdateTime)
        {
            GroundCheckUpdateTic = 0;

            if (PretmpY == 0)
            {
                PretmpY = transform.position.y;
                return;
            }

            float reY = transform.position.y - PretmpY;  //    -1  - 0 = -1 ,  -2 -   -1 = -3

            if (reY <= 0)
            {
                if (isGrounded)
                {
                    LandingEvent();
                    isOnceJump = false;

                }
                else
                {

                }
            }
            PretmpY = transform.position.y;

        }
    }
    protected abstract void LandingEvent();

}
