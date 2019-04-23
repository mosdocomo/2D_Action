using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchSystemManager;

public class TestGrilMoving : MonoBehaviour
{
    TouchManager touchManager;
    TouchSystemManagerProperty[] touchPropertys;

    private Vector2[] startPosition;
    private Vector2[] endPosition;
    private float[] beganTime;

    // Start is called before the first frame update
    void Start()
    {
        touchManager = new TouchManager();
        touchPropertys = touchManager.getTouchManager();
        startPosition = new Vector2[2];
        endPosition = new Vector2[2];
        beganTime = new float[2];
    }

    // Update is called once per frame
    void Update()
    {
        touchManager.TouchUpdate();
        touchPropertys = touchManager.getTouchManager();

        for (int i = 0; i < touchManager.getMaxTouches(); i++)
        {
            if (touchPropertys[i].touchFlag)
            {
                if (touchPropertys[i].nowTouchPhase == TouchPhase.Began)
                {
                    Debug.Log("ポジション：" + touchPropertys[i].touchPosition);

                    startPosition[i] = touchPropertys[i].touchPosition;
                    beganTime[i] = 0;
                    Debug.Log("マウス押下");
                }
                else if (touchPropertys[i].nowTouchPhase == TouchPhase.Moved)
                {
                    endPosition[i] = touchPropertys[i].touchPosition;

                    // 移動ベクトル方向取得
                    Vector2 vct = (endPosition[i] - startPosition[i]).normalized;
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = vct * 2.0f;

                    beganTime[i] += Time.deltaTime;
                    Debug.Log("マウス長押し　vecter：" + vct);
                }
                else if (touchPropertys[i].nowTouchPhase == TouchPhase.Ended)
                {
                    endPosition[i] = touchPropertys[i].touchPosition;
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    Debug.Log("マウスはなす");
                }
            }
        }
    }
}
