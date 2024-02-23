using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int layermask;
    
    private bool init_player_pos = true;

    // canvas
    private GameObject canvas;
    // 親の指定
    [SerializeField] private RectTransform _markerPanel;
    [SerializeField] private FollowTransform _markerPrefab;

    // スクリプトのインスタンス
    private FollowTransform marker;
    // HPテキストのgameObject
    [SerializeField] private GameObject HPtext;
    // Stageオブジェクト
    private GameObject Stage;
    // stagescript
    private StageScript stageScript;


    // マウスドラッグ処理
    private float previousPosX;
    private float currentPosX;

    // x座標をclampするときの最小値、最大値
    private float xMin;
    private float xMax;

    // player scale
    private float pScale;
    // 上のコライダーの半径と下の半径
    private float radiusUp = 0.1965f;
    //private float radiusDw = 0.14625f;
    // コライダーのx offset
    private float colxoffset;
    
    
    // 横から接触しないための隙間
    private float space = 0.05f;

    // display size
    private float displayWidth = 2.8f;

    // スピード係数
    public float speed;

    // playerの攻撃力
    public int power;
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

    // SE object
    [SerializeField] private GameObject SEbomb;
    
    [SerializeField, Header("総理アニメーター")]
    Animator PlayerAnimator;
    
    void Awake()
    {
        canvas = GameObject.Find("Canvas");
        _markerPanel = canvas.GetComponent<RectTransform>();
        Stage = GameObject.Find("Stage");
    }

    void Start()
    {
        // player scale
        pScale = gameObject.GetComponent<Transform>().localScale.x;
        // localに変換
        colxoffset = 0.1f*pScale;

        // hpUIの初期化
        // スクリプトをインスタンス化
        marker = Instantiate(_markerPrefab, _markerPanel);
        marker.Initialize(gameObject.transform);
        // markerがアタッチされているgameObjectの取得
        HPtext = marker.gameObject;
        marker.ChangeText(HP);

        // HPtextのスクリプト取得
        // marker = HPtext.GetComponent<FollowTransform>();
        // StageScript取得
        stageScript = Stage.GetComponent<StageScript>();
        
        // playerのlayer7とitemのlayer9を無視する
        layermask = (1 + 4) << 7;
        layermask = ~layermask;
        taxRate = 1.0f;

        
        Move();
    }

    void Update()
    {
        MoveDrag();
        if(init_player_pos == true) DebugPos();
        // HP 表示更新
        marker.ChangeText(HP);

        // gameOver
        if(HP < 0) {
            // play se
            Instantiate(SEbomb);
            destroyText();
            stageScript.IsGameover = true;
            // Debug.Log(stageScript.IsGameover);
            Destroy(gameObject);
        }
        

    }

    // InvicibleMode()をコルーチンにすると, blockが削除されたときにWaitFoSecondsがなくなるため、
    // 待つ処理はinv()で行う
    public void InvincibleMode()
    {
        // 呼ばれたときにextendInv変数をインクリメント
        extendInv++;
        // 無敵
        IsInvincible = true;
        //アニメーション切替
        PlayerAnimator.SetBool("PowerUP", IsInvincible);
        // speed up
        speed = 4.0f;
        // rate up
        taxRate = 2.0f;
        
        StartCoroutine(inv());
    }

    IEnumerator inv()
    {
        // InvTime 待つ
        yield return new WaitForSeconds(InvTime);
        
        // 無敵中に他の無敵を取らなかった場合、無敵終了
        if(extendInv == 1) {
            // 元に戻す
            speed = 3.0f;
            taxRate = 1.0f;
            IsInvincible = false;
            //アニメーション切替
            PlayerAnimator.SetBool("PowerUP", IsInvincible);
            // speed変化後のspeedをplayerに適用する
            Move();
        } 
        extendInv--;
    }

    // プレイヤー移動
    void MoveDrag() 
    {
        // ポーズ中は移動しない
        if(Time.timeScale == 0) return;

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
            // 左右にraycastを伸ばす. positionはコライダーの大きさによる微調整
            float yoffU = 0.8f * pScale;
            //float yoffD = 0.6f * pScale;

            RaycastHit2D LU = Physics2D.Raycast(transform.position+new Vector3(colxoffset,yoffU+radiusUp-space,0), Vector2.left, displayWidth*2, layermask);
            RaycastHit2D RU = Physics2D.Raycast(transform.position+new Vector3(colxoffset,yoffU+radiusUp-space,0), Vector2.right, displayWidth*2, layermask);
            RaycastHit2D LD = Physics2D.Raycast(transform.position+new Vector3(colxoffset,yoffU-radiusUp-space,0), Vector2.left, displayWidth*2, layermask);
            RaycastHit2D RD = Physics2D.Raycast(transform.position+new Vector3(colxoffset,yoffU-radiusUp-space,0), Vector2.right, displayWidth*2, layermask);

            // 左にコライダーがあるとき
            if(LU.collider != null || LD.collider != null) {
                // Downが優先
                if(LD.collider != null) xMin = LD.point.x + radiusUp - colxoffset + space;
                else xMin = LU.point.x + radiusUp - colxoffset + space;
                
            } else {
                xMin = -displayWidth + radiusUp - colxoffset;
            }
            // 右にコライダーがあるとき
            if(RU.collider != null || RD.collider != null) {
                if(RD.collider != null) xMax = RD.point.x - radiusUp - colxoffset - space;
                else xMax = RU.point.x - radiusUp - colxoffset - space;
                
            } else {
                xMax = displayWidth - radiusUp - colxoffset;
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
    
    void DebugPos()
    {
        Vector3 pos = new Vector3(0,0,0);
        gameObject.transform.position = pos;

        init_player_pos = false;
    }
}
