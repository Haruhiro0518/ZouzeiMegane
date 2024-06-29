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
            // ManageHPUIを複数持つオブジェクトがあるため、ManageHPUIの配列を受け取る
            ManageHPUI[] manageHPUI = child.GetComponents<ManageHPUI>();
            foreach(ManageHPUI m in manageHPUI) {
                if(m != null) {
                    m.DestroyText();
                } 
            }
            // ManageImgUIを持つ場合、ImgもDestroyする
            ManageImgUI manageImgUI = child.GetComponent<ManageImgUI>();
            if(manageImgUI != null) {
                manageImgUI.DestroyImage();
            }
        }
        
        Destroy(gameObject);
    }

    // smokeエフェクトと同時にItemのHP, Animation変更
    public void ItemSmokeAndChangeParam()
    {
        foreach(Transform child in this.transform)
        {
            if(child.tag == "item")
            {
                Item item = child.GetComponent<Item>();
                item.Smoke();
                item.SetItemHP();
                item.SwitchItemDamageAnim();
            }
        }
    }

    // smokeエフェクトと同時に増税メガネを付与. blockの個数を返す
    public int BlockSmokeAndSetGlasses()
    {
        int blockcount = 0;
        foreach(Transform child in this.transform)
        {
            if(child.tag == "block")
            {
                Block block = child.GetComponent<Block>();
                block.Smoke();
                block.SetGlassesUnconditionally();
                blockcount++;
            }
        }
        return blockcount;
    }
    
}
