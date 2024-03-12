using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    // HP
    private int HP;
    private int HPorigin;
    // nextDamageDelay
    public float nextDamageDelay = 0.09f;
    private float DelayAfterDestroyed = 0.06f;
    
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
    // SE object
    [SerializeField] private GameObject SEmoney;
    

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
        Set_HP_Glass();

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
            // imageの位置をオフセットによりずらす
            marker2._worldOffset = new Vector3(0f, -0.25f, 0f);
            marker._worldOffset = new Vector3(0f, 0.2f, 0f);
        }

        
        scoreGUI = GameObject.Find("ScoreGUI");
        scoreScript = scoreGUI.GetComponent<Score>();

        player = Player.gameObject.GetComponent<Player>();

    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        IsCol = true;
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
            // プレイヤーのHPが0以上かつ、衝突している間は
            // 両者のHPを減らす。nextDamageDelay秒ごとにHPが減る
            if((IsCol == true) && (HP > 0)) {
                int valueScored = DecreaseBlockHP();
                scoreScript.AddScore(valueScored, player.taxRate);
                player.DecreaseHP();
                marker.ChangeText(HP); 

                int diff = HPorigin - HP;
                if(diff > 24) {
                    nextDamageDelay = 0.05f;
                } else if (diff > 12) {
                    nextDamageDelay = 0.07f;
                }

                if(HP==0) {
                    yield return new WaitForSeconds(DelayAfterDestroyed);
                }
                else {
                    yield return new WaitForSeconds(nextDamageDelay);
                }
            }
            else if(HP <= 0) {
                if(haveGlasses == true && !(player.HP < 0)) {
                    player.InvincibleMode();
                }
                Instantiate(SEmoney);
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

    private void Set_HP_Glass() 
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
        
        HPorigin = HP;
    }

    // blockのHPを減らし、減ったHPを返す
    public int DecreaseBlockHP()
    {
        int currentHP = HP;

        if(player.IsInvincible == true)
        {
            HP = 0;
            return currentHP;
        } else {
            HP = HP - player.power;
            if(HP < 0) {
                HP = 0;
            }
            int difference = currentHP - HP;
            return difference;
        }
    }
}
