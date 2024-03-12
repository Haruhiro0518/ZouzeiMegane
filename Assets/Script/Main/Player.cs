using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int layermask;
    
    private bool init_player_pos = true;

    private GameObject canvas;

    // 親の指定
    [SerializeField] private RectTransform _markerPanel;
    [SerializeField] private FollowTransform _markerPrefab;
    private FollowTransform marker;

    [SerializeField] private GameObject HPtext;

    private GameObject WaveGenerator;
    
    private WaveGenerate waveGenerate;
    private FollowPlayer followPlayer;

    // マウスドラッグ処理
    private float previousPosX;
    private float currentPosX;

    // x座標をclampするときの最小値、最大値
    private float xMin;
    private float xMax;

    private const float ColliderRadius = 0.1965f;
    private const float PlayerScale = 0.195f;
    private const float colYoffset = 0.8f * PlayerScale;
    private const float colXoffset = 0.1f * PlayerScale;
    
    // 横から接触しないための隙間
    private float space = 0.05f;

    private float displayWidth = 2.8f;

    private float PlayerSpeed = 3.0f;
    // playerの攻撃力
    public int power;

    public int HP;
    // 税率
    public float taxRate = 1.0f;
    // 無敵
    public bool IsInvincible = false;
    private float InvTime = 5.0f;
    private int extendInv;

    [SerializeField] private GameObject SEbomb;
    
    [SerializeField, Header("総理アニメーター")]
    Animator PlayerAnimator;
    
    void Awake()
    {
        canvas = GameObject.Find("Canvas");
        _markerPanel = canvas.GetComponent<RectTransform>();
        WaveGenerator = GameObject.Find("WaveGenerator");
        followPlayer = GameObject.Find("Main Camera").GetComponent<FollowPlayer>();
    }

    void Start()
    {
        marker = Instantiate(_markerPrefab, _markerPanel);
        marker.Initialize(gameObject.transform);
        // markerがアタッチされているgameObjectの取得
        HPtext = marker.gameObject;
        marker.ChangeText(HP);

        // HPtextのスクリプト取得
        // marker = HPtext.GetComponent<FollowTransform>();
        
        waveGenerate = WaveGenerator.GetComponent<WaveGenerate>();
        
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
            waveGenerate.IsGameover = true;
            // Debug.Log(waveGenerate.IsGameover);
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
        // PlayerSpeed up
        PlayerSpeed = 4.0f;
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
            PlayerSpeed = 3.0f;
            taxRate = 1.0f;
            IsInvincible = false;
            //アニメーション切替
            PlayerAnimator.SetBool("PowerUP", IsInvincible);
            // PlayerSpeed変化後のPlayerSpeedをplayerに適用する
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

            UpdateXminXmax();
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
        GetComponent<Rigidbody2D>().velocity = transform.up * PlayerSpeed;
    }

    // プレイヤーの球コライダーの上部と下部から、水平方向にレイキャストを伸ばす
    // レイキャストがぶつかったブロックの端の座標をclampするx座標のxmin / xmaxとする
    void UpdateXminXmax()
    {

        RaycastHit2D topL = Physics2D.Raycast(transform.position+new Vector3(colXoffset,colYoffset+ColliderRadius-space*2,0), Vector2.left, displayWidth*2, layermask);
        RaycastHit2D topR = Physics2D.Raycast(transform.position+new Vector3(colXoffset,colYoffset+ColliderRadius-space*2,0), Vector2.right, displayWidth*2, layermask);
        RaycastHit2D botL = Physics2D.Raycast(transform.position+new Vector3(colXoffset,colYoffset-ColliderRadius-space,0), Vector2.left, displayWidth*2, layermask);
        RaycastHit2D botR = Physics2D.Raycast(transform.position+new Vector3(colXoffset,colYoffset-ColliderRadius-space,0), Vector2.right, displayWidth*2, layermask);

        
        if(topL.collider != null || botL.collider != null) {
            
            if(botL.collider != null) xMin = botL.point.x + ColliderRadius - colXoffset + space;
            else xMin = topL.point.x + ColliderRadius - colXoffset + space;
        } else {
            xMin = -displayWidth + ColliderRadius - colXoffset;
        }
        

        if(topR.collider != null || botR.collider != null) {
            if(botR.collider != null) xMax = botR.point.x - ColliderRadius - colXoffset - space;
            else xMax = topR.point.x - ColliderRadius - colXoffset - space; 
        } else {
            xMax = displayWidth - ColliderRadius - colXoffset;
        }

    }


    public bool IsCollisionStay = false;
    void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.tag == "block") {
            IsCollisionStay = true;
            StartCoroutine(followPlayer.CameraMoveupOrDown());
        }
        
    }

    void OnCollisionExit2D(Collision2D c) 
    {
        Move();
        if(c.gameObject.tag == "block") {
            IsCollisionStay = false;
            StartCoroutine(followPlayer.CameraMoveupOrDown());
        }
    }

    public void DecreaseHP()
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
