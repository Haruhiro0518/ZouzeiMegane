using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TaxAreaオブジェクトにアタッチするクラス
// Playerと衝突した場合、税率を税率変化率によって増減させる
public class TaxArea : MonoBehaviour
{
    [System.NonSerialized] public string de_or_increase;
    private float changeTaxRate;
    // コンポーネント
    private ManageHPUI manageHPUI;
    private SpriteRenderer sprite;
    private Player player;
    private WaveGenerate waveGenerate;
    private TaxRateText taxRateText;
    private AudioManager audioManager;
    
    // ScriptableObject
    [SerializeField] ValueData data;

    void Start()
    {
        waveGenerate = GameObject.Find("WaveGenerator").GetComponent<WaveGenerate>();
        player = GameObject.Find("Player").GetComponent<Player>();
        taxRateText = GameObject.Find("TaxRateText").GetComponent<TaxRateText>();
        audioManager = GameObject.Find("MainManager").GetComponent<AudioManager>();
        manageHPUI = gameObject.GetComponent<ManageHPUI>();
        sprite = gameObject.GetComponent<SpriteRenderer>();

        if(de_or_increase == "increase") {
            sprite.color = new Color32(225, 70, 0, 140);
            changeTaxRate = SelectChangeRate(player.taxRate);
            manageHPUI.ChangeText("増税\n<size=100>"+(changeTaxRate*100f).ToString()+"</size>%");
        } else if(de_or_increase == "decrease") {
            sprite.color = new Color32(0, 202, 255, 140);
            changeTaxRate = SelectChangeRate(player.taxRate);
            manageHPUI.ChangeText("減税\n<size=100>"+(changeTaxRate*100f).ToString()+"</size>%");
        }
    }

    bool cantIncrease, cantDecrease = false;
    float SelectChangeRate(float p_rate)
    {
        if(de_or_increase == "increase") {
            if(p_rate == 0) {
                return 1.5f;
            } else if(p_rate == 0.5) {
                return 1.0f;
            } else if(p_rate == 1.0) {
                return 0.5f;
            } else {
                cantIncrease = true;
                return 0f;
            }
        } 
        else {
            if(p_rate == 0) {
                cantDecrease = true;
                return 0f;
            } else if(p_rate == 0.5) {
                return -0.5f;
            } else if(p_rate == 1.0) {
                return -0.5f; 
            } else {
                return -1.0f;
            }
        }
        
    }
    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player") {
            if(player.IsInvincible==true && changeTaxRate < 0) {
                IgnoreDecreaseTaxRate();
                return;
            } else {
                player.taxRate += changeTaxRate;
                if(player.taxRate < 0f) {
                    player.taxRate = 0f;
                }
            }
            
            OnTaxRateChanged(changeTaxRate);
        }
    }

    void OnTaxRateChanged(float changetaxrate)
    {
        data.ChangeItemHPminmax(player.taxRate);
        data.ChangeBlockHPDistribution(player.taxRate);
        player.PlayerSpeed = player.SelectPlayerSpeed();
        player.Move();

        if(cantIncrease == true) {
            taxRateText.VibrateScaleUp();
        } else if(cantDecrease == true) {
            taxRateText.VibrateScaleDown();
        }

        if(changeTaxRate <= 0 && de_or_increase=="decrease") {
            waveGenerate.AccelerateNextTaxArea(15);
            player.HP += 25;
        }

    }
    // 減税を検討する。検討すると、PlayerSpeedOffsetが増え、総理が加速する
    void IgnoreDecreaseTaxRate()
    {

        audioManager.Play_ignoreTaxArea();
        manageHPUI.ChangeText("<size=120>検討</size>");

        player.PlayerSpeedOffset += 0.2f;
        if(player.PlayerSpeedOffset > 1.0f) {
            player.PlayerSpeedOffset = 1.0f;
        }
        player.PlayerSpeed = player.SelectPlayerSpeed();
        player.Move();

        StartCoroutine(AnimateTaxArea());
    }


    private float whichside, move_x, move_y, x,y;
    // PlayerがTaxAreaを突き飛ばすアニメーション
    IEnumerator AnimateTaxArea()
    {
        x = y = 0;
        if(gameObject.transform.position.x > 0f) { // 右側
            whichside = 1f;
        } else {                                   // 左側
            whichside = -1f;
        }

        Vector3 offset = new Vector3(0f,0f,0f);

        while(true) 
        {
            // -3まで下に落ちたら終了
            if(move_y < -3f) {
                break;
            }
            // それぞれの増加率は試して上手くいった数値
            x += 0.005f;
            y += 0.05f;
            move_x = (0.3f - (1f-Mathf.Pow(1f-x, 3f))) * whichside;    // 0.3 - (1-(1-x)^3)
            move_y = 0.4f -(Mathf.Pow(y, 3f));     // 0.4 - (y*y*y)
            gameObject.transform.position += new Vector3(move_x, move_y, 0f);

            gameObject.transform.eulerAngles = new Vector3(0f, 0f, move_x*200f);

            yield return new WaitForSeconds(0.02f);
        }
        yield break;
    }
    
}
