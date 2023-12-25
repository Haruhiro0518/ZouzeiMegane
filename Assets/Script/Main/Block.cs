using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    // HP
    private int HP;
    // delay
    public float Delay = 0.5f;
    
    // 衝突中か
    private bool IsCol = false;

    // 増税めがねをもつ確率
    public int GlassesPercent = 50;
    // 増税めがねをもっているか
    public bool haveGlasses;

   
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

    }

    // コルーチンとする
    IEnumerator OnCollisionStay2D(Collision2D c)
    {
        // Playerとのみ衝突するため、c はPlayerの情報であることを前提とする

        IsCol = true;
        // Playerコンポーネント取得
        Player player = c.gameObject.GetComponent<Player>();

        
        while(true) {
            // 衝突中かつHPが0より大きいならダメージを受ける
            if((IsCol == true) && (HP > 0)) {
                // HPをPlayerのpower分減らす
                HP = HP - player.power;
                player.DHP();
                marker.ChangeText(HP); 
                scoreScript.AddScore(player.power, player.taxRate);
            }
            // HPがなくなったらdestroy
            else if(HP <= 0) {
                Destroy(gameObject);
                destroyText();
            } 
            // IsColがfalseならbreak;
            else {
                yield break;
            }

            // Delay秒まつ
            yield return new WaitForSeconds(Delay);
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        IsCol = false;
    }

    public void destroyText() 
    {
        Destroy(HPtext);
        if(GlassesImg != null) 
        Destroy(GlassesImg);
    }

    public void InitializeParam() 
    {
        // HP決定
        HP = Random.Range(1, 20);
        
        // めがねを持つか
        // 0~99
        if(Random.Range(0,100) < GlassesPercent) {
            haveGlasses = true;    // もつ
        } else {
            haveGlasses = false;   // もたない
        }
    }
}
