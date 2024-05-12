using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave-Randomプレハブで使うクラス
// block bar itemをそれぞれランダムにインスタンス化する
public class WaveRandom : MonoBehaviour
{
    [SerializeField] private GameObject Block;
    [SerializeField] private GameObject Bar;
    [SerializeField] private GameObject Item;
    // ScriptableObject
    [SerializeField] private ValueData data;

    [System.NonSerialized] public bool IsNoBlock = false;

    const float space = 1.12f;
    const float minx_block = -2.24f;
    const float minx_bar = -1.68f;

    void Start()
    {
        GenerateBlockOrItem();
        GenerateBar();
    }

    enum objects {
        Block,
        Item,
        empty
    }

    
    void GenerateBlockOrItem()
    {
        // ブロックは最大5個並べられる
        for(int i = 0; i < 5; i++)
        {
            // blockまたはitemを配置するかしないか
            int p = Random.Range(0,100);
            if(p < data.percentBlockAndItem) {

                p = Random.Range(0,100);
                if(p < data.percentBlock) 
                {
                    InstantiateBlock(i);
                }
                else 
                {
                    Vector3 itempos = new Vector3(minx_block + space*i, gameObject.transform.position.y,0f);
                    Instantiate(Item, itempos, Quaternion.identity, gameObject.transform);
                }
            } 
        }
    }

    void GenerateBar()
    {
        // barは最大3本。左端と右端は不要
        for(int i = 1; i < 4; i++)
        {
            int p = Random.Range(0, 100);
            if(p < data.percentBar)
            {
                Vector3 barpos = new Vector3(minx_bar + space*i, gameObject.transform.position.y,0f);
                Instantiate(Bar, barpos, Quaternion.identity, gameObject.transform);
            }
        }
    }


    void InstantiateBlock(int num)
    {
        if(IsNoBlock == true) {
            return;
        }
        Vector3 blockpos = new Vector3(minx_block + space*num, gameObject.transform.position.y,0f);
        Instantiate(Block, blockpos, Quaternion.identity, gameObject.transform);

        return;
    }

    
}
