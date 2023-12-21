using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // マウスドラッグ処理
    private float previousPosX;
    private float currentPosX;

    // player size
    private float radius;
    
    // display size
    private float displayWidth = 2.8f;
    
    void Start()
    {
        // プレイヤーの半径
        radius = GetComponent<Transform>().transform.localScale.x / 2;
    }

    
    void Update()
    {
        Move();
    }

    // プレイヤーをドラッグで移動
    void Move() 
    {
        // mouse左ボタンを押したとき
        if (Input.GetMouseButtonDown(0)) {
            previousPosX = Input.mousePosition.x;
        }
        // mouse左ボタンを押している間
        if(Input.GetMouseButton(0)) {
            
            currentPosX = Input.mousePosition.x;
            // 移動距離の計算　Screen.widthで割って1に正規化. 定数をかけて,マウスの移動とplayerが一致
            float diffDistance = (currentPosX - previousPosX) / Screen.width * displayWidth*2;
            // 次のローカルx座標を設定
            Vector2 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x + diffDistance,  -displayWidth + radius,  displayWidth - radius);
            transform.position = pos;

            // タップ位置を更新
            previousPosX = currentPosX;
        }
    }


}
