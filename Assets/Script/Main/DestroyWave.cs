using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWave : MonoBehaviour
{
    // ブロックオブジェクトのテキスト削除関数を呼んでからWaveを消す
    public void destroyObject()
    {
    
        foreach ( Transform child in this.transform )
        {
            // blockのレイヤーなら
            /*
            if(child.gameObject.layer == 6) {
                Block block = child.GetComponent<Block>();
                
                block.destroyText();
            } 
            // Itemのレイヤーなら
            else if(child.gameObject.layer == 9) {
                Item item = child.GetComponent<Item>();
                
                item.destroyText();
            }*/
            
            ManageHPUI manageHPUI;
            if((manageHPUI = child.GetComponent<ManageHPUI>()) != null) {
                manageHPUI.DestroyText();
            }
            
        }
        
        Destroy(gameObject);
    }
}
