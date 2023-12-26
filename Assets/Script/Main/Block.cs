using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    // HP
    private int HP;
    // delay
    private float Delay = 0.08f;
    
    // 衝突中か
    private bool IsCol = false;
    

    // 増税めがねをもつ確率
    public int GlassesPercent;
    // 増税めがねをもっているか
    public bool haveGlasses;
    // Player情報
   private GameObject Player;
   private Player player;
    // canvas
    private GameObject canvas;
    
    // 親の指定
    [SerializeField] private RectTransform _markerPanel;
    [SerializeField] private FollowTransform _markerPrefab;
    [SerializeField] private FollowTransform _markerPrefab_glasses;
    // スクリプトのインスタンス
    private FollowTransform marker;
    // HPテキストのgameObject
    private GameObject HPtext;

    // glasses UI
    private FollowTransform marker2;
    private GameObject GlassesImg;

    // scoreUI
    private GameObject scoreGUI;
    // Scoreスクリプト
    private Score scoreScript;
    

    void Awake()
    {
        canvas = GameObject.Find("Canvas");
        _markerPanel = canvas.GetComponent<RectTransform>();

    }

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        // HP や 眼鏡をもつか　などのParam 初期化
        InitializeParam();

        // hpUIの初期化
        // スクリプトをインスタンス化
        marker = Instantiate(_markerPrefab, _markerPanel);
        marker.Initialize(gameObject.transform);
        // markerがアタッチされているgameObjectの取得
        HPtext = marker.gameObject;
        marker.ChangeText(HP);

        // glassesImageの初期化
        if(haveGlasses == true)
        {
            marker2 = Instantiate(_markerPrefab_glasses, _markerPanel);
            marker2.Initialize(gameObject.transform);
            GlassesImg = marker2.gameObject;
            // 位置ずらす
            marker2._worldOffset = new Vector3(0f, -0.25f, 0f);
            marker._worldOffset = new Vector3(0f, 0.2f, 0f);
        }

        // scoreGUIを取得
        scoreGUI = GameObject.Find("ScoreGUI");
        // Scoreスクリプト取得
        scoreScript = scoreGUI.GetComponent<Score>();

        // Playerコンポーネント取得
        player = Player.gameObject.GetComponent<Player>();

    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        IsCol = true;
        // 衝突中の処理
        StartCoroutine(collisionStay(c));
    }
    
    void OnCollisionStay2D(Collision2D c)
    {
        
    }

    void OnCollisionExit2D(Collision2D c)
    {
        IsCol = false;
    }

    IEnumerator collisionStay(Collision2D c)
    {

        

        while(true) {
            
            // 衝突中かつHPが0より大きいならダメージを受ける
            if((IsCol == true) && (HP > 0)) {
                // HPを減らす処理
                
                scoreScript.AddScore(DHP(player), player.taxRate);
                player.DHP();
                marker.ChangeText(HP); 
                // Delay秒まつ 
                // HP=0ならDelayなし
                if(HP==0) yield return new WaitForSeconds(0.05f);
                else  yield return new WaitForSeconds(Delay);
            }
            // HPがなくなったらdestroy
            else if(HP <= 0) {
                // このblockが増税めがねを持っている場合、playerを無敵にする
                if(haveGlasses == true && !(player.HP < 0)) player.InvincibleMode();

                Destroy(gameObject);
                destroyText();
                yield break;
            } 
            // IsColがfalseならbreak;
            else {
                yield break;
            }
        }
    }

    public void destroyText() 
    {
        Destroy(HPtext);
        if(GlassesImg != null) {
            Destroy(GlassesImg);
        }
        
    }

    public void InitializeParam() 
    {
        // HP決定 割合で決める
        // 1~4:20%, 5~20:65%, 21~35:10%, 36~50:5%
        int percent = Random.Range(0,100);
        if(percent < 20) HP = Random.Range(1,5);
        else if(percent >= 20 && percent < 85) HP = Random.Range(5, 21);
        else if(percent >= 85 && percent < 95) HP = Random.Range(21, 36);
        else HP = Random.Range(36, 51);
        
        // めがねを持つか 
        // 0~99
        // めがねを持つのはblockのHPが25以上のとき
        if(HP >= 25) {
            if(Random.Range(0,100) < GlassesPercent) {
                haveGlasses = true;    // もつ
            } else {
                haveGlasses = false;   // もたない
            }
        }
        
    }

    // blockのHPを減らす関数
    public int DHP(Player p)
    {
        // playerが無敵なら一回で壊れる
        if(p.IsInvincible == true){
            int temp = HP;
            HP = 0;
            return temp;
        } else {
            // HPをPlayerのpower分減らす
            HP = HP - p.power;
            return p.power;
        }
    }
}
