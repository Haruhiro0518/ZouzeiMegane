using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 全てのWaveプレハブにアタッチするクラス

public class ManageWave : MonoBehaviour
{

    // 子オブジェクトのテキスト削除関数を呼んでからWaveを消す
    public void destroyObject()
    {
    
        foreach ( Transform child in this.transform )
        {   
            ManageHPUI manageHPUI;
            if((manageHPUI = child.GetComponent<ManageHPUI>()) != null) {
                manageHPUI.DestroyText();
            } 
        }
        
        Destroy(gameObject);
    }

    public void ItemSmokeAndChangeParam()
    {
        foreach(Transform child in this.transform)
        {
            if(child.tag == "item")
            {
                Item item = child.GetComponent<Item>();
                item.Smoke();
                item.SetItemHP();
                item.ItemDamageAnimOnOff();
            }
        }
    }
}
