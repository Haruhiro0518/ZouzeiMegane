using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // HP
    public int HP = 50;
    // delay
    public float Delay = 0.5f;
    
    // 衝突中か
    public bool IsCol = false;

    // コルーチンとする
    IEnumerator OnCollisionStay2D(Collision2D c)
    {
        // Playerとのみ衝突するため、c はPlayerの情報であることを前提とする

        IsCol = true;
        // Playerコンポーネント取得
        Player player = c.gameObject.GetComponent<Player>();

        
        while(true) {
            // 衝突中かつHPが0より大きいならダメージを受ける
            if(IsCol == true && HP > 0) {
                // HPをPlayerのpower分減らす
                HP = HP - player.power;
            }
            // HPがなくなったらdestroy
            else if(HP <= 0) {
                Destroy(gameObject);
            } 
            // IsColがfalseならbreak;
            else {
                yield break;
            }

            // Delay秒まつ
            yield return new WaitForSeconds(Delay);
        }
    }

    void OnCollisionExit2D(Collision2D c)
    {
        IsCol = false;
    }
}
