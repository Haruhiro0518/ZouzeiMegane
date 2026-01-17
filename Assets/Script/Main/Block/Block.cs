using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Block : MonoBehaviour
{
    private int _HP;
    private int originalHP;
    
    private float nextDamageDelay;  // 0.08f -> 0.07f -> 0.05f
    private const float DelayAfterDestroyed = 0.06f;
    private float scoreAccumulator = 0f;

    [SerializeField] private bool haveGlasses;

    private Player player;
    private Score Score;

    private RectTransform _uiParentObjectTransform;
    [SerializeField] private GameObject _GlassesPrefab;
    private GameObject GlassesPrefab;
    private FollowTransform GlassesScript;
    
    [SerializeField] private ManageHPUI manageHPUI, manageSCOREUI;
    [SerializeField] private GameObject SEmoney;
    [SerializeField] private PlaySE playSE;
    [SerializeField] private BlockScoreFX blockScoreFX;
    [SerializeField] private GameObject SmokeFX;
    [SerializeField] private ValueData data;
    

    void Awake()
    {
        _uiParentObjectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Score = GameObject.Find("Score").GetComponent<Score>();

        Set_HP_Glass();
        manageHPUI.ChangeText(_HP.ToString());

        if(haveGlasses == true)
        {
            InstantiateGlasses();
			// メガネがある時は、小要素のMoneyを非表示
			// 0番目（一番上）の子オブジェクトを非アクティブにする
			transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
                int value = Decrease_HPReturnValueScored();
                float valueScored = (float)(value * player.taxRate);
                Score.AddScore(valueScored);
                scoreAccumulator += valueScored;
				if(!player.IsInvincible) player.AddHP(-1);
                manageHPUI.ChangeText(_HP.ToString());

                int diff = originalHP - _HP;
                if(diff <= 11) {
                    nextDamageDelay = 0.08f;    
                } else if (diff <= 23) {
                    nextDamageDelay = 0.07f;
                } else {
                    nextDamageDelay = 0.05f;
                }


                if(_HP > 0) {
                    playSE.Play();
                    blockScoreFX.HitBlockScore(valueScored);
                    yield return new WaitForSeconds(nextDamageDelay);
                }
                else if(_HP == 0) {
                    Instantiate(SEmoney);

                    if(player.IsInvincible == true) {
                        blockScoreFX.InvincibleDestory(scoreAccumulator);
                    } else {
                        blockScoreFX.HitBlockScoreLong(valueScored);
                        manageHPUI.DestroyText();
                    }

                    if(haveGlasses == true) {
                        Destroy(GlassesPrefab);
                        if(player.HP >= 0) {
                            player.InvincibleMode();
                        }
                    }
					// オブジェクトの削除
					foreach (Transform child in transform) {
						Destroy(child.gameObject); 
					}
        
                    // ヒットストップ
                    yield return new WaitForSeconds(DelayAfterDestroyed);
                    // 破壊(されているように見せる)
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    StartCoroutine(DelayedDestroy(blockScoreFX.AnimationLength));
                    
                }
            }
            // IsColがfalse（衝突していない）ならbreak;
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

        if(player.exceedMaxInvCount == true) {
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

    public int Decrease_HPReturnValueScored()
    {
        int previousHP = _HP;

        if(player.IsInvincible == true)
        {
            _HP = 0;
            return previousHP * Score.scorerate;
        } 
        else 
        {
            _HP = _HP - 1;

            int difference;
            if(_HP==0) {    // 最後に壊すときだけスコアは5足す
                difference = 5;
            } else {
                difference = 1;
            }
            
            return difference * Score.scorerate;
        }
    }

    public IEnumerator DelayedDestroy(float delaytime)
    {
        
        yield return new WaitForSeconds(delaytime);

        manageHPUI.DestroyText();
        manageSCOREUI.DestroyText();
        Destroy(gameObject);
    }

    private void InstantiateGlasses()
    {
        GlassesPrefab = Instantiate(_GlassesPrefab, _uiParentObjectTransform);
        GlassesScript = GlassesPrefab.GetComponent<FollowTransform>();
        GlassesScript.Initialize(gameObject.transform);

        GlassesScript._worldOffset = new Vector3(0f, -0.25f, 0f);
        manageHPUI.ChangeWorldOffset(new Vector3(0f, 0.2f, 0f));
    }

    public void SetGlassesUnconditionally()
    {
        if(haveGlasses == true) {
            return;
        } else {
            haveGlasses = true;
            InstantiateGlasses();
        }
    }
    public void Smoke()
    {
        Instantiate(SmokeFX, gameObject.transform.position, Quaternion.identity);
    }
}