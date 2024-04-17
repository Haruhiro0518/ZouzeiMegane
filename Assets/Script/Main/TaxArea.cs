using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TaxAreaオブジェクトにアタッチするクラス
// Playerと衝突した場合、税率を税率変化率によって増減させる
public class TaxArea : MonoBehaviour
{
    [System.NonSerialized] public string de_or_increase;
    private float _changeTaxRate;
    // コンポーネント
    private ManageHPUI manageHPUI;
    private SpriteRenderer _sprite;
    // ScriptableObject
    [SerializeField] ValueData valueData;

    void Start()
    {
        manageHPUI = gameObject.GetComponent<ManageHPUI>();
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        

        // 税率変化は50% ~ 150%の間で50%区切り
        _changeTaxRate = (int)Random.Range(1, 4) * 50;

        if(de_or_increase == "increase") {
            _sprite.color = new Color32(225, 70, 0, 90);
            manageHPUI.ChangeText("増税\n<size=100>"+_changeTaxRate.ToString()+"</size>%");
        } else if(de_or_increase == "decrease") {
            _sprite.color = new Color32(0, 202, 255, 90);
            _changeTaxRate *= -1;
            manageHPUI.ChangeText("減税\n<size=100>"+_changeTaxRate.ToString()+"</size>%");
        }
    }

    
    void Update()
    {
        
    }

    
    void OnTriggerEnter2D(Collider2D c)
    {
        Player p;
        if(c.gameObject.tag == "Player") {
            p = c.gameObject.GetComponent<Player>();
            p.taxRate += _changeTaxRate / 100f;
            
            // taxRateが上限を超えないようにする。無敵状態であるかによって上限は変化する
            if(p.IsInvincible == false) {
                if(p.taxRate > p.taxRateMax) {
                    p.taxRate = p.taxRateMax;
                }
            } else {
                if(p.taxRate > p.taxRateMaxInv) {
                    p.taxRate = p.taxRateMaxInv;
                }
            }

            p.taxRate = (p.taxRate < 0) ? 0 : p.taxRate;

            // Itemの出現確率とHPをtaxRateに応じて変化させる
            valueData.ChangeItemHP_Percent(p.taxRate);
            p.PlayerSpeed = p.ComputePlayerSpeed();
            p.Move();
            
        }
    }

    
}
