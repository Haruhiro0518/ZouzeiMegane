using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assets->Create->ScriptableObj->Create ValueData でインスタンス化して使用する
// Itemが生成されるときにインスタンスのmaxItemHPを参照する

[CreateAssetMenu(menuName = "ScriptableObj/Create ValueData")]
public class ValueData : ScriptableObject
{
    public int maxItemHP;

    public void ChangeItemHP_Percent(float rate) 
    {
        if(rate >= 5) {
            maxItemHP = 1;
        } 
        else if(rate >= 4 && rate < 5) {
            maxItemHP = 2;
        } 
        else if(rate >= 3 && rate < 4) {
            maxItemHP = 3;
        } 
        else if(rate >= 2 && rate < 3) {
            maxItemHP = 4;
        }
        else if(rate >= 1 && rate < 2) {
            maxItemHP = 5;
        }
        else if(rate >= 0.5 && rate < 1) {
            maxItemHP = 6;
        }
        else if(rate >= 0 && rate < 0.5) {
            maxItemHP = 7;
        }
    }
}
