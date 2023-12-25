using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int layermask;

    // スクリプトのインスタンス
    private FollowTransform marker;
    // HPテキストのgameObject
    [SerializeField] private GameObject HPtext;
    // Stageオブジェクト
    [SerializeField] private GameObject Stage;
    // stagescript
    private StageScript stageScript;


    // マウスドラッグ処理
    private float previousPosX;
    private float currentPosX;

    // x座標をclampするときの最小値、最大値
    private float xMin;
    private float xMax;

    // player size 2.6/2 定数
    private float radius = 0.13f;
    
    // 横から接触しないための隙間
    private float space = 0.05f;

    // display size
    private float displayWidth = 2.8f;

    // スピード係数
    public float speed = 2.5f;

    // playerの攻撃力
    public int power = 1;
    // playerのHP
    public int HP;
    // 税率
    public float taxRate = 1.0f;
    // 無敵状態であるか
    public bool IsInvincible = false;
    // 無敵秒数
    private float InvTime = 5.0f;
    // 無敵時間延長
    private int extendInv;


    void Start()
    {
        HP = 100;
        // HPtextのスクリプト取得
        marker = HPtext.GetComponent<FollowTransform>();
        // StageScript取得
        stageScript = Stage.GetComponent<StageScript>();

        // プレイヤーの半径
        // radius = GetComponent<Transform>().transform.localScale.x / 2;
        
        // playerのlayer7とlayer9を無視する
        layermask = (1 + 4) << 7;
        layermask = ~layermask;

        Move();
    }

    void Update()
    {
        MoveDrag();
        // HP 表示更新
        marker.ChangeText(HP);

        // gameOver
        if(HP <= 0) {
            destroyText();
            stageScript.IsGameover = true;
            Destroy(gameObject);
        }
        Debug.Log(extendInv);
    }

    // InvicibleMode()をコルーチンにすると, blockが削除されたときにWaitFoSecondsがなくなるため、
    // 待つ処理はinv()で行う
    public void InvincibleMode()
    {
        // 呼ばれたときにextendInv変数をインクリメント
        extendInv++;
        // 無敵
        IsInvincible = true;
        // speed up
        speed = 4f;
        StartCoroutine(inv());
    }

    IEnumerator inv()
    {
        // InvTime 待つ
        yield return new WaitForSeconds(InvTime);
        
        // 無敵中に他の無敵を取らなかった場合、無敵終了
        if(extendInv == 1) {
            // 元に戻す
            speed = 2.5f;
            IsInvincible = false;
        } 
        extendInv--;
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
            RaycastHit2D LD = Physics2D.Raycast(transform.position+new Vector3(0,-radius-space,0), Vector2.left, displayWidth*2, layermask);
            RaycastHit2D RD = Physics2D.Raycast(transform.position+new Vector3(0,-radius-space,0), Vector2.right, displayWidth*2, layermask);

            // 左にコライダーがあるとき
            if(LU.collider != null || LD.collider != null) {
                // Downが優先
                if(LD.collider != null) xMin = LD.point.x + radius + space;
                else xMin = LU.point.x + radius + space;
                
            } else {
                xMin = -displayWidth + radius;
            }
            // 右にコライダーがあるとき
            if(RU.collider != null || RD.collider != null) {
                if(RD.collider != null) xMax = RD.point.x - radius - space;
                else xMax = RU.point.x - radius - space;
                
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

    
    void OnCollisionExit2D(Collision2D c) 
    {
        Move();
    }

    public void DHP()
    {
        // 無敵ならHPは減らない
        if(IsInvincible == true) return;
        HP -= 1;
    }

    public void destroyText() 
    {
        Destroy(HPtext);
    }
    
}
