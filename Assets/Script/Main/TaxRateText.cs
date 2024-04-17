using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// PlayerのtaxRateを百分率に直して表示するクラス
public class TaxRateText : MonoBehaviour
{
    private TMPro.TMP_Text taxRateText;
    private float taxrate;
    private Player player;

    void Start()
    {
        taxRateText = gameObject.GetComponent<TMPro.TMP_Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    
    void Update()
    {
        // Debug.Log(player.taxRate * 100f);
        taxrate = (player.taxRate * 100f);
        taxRateText.SetText("税率 <size=50>"+taxrate+"</size>%");
        
    }


}
