using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤー操作処理・HPテキスト変更・無敵状態の処理

public class Player : MonoBehaviour
{
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
    [SerializeField] private DragPlayer dragPlayer;

    [SerializeField] ValueData data;
    void Awake()
    {
        waveGenerate = GameObject.Find("WaveGenerator").GetComponent<WaveGenerate>();
        followPlayer = GameObject.Find("Main Camera").GetComponent<FollowPlayer>();
    }

    void Start()
    {
        manageHPUI = gameObject.GetComponent<ManageHPUI>();
        
        Move();
    }

    void Update()
    {
        manageHPUI.ChangeText(HP.ToString());

        if(HP < 0) {
            OnGameOver();
        }
        if(waveGenerate.IsGameClear == true) {
            PlayerSpeed = 0f;
        } 
        dragPlayer.Drag();
    }

    // 上向きに移動
    public void Move()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * PlayerSpeed;
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
            Debug.Log("exit");
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

    public void DecreaseHP()
    {
        if(IsInvincible == true) return;
        HP -= 1;
    }

}
