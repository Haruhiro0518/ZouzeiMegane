using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    // マウスドラッグ処理
    private float previousPosX;
    private float currentPosX;
    
    // x座標をclampするときの最小値、最大値
    private float xMin;
    private float xMax;

    // player size
    private float radius;
    
    // display size
    private float displayWidth = 2.8f;

    private int layermask;
    
    void Start()
    {
        // プレイヤーの半径
        radius = GetComponent<Transform>().transform.localScale.x / 2;
        // radius = GetComponent<CircleCollider2D>().radius;
        // playerのlayer7だけ無視する
        layermask = 1 << 7;
        layermask = ~layermask;
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

            // 次のローカルx座標を設定  CLAMP
            // 左右にcast a ray
            
            RaycastHit2D L = Physics2D.Raycast(transform.position, Vector2.left, displayWidth*2, layermask);
            RaycastHit2D R = Physics2D.Raycast(transform.position, Vector2.right, displayWidth*2, layermask);
            
            // 左にコライダーがあるとき
            if(L.collider != null) {
                // xMin = L.transform.position.x + L.transform.localScale.x / 2 + radius;
                xMin = L.point.x + radius;
                
            } else {
                xMin = -displayWidth + radius;
            }
            // 右にコライダーがあるとき
            if(R.collider != null) {
                // xMax = R.transform.position.x - R.transform.localScale.x / 2 - radius;
                xMax = R.point.x - radius;
            } else {
                xMax = displayWidth - radius;
                // Debug.Log(displayWidth-radius+":"+xMax);
            }
            
            Vector2 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x + diffDistance,  xMin,  xMax);
            
            transform.position = pos;
            

            // タップ位置を更新
            previousPosX = currentPosX;
        } 
    }


}
