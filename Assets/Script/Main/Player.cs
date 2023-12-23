using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int layermask;

    // マウスドラッグ処理
    private float previousPosX;
    private float currentPosX;

    // x座標をclampするときの最小値、最大値
    private float xMin;
    private float xMax;

    // player size
    private float radius;
    
    // 横から接触しないための隙間
    private float space = 0.05f;

    // display size
    private float displayWidth = 2.8f;

    // スピード係数
    public float speed = 2.5f;

    // playerの攻撃力
    public int power = 1;



    

    void Start()
    {
        // プレイヤーの半径
        radius = GetComponent<Transform>().transform.localScale.x / 2;
        
        // playerのlayer7だけ無視する
        layermask = 1 << 7;
        layermask = ~layermask;

        Move();
    }

    
    void Update()
    {
        MoveDrag();
        
    }

    // プレイヤー移動
    void MoveDrag() 
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
            
            RaycastHit2D LU = Physics2D.Raycast(transform.position+new Vector3(0,radius-space,0), Vector2.left, displayWidth*2, layermask);
            RaycastHit2D RU = Physics2D.Raycast(transform.position+new Vector3(0,radius-space,0), Vector2.right, displayWidth*2, layermask);
            RaycastHit2D LD = Physics2D.Raycast(transform.position+new Vector3(0,-radius+space,0), Vector2.left, displayWidth*2, layermask);
            RaycastHit2D RD = Physics2D.Raycast(transform.position+new Vector3(0,-radius+space,0), Vector2.right, displayWidth*2, layermask);

            // 左にコライダーがあるとき
            if(LU.collider != null || LD.collider != null) {
                // 
                if(LU.collider != null) xMin = LU.point.x + radius + space;
                else xMin = LD.point.x + radius + space;
                
            } else {
                xMin = -displayWidth + radius;
            }
            // 右にコライダーがあるとき
            if(RU.collider != null || RD.collider != null) {
                if(RU.collider != null) xMax = RU.point.x - radius - space;
                else xMax = RD.point.x - radius - space;
                
            } else {
                xMax = displayWidth - radius;
            }
            
            Vector2 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x + diffDistance,  xMin,  xMax);
            
            transform.position = pos;
            

            // タップ位置を更新
            previousPosX = currentPosX;
        } 
    }

    // 上向きに移動
    public void Move()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName(c.gameObject.layer);

        // レイヤー名がBlock以外のときは何も行わない
        if(layerName != "Block") return;

        // blockの削除
        // Destroy(c.gameObject);

        // エネミーの削除
        // Destroy(gameObject);
    }
    void OnCollisionExit2D(Collision2D c) 
    {
        
        Move();
    }
}
