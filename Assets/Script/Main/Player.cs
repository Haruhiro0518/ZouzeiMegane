using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤー操作処理・HPテキスト変更・無敵状態の処理

public class Player : MonoBehaviour
{
    // 当たり判定調整に使うレイヤーマスク
    private int layermask;
    
    // マウスドラッグ処理
    private float previousPosX;
    private float currentPosX;

    // x座標をclampするときの最小値、最大値
    private float xMin;
    private float xMax;

    // 当たり判定に使う定数
    private const float ColliderRadius = 0.1965f;
    private const float PlayerScale = 0.195f;
    private const float colYoffset = 0.8f * PlayerScale;
    private const float colXoffset = 0.1f * PlayerScale;
    private float displayWidth = 2.8f;
    // 横から接触しないための隙間
    private float space = 0.05f;
    
    // playerのパラメータ
    [System.NonSerialized] public int HP = 50;
    [System.NonSerialized] public float PlayerSpeed = OriginalPlayerSpeed;
    [System.NonSerialized] public float PlayerSpeedOffset = 0f; // 減税を検討すると増える(TaxArea.cs)
    const float OriginalPlayerSpeed = 3.0f;
    [System.NonSerialized] 
    public float taxRate = 1.0f;
    [System.NonSerialized] public float taxRateMax = 1.5f;
    [System.NonSerialized] public float taxRateMaxInv = 2.0f;
    
    // 無敵状態の制御に使う変数
    // [System.NonSerialized] 
    public bool IsInvincible = false;
    private float InvTime = 6.85f;
    private int InvModeCallCount = 0;
    [System.NonSerialized] public int TotalInvModeCallCount = 0;
    // コンポーネント
    private ManageHPUI manageHPUI;
    private WaveGenerate waveGenerate;
    private FollowPlayer followPlayer;
    [SerializeField] private GameObject SEbomb;
    [SerializeField] private GameObject FXsmoke;
    [SerializeField] Animator PlayerAnimator;
    [SerializeField] private InvincibleBGM invincibleBGM;

    [SerializeField] ValueData data;
    void Awake()
    {
        waveGenerate = GameObject.Find("WaveGenerator").GetComponent<WaveGenerate>();
        followPlayer = GameObject.Find("Main Camera").GetComponent<FollowPlayer>();
    }

    void Start()
    {
        manageHPUI = gameObject.GetComponent<ManageHPUI>();
        
        // playerのlayer7とitemのlayer9, taxareaのlayer10, insidescreenのlayer11を無視する
        layermask = (1 + 4 + 8 + 16) << 7;
        layermask = ~layermask;

        Move();
    }

    void Update()
    {
        MoveDrag();
        if(init_player_pos == true) DebugPos();

        manageHPUI.ChangeText(HP.ToString());

        if(HP < 0) {
            OnGameOver();
        }
        if(waveGenerate.IsGameClear == true) {
            PlayerSpeed = 0f;
        }   
    }

    // InvicibleMode()をコルーチンにすると, blockが削除されたときにWaitFoSecondsがなくなるため、
    // 待つ処理はinv()で行う
    public void InvincibleMode()
    {
        IsInvincible = true;
        InvModeCallCount++;
        TotalInvModeCallCount++;

        if(InvModeCallCount == 1) {
            PlayerAnimator.SetBool("PowerUP", IsInvincible);
            invincibleBGM.Play();
            taxRate += 0.5f;
            data.ChangeBlockHPDistribution(taxRate);
            ChangeItemParameter(IsInvincible);
            PlayerSpeed = SelectPlayerSpeed();
            Move();
        }
        
        StartCoroutine(inv());
    }

    IEnumerator inv()
    {
        // InvTime 待つ
        yield return new WaitForSeconds(InvTime);
        
        // 無敵中に他の無敵を取らなかった場合、無敵終了
        if(InvModeCallCount == 1) {
            OnInvincibleExit();
        } 
        InvModeCallCount--;
    }

    void OnInvincibleExit()
    {
        // 元に戻す
        IsInvincible = false;
        PlayerAnimator.SetBool("PowerUP", IsInvincible);
        invincibleBGM.Stop();
        taxRate -= 0.5f;
        taxRate = (taxRate < 0) ? 0 : taxRate;
        TotalInvModeCallCount = 0;

        data.ChangeBlockHPDistribution(taxRate);
        ChangeItemParameter(IsInvincible);
        PlayerSpeed = SelectPlayerSpeed();
        waveGenerate.AccelerateNextTaxArea(40);
        Move();
    }

    [System.NonSerialized] public bool IsCollisionStay = false;
    private int playerCollisionCount = 0;
    void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.tag == "block") {
            playerCollisionCount++;
            if(playerCollisionCount == 1) {
                IsCollisionStay = true;
                StartCoroutine(followPlayer.CameraMoveupOrDown());
            }
        }
        
    }

    void OnCollisionExit2D(Collision2D c) 
    {
        Move();
        if(c.gameObject.tag == "block") {
            StartCoroutine(WaitCollisionExit());
        }
    }
    // CameraMoveupOrDown()を想定通りに呼び出すための処理
    // (詳細：Playerが素早く現在のブロックから隣のブロックに移動したとき、衝突の個数は1->0->1となってしまい
    // CollisionEnter2Dでのコルーチンが2回呼ばれてしまう。衝突の個数を1->wait->2と変化させると1度しか呼ばれない)
    IEnumerator WaitCollisionExit() {
        yield return new WaitForSeconds(0.01f);

        playerCollisionCount--;
        if(playerCollisionCount == 0) {
            IsCollisionStay = false;
            StartCoroutine(followPlayer.CameraMoveupOrDown());
        }
    }

    
    private void ChangeItemParameter(bool IsInv)
    {
        if(IsInv == true)
        {
            data.ItemHPCoefficient = -1;
            data.ChangeItemHPminmax(taxRate);
        } 
        else 
        {
            data.ItemHPCoefficient = 1;
            data.ChangeItemHPminmax(taxRate);
        }

        waveGenerate.AllItemSmokeAndChangeParam();
    }

    private void OnGameOver()
    {
        Instantiate(SEbomb);
        Instantiate(FXsmoke, gameObject.transform.position, Quaternion.identity);
        manageHPUI.DestroyText();
        waveGenerate.IsGameOver = true;
        
        Destroy(gameObject);
    }

    public float SelectPlayerSpeed() 
    {
        float selectedSpeed;
        if(taxRate == 0) {
            selectedSpeed = 2.7f;
        } else if(taxRate == 0.5) {
            selectedSpeed = 2.8f;
        } else if(taxRate == 1) {
            selectedSpeed = OriginalPlayerSpeed;
        } else if(taxRate == 1.5) {
            selectedSpeed = 4.4f;
        } else {
            selectedSpeed = 5.0f;
        }
        return (selectedSpeed + PlayerSpeedOffset);
    }

    // 上向きに移動
    public void Move()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * PlayerSpeed;
    }

    // ドラッグ処理
    void MoveDrag() 
    {
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

    public void DecreaseHP()
    {
        if(IsInvincible == true) return;
        HP -= 1;
    }

    // unityroomで実行するときにplayerの位置が原点からずれてしまうため
    // 最初にプレイヤーを原点に配置する。OnEnableとかStartで修正できないか試す
    private bool init_player_pos = true;
    void DebugPos()
    {
        Vector3 pos = new Vector3(0,0,0);
        gameObject.transform.position = pos;

        init_player_pos = false;
    }
}
