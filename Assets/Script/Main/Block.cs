using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    private int _HP;
    private int originalHP;
    
    private float nextDamageDelay = 0.08f;
    private float DelayAfterDestroyed = 0.06f;

    public bool haveGlasses;

    private GameObject Player;
    private Player player;

    private ManageHPUI manageHPUI;

    private RectTransform _uiParentObjectTransform;
    [SerializeField] private GameObject _GlassesPrefab;
    private GameObject GlassesPrefab;
    private FollowTransform GlassesScript;
    
    private GameObject scoreGUI;
    private Score scoreScript;
    [SerializeField] private GameObject SEmoney;
    [SerializeField] private AudioSource BlockAudio;

    [SerializeField] private ValueData data;
    

    void Awake()
    {
        _uiParentObjectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }
    
    void Start()
    {
        Player = GameObject.Find("Player");
        player = Player.gameObject.GetComponent<Player>();
        manageHPUI = gameObject.GetComponent<ManageHPUI>();
        scoreGUI = GameObject.Find("ScoreGUI");
        scoreScript = scoreGUI.GetComponent<Score>();

        Set_HP_Glass();
        manageHPUI.ChangeText(_HP.ToString());

        if(haveGlasses == true)
        {
            GlassesPrefab = Instantiate(_GlassesPrefab, _uiParentObjectTransform);
            GlassesScript = GlassesPrefab.GetComponent<FollowTransform>();
            GlassesScript.Initialize(gameObject.transform);

            GlassesScript._worldOffset = new Vector3(0f, -0.25f, 0f);
            manageHPUI.ChangeWorldOffset(new Vector3(0f, 0.2f, 0f));
        }
    }

    private bool IsCol = false;
    void OnCollisionEnter2D(Collision2D c)
    {
        IsCol = true;
        StartCoroutine(collisionStay(c));
    }
    

    void OnCollisionExit2D(Collision2D c)
    {
        IsCol = false;
    }

    IEnumerator collisionStay(Collision2D c)
    {
        while(true) {
            // プレイヤーのHPが0以上かつ、衝突している間は
            // 両方のHPを減らす。nextDamageDelay秒ごとにHPが減る
            if((IsCol == true) && (_HP > 0)) {
                int valueScored = DecreaseHPReturnValueScored();
                scoreScript.AddScore(valueScored, player.taxRate);
                player.DecreaseHP(); 
                manageHPUI.ChangeText(_HP.ToString());


                int diff = originalHP - _HP;
                if(diff > 24) {
                    nextDamageDelay = 0.05f;
                } else if (diff > 12) {
                    nextDamageDelay = 0.07f;
                }

                if(_HP==0) {
                    yield return new WaitForSeconds(DelayAfterDestroyed);
                }
                else {
                    BlockAudio.Play();
                    yield return new WaitForSeconds(nextDamageDelay);
                }
            }
            else if(_HP <= 0) {
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
        int p = Random.Range(0,100);
        if(p < data.hpRange1to4) _HP = Random.Range(1,5);
        else if(p >= data.hpRange1to4 && p < data.hpRange5to9) _HP = Random.Range(5, 10);
        else if(p >= data.hpRange5to9 && p < data.hpRange10to19) _HP = Random.Range(10, 20);
        else if(p >= data.hpRange10to19 && p < data.hpRange20to29) _HP = Random.Range(20, 30);
        else _HP = Random.Range(30, 51);

        originalHP = _HP;

        if(player.TotalInvModeCallCount > 13) {
            haveGlasses = false;
            return;
        }

        if(_HP >= 25) {
            if(Random.Range(0,100) < data.GlassesPercent) {
                
                haveGlasses = true;    
            } else {
                haveGlasses = false;
            }
        }
    }

    public int DecreaseHPReturnValueScored()
    {
        int previousHP = _HP;

        if(player.IsInvincible == true)
        {
            _HP = 0;
            return previousHP;
        } 
        else 
        {
            _HP = _HP - 1;

            int difference;
            if(_HP==1) {    // 最後に壊すときだけスコアは5足す
                difference = 5;
            } else {
                difference = 1;
            }
            
            return difference;
        }
    }
}