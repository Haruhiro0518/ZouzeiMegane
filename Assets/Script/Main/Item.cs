using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // HP
    public int HP;

    // canvas
    private GameObject canvas;
    
    // 親の指定
    [SerializeField] private RectTransform _markerPanel;
    [SerializeField] private FollowTransform _markerPrefab;
    // スクリプトのインスタンス
    private FollowTransform marker;
    // HPテキストのgameObject
    private GameObject HPtext;
    private Player player;


    void Awake()
    {
        canvas = GameObject.Find("Canvas");
        _markerPanel = canvas.GetComponent<RectTransform>();
    }
    
    void Start()
    {
        InitializeHP();
        player = GameObject.Find("Player").gameObject.GetComponent<Player>();
        // hpUIの初期化
        // スクリプトをインスタンス化
        marker = Instantiate(_markerPrefab, _markerPanel);
        marker.Initialize(gameObject.transform);
        // markerがアタッチされているgameObjectの取得
        HPtext = marker.gameObject;
        marker.ChangeText(HP);
    }

    
    void OnTriggerEnter2D(Collider2D c)
    {
        // playerと衝突することだけを考える
        
        // playerのHPにItemのHPを足す
        player.HP += HP;

        destroyText();
        Destroy(gameObject);
    }

    void InitializeHP()
    {
        HP = Random.Range(1, 6);
    }

    public void destroyText() 
    {
        Destroy(HPtext);
    }
}
