using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    // HP
    private int HP;
    private int originalHP;
    // nextDamageDelay
    public float nextDamageDelay = 0.09f;
    private float DelayAfterDestroyed = 0.06f;

    // 増税めがねをもつ確率
    public int GlassesPercent;
    // 増税めがねをもっているか
    public bool haveGlasses;

    private GameObject Player;
    private Player player;
    /*
    // 親の指定
    
    [SerializeField] private FollowTransform _uiObjectPrefab;
    [SerializeField] private FollowTransform _uiParentObjectTransform_glass;
    private FollowTransform uiObjectPrefab;
    private GameObject HPtext;
    */

    private ManageHPUI manageHPUI;
    private RectTransform _uiParentObjectTransform;
    [SerializeField] private GameObject _GlassesPrefab;
    private GameObject GlassesPrefab;
    private FollowTransform GlassesScript;
    

    // scoreUI
    private GameObject scoreGUI;
    // Scoreスクリプト
    private Score scoreScript;
    // SE object
    [SerializeField] private GameObject SEmoney;
    

    void Awake()
    {
        _uiParentObjectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        manageHPUI = gameObject.GetComponent<ManageHPUI>();
        Set_HP_Glass();
        
        if(haveGlasses == true)
        {
            GlassesPrefab = Instantiate(_GlassesPrefab, _uiParentObjectTransform);
            GlassesScript = GlassesPrefab.GetComponent<FollowTransform>();
            GlassesScript.Initialize(gameObject.transform);

            GlassesScript._worldOffset = new Vector3(0f, -0.25f, 0f);
            manageHPUI.ChangeWorldOffset(new Vector3(0f, 0.2f, 0f));
        }
        
        scoreGUI = GameObject.Find("ScoreGUI");
        scoreScript = scoreGUI.GetComponent<Score>();
        manageHPUI.ChangeText(HP.ToString());
        player = Player.gameObject.GetComponent<Player>();

    }

    void Update()
    {
        
    }

    private bool IsCol = false;
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
                manageHPUI.ChangeText(HP.ToString());


                int diff = originalHP - HP;
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
                if(haveGlasses == true) {
                    Destroy(GlassesPrefab);
                    if(player.HP >= 0) {
                        player.InvincibleMode();
                    }
                }
    
                Instantiate(SEmoney);
                manageHPUI.DestroyText();
                Destroy(gameObject);
                yield break;
            } 
            // IsColがfalseならbreak;
            else {
                yield break;
            }
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
        
        originalHP = HP;
    }

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


/*
/*
        uiObjectPrefab = Instantiate(_uiObjectPrefab, _uiParentObjectTransform);
        uiObjectPrefab.Initialize(gameObject.transform);
        // uiObjectPrefabがアタッチされているgameObjectの取得
        // HPtext = uiObjectPrefab.gameObject;
        // uiObjectPrefab.ChangeText(HP);
        // ChangeText(HP);
        manageHPUI.ChangeText(HP);
*/
