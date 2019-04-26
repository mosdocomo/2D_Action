using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchSystemManager;

public class TestGrilMoving : MonoBehaviour
{
    public Vector2 speedLv;     // 移動速度
    public Vector2 jumpLv;      // ジャンプ力

    TouchManager touchManager;
    TouchSystemManagerProperty[] touchPropertys;
    public Animator anime_Swordman;

    private bool playerAttacking;
    private bool playerMoving;
    private bool playerJumping;
    // Start is called before the first frame update
    void Start()
    {
        speedLv = new Vector2(2.0f, 0.0f);  // コンポーネントに代入 
        jumpLv = new Vector2(300.0f, 0.0f); // addforceで飛ぶ

        touchManager = new TouchManager();
        touchPropertys = touchManager.getTouchManager();
        playerAttacking = false;
        playerMoving = false;
        playerJumping = false;
}

    protected void AnimUpdate()
    {
        if (!anime_Swordman.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (playerAttacking)
            {


                anime_Swordman.Play("Attack");
            }
            else
            {

                if (playerMoving)
                {

                    anime_Swordman.Play("Run");
                }
                else
                {
                    if (!playerJumping)
                        anime_Swordman.Play("Idle");

                }

            }



        }
    }

    // Update is called once per frame
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
                    playerMoving = true;
                }
                else if (touchPropertys[i].nowTouchPhase == TouchPhase.Ended)
                {
                    
                }
            }
        }
    }

    private void FixedUpdate()
    {
        for (var i = 0; i < touchManager.getMaxTouches(); i++){
            if (touchPropertys[i].nowTouchPhase == TouchPhase.Began && playerMoving)
            {
            }
            else if (touchPropertys[i].nowTouchPhase == TouchPhase.Moved && playerMoving)
            {
                // 移動ベクトル方向取得
                Vector2 vct = (touchPropertys[i].endTouchPosition - touchPropertys[i].startTouchPosition).normalized;
                float rad = Mathf.Rad2Deg * (Mathf.Atan2(vct.x, vct.y));    // 入力したベクトルを弧度法radとして再取得
                if (rad < 85.0f                         // X軸正移動
                    || rad > 275.0f                     // X軸正移動
                    || rad >= 95.0f && rad <= 265.0f)   // X軸負移動
                {
                    if(rad > 80.0f && rad < 100.0f)
                    {// 上入力判定（ジャンプ）
                        this.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpLv);
                    }
                    else if(rad > 260.0f && rad < 280.0f)
                    {// 下入力判定

                    }// 
                }
                this.gameObject.GetComponent<Rigidbody2D>().velocity = vct * 2.0f;
                    
                Debug.Log("マウス長押し　vecter：" + vct);
            }
            else if (touchPropertys[i].nowTouchPhase == TouchPhase.Ended && playerMoving)
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                playerMoving = false;
            }
        }
    }
}
