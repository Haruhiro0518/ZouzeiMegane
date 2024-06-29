using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TaxAreaオブジェクトにアタッチするクラス
// Playerと衝突した場合、税率を税率変化率によって増減させる
public class TaxArea_increase : MonoBehaviour
{
    private float changeTaxRate;
    // コンポーネント
    private ManageHPUI manageHPUI;
    private Player player;
    private WaveGenerate waveGenerate;
    private TaxRateText taxRateText;
    
    // ScriptableObject
    [SerializeField] ValueData data;

    void Start()
    {
        waveGenerate = GameObject.Find("WaveGenerator").GetComponent<WaveGenerate>();
        player = GameObject.Find("Player").GetComponent<Player>();
        taxRateText = GameObject.Find("TaxRateText").GetComponent<TaxRateText>();
        manageHPUI = gameObject.GetComponent<ManageHPUI>();
        
        changeTaxRate = SelectChangeRate(player.taxRate);
        ChangeTaxAreaText();
    }

    bool cantIncrease = false;
    float SelectChangeRate(float p_rate)
    {
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
    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player") {

            if(player.IsInvincible == true && player.taxRate >= 1.5f) {
                waveGenerate.NextBlockWaveSetGlasses();
            }
            else {
                ChangeTaxRate();
            }
            
        }
    }

    void ChangeTaxRate()
    {
        // 税率を変える
        player.taxRate += changeTaxRate;
        
        // 変化後税率に関してパラメータ変更
        data.ChangeItemHPminmax(player.taxRate);
        data.ChangeBlockHPDistribution(player.taxRate);
        player.PlayerSpeed = player.SelectPlayerSpeed();
        player.Move();

        if(cantIncrease == true) {
            taxRateText.VibrateScaleUp();
        } 

    }
    
    [SerializeField] ManageImgUI manageImgUI;
    public void ChangeTaxAreaText()
    {
        if(player.IsInvincible == false) {
            manageHPUI.ChangeText("増税\n<size=100>"+(changeTaxRate*100f).ToString()+"</size>%");
            manageImgUI.image.enabled = false;
        } else {
            manageHPUI.ChangeText("増税\n\n\n");    // '\n'は位置合わせ
            manageImgUI.image.enabled = true;
        }
    }
}
