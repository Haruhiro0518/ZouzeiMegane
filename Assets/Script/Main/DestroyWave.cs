using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWave : MonoBehaviour
{
    public void destroyObject()
    {
        // ブロックオブジェクトのテキスト削除関数を呼んでからWaveを消す

        // 子オブジェクトをループして取得
        foreach ( Transform child in this.transform )
        {
            // blockのレイヤーなら
            if(child.gameObject.layer == 6) {
                Block block = child.GetComponent<Block>();
                
                block.destroyText();
            }
        }
        
        Destroy(gameObject);
    }
}
