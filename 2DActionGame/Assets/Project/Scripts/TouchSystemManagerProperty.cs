using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TouchSystemManager
{
    public class TouchSystemManagerProperty : MonoBehaviour
    {
        // 画面がタッチされているか判定
        public bool touchFlag { set; get; }
        
        // タッチされた情報の配列
        public Vector2 touchPosition { set; get; }

        // タッチフェーズの状態
        public TouchPhase nowTouchPhase { set; get; }
    }
}


