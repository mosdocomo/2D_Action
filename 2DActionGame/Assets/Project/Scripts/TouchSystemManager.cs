using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TouchSystemManager
{
    public class TouchManager : MonoBehaviour
    {
        private const int maxTouches = 2;                  // 画面の最大タッチ数
        private static TouchSystemManagerProperty[] touchManagerPropertys;       // 指の最大数分の情報を格納するtouchManagerクラス
        private bool firstTouch = true;


        /**
         * コンストラクタ
         * 
         * @access public
         * 
         */
        public TouchManager()
        {
            touchManagerPropertys = new TouchSystemManagerProperty[maxTouches]
            {
                new TouchSystemManagerProperty{touchFlag = false, touchPosition = Vector2.zero, nowTouchPhase = TouchPhase.Began },
                new TouchSystemManagerProperty{touchFlag = false, touchPosition = Vector2.zero, nowTouchPhase = TouchPhase.Began }
            };
            Debug.Log(touchManagerPropertys[0].touchFlag);
        }

        /**
         *  タッチ情報の更新処理
         * 
         * @access public
         * 
         */
        public void TouchUpdate()
        {
            // エディタ判定
            if (Application.isEditor)
            {
                touchManagerPropertys[0].touchFlag = false;

                // 左クリックを押下処理
                if (Input.GetMouseButtonDown(0))
                {
                    touchManagerPropertys[0].touchFlag = true;
                    touchManagerPropertys[0].nowTouchPhase = TouchPhase.Began;
                    touchManagerPropertys[0].touchPosition = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    touchManagerPropertys[0].touchFlag = true;
                    touchManagerPropertys[0].nowTouchPhase = TouchPhase.Ended;
                    touchManagerPropertys[0].touchPosition = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0))
                {
                    touchManagerPropertys[0].touchFlag = true;
                    touchManagerPropertys[0].nowTouchPhase = TouchPhase.Moved;
                    touchManagerPropertys[0].touchPosition = Input.mousePosition;
                }
            }
            else
            {
                // 実機処理
                if (Input.touchCount > 0)
                {
                    Touch[] touches = Input.touches;
                    if (firstTouch)
                    {
                        for (var i = 0; i < maxTouches; i++)
                        {
                            touchManagerPropertys[i].touchFlag = true;
                            touchManagerPropertys[i].touchPosition = touches[i].position;
                            touchManagerPropertys[i].nowTouchPhase = touches[i].phase;

                        }
                        // １番最初のタッチが離れて、２番目のタッチが存在する場合
                        if (touches[0].phase == TouchPhase.Ended && Input.touchCount >= 2)
                        {
                            firstTouch = false;
                        }
                    }
                    else
                    {
                        int j = 1;
                        for (var i = 0; i < maxTouches; i++)
                        {
                            touchManagerPropertys[j].touchFlag = true;
                            touchManagerPropertys[j].touchPosition = touches[i].position;
                            touchManagerPropertys[j].nowTouchPhase = touches[i].phase;
                            j--;
                        }
                    }
                }
                else
                {
                    // 全てのタッチが離れた場合
                    firstTouch = true;
                }
            }
        }

        /**
         *  タッチ情報の取得
         * 
         * @access public
         * 
         */
        public TouchSystemManagerProperty[] getTouchManager()
        {
            return touchManagerPropertys;
        }


        /**
         *  タッチ最大数の取得
         * 
         * @access public
         * 
         */
        public int getMaxTouches()
        {
            return maxTouches;
        }
    }
}