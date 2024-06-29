using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TaxAreaオブジェクトにアタッチするクラス
// Playerと衝突した場合、税率を税率変化率によって増減させる
public class TaxArea_decrease : MonoBehaviour
{   

    private float changeTaxRate;
    // コンポーネント
    private ManageHPUI manageHPUI;
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
    
        changeTaxRate = SelectChangeRate(player.taxRate);
        ChangeTaxAreaText();
    }

    bool cantDecrease = false;
    float SelectChangeRate(float p_rate)
    {
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
    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player") {

            if(player.IsInvincible == false) {
                ChangeTaxRate();
            } 
            else {                          // 無敵かつ減税
                IgnoreDecreaseTaxRate();
            } 
            
        }
    }

    void ChangeTaxRate()
    {
        // 税率を変える
        player.taxRate += changeTaxRate;
        if(player.taxRate < 0f) {
            player.taxRate = 0f;
        }
        // 変化後税率に関してパラメータ変更
        data.ChangeItemHPminmax(player.taxRate);
        data.ChangeBlockHPDistribution(player.taxRate);
        player.PlayerSpeed = player.SelectPlayerSpeed();
        player.Move();

        if(cantDecrease == true) {
            taxRateText.VibrateScaleDown();
        }

        waveGenerate.AccelerateNextTaxArea(15);
        player.HP += 25;
        
    }
    // 減税を検討(ignore)する。検討すると、PlayerSpeedOffsetが増え、総理が加速する
    void IgnoreDecreaseTaxRate()
    {

        audioManager.Play_ignoreTaxArea();
        manageHPUI.ChangeText("<size=120>検討</size>");

        if(player.PlayerSpeedOffset < 1.0f) {
            player.PlayerSpeedOffset += 0.2f;
            SpeedBarUpdate();
        } else {
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
    
    // スピードバー表示更新. speedUptextの表示
    private SpeedBar speedBar;
    void SpeedBarUpdate()
    {
        speedBar = GameObject.Find("SpeedBar").GetComponent<SpeedBar>();
        speedBar.SetValue(player.PlayerSpeedOffset);
        speedBar.SpeedUpTextFX();
    }

    [SerializeField] ManageImgUI manageImgUI;
    public void ChangeTaxAreaText()
    {
        if(player.IsInvincible == false) {
            manageHPUI.ChangeText("減税\n<size=100>"+(changeTaxRate*100f).ToString()+"</size>%");
            manageImgUI.image.enabled = false;
        } else {
            manageHPUI.ChangeText("減税\n\n\n");
            manageImgUI.image.enabled = true;
        }
    }
}
