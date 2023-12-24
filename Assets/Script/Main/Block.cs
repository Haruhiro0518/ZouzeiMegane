using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    // HP
    public int HP;
    // delay
    public float Delay = 0.5f;
    
    // 衝突中か
    public bool IsCol = false;
   
    // canvas
    private GameObject canvas;
    
    // 親の指定
    [SerializeField] private RectTransform _markerPanel;
    [SerializeField] private FollowTransform _markerPrefab;
    // スクリプトのインスタンス
    private FollowTransform marker;
    // HPテキストのgameObject
    private GameObject HPtext;

    void Awake()
    {
        canvas = GameObject.Find("Canvas");
        _markerPanel = canvas.GetComponent<RectTransform>();
    }

    // Use this for initialization
    void Start()
    {
        InitializeHP();
        // hpUIの初期化
        // スクリプトをインスタンス化
        marker = Instantiate(_markerPrefab, _markerPanel);
        marker.Initialize(gameObject.transform);
        // markerがアタッチされているgameObjectの取得
        HPtext = marker.gameObject;
        marker.ChangeText(HP);
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
    }

    public void InitializeHP() 
    {
        HP = Random.Range(1, 20);
    }
}
