using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// waveをランダムに生成するメソッド
public class GenerateWave : MonoBehaviour
{
    // prefabs
    [SerializeField] private GameObject Block;
    [SerializeField] private GameObject Bar;
    [SerializeField] private GameObject Item;

    // objectを配置する間隔
    const float spc = 1.12f;
    // 左端 1は,block/itemを配置するx座標の基準. 2は,barを配置するx座標の基準
    const float minx1 = -2.24f;
    const float minx2 = -1.68f;
    // 右端
    const float maxx1 = 2.24f;
    const float maxx2 = 1.68f;

    // block/itemを配置する確率
    private float percentBlock = 30;
    // barを配置する確率
    private float percentBar = 30;

    void Start()
    {
        GenerateBlock(minx1, maxx1);
        GenerateBar(minx2, maxx2);
    }


    void Update()
    {
        
    }

    void GenerateBlock(float min, float max)
    {
        for(float i = min; i <= max; i+=spc)
        {
            // Debug.Log(i);
            // block/itemを配置するか
            int r = Random.Range(0,100);
            if(r < percentBlock) {

                // どちらを配置するか
                int r2 = Random.Range(0,2);
                // block instantiate
                if(r2 == 0) 
                {
                    Instantiate(Block,      // prefab 
                        new Vector3(i, gameObject.transform.position.y,0f),    // position
                        Quaternion.identity,    // rotation
                        gameObject.transform  // parent
                    );
                } 
                // item
                else 
                {
                    Instantiate(Item, new Vector3(i, gameObject.transform.position.y,0f), 
                        Quaternion.identity, gameObject.transform);
                }
            }
        }
    }

    void GenerateBar(float min, float max)
    {
        
        for(float i = min; i <= max; i+=spc)
        {
            int r = Random.Range(0, 100);
            if(r < percentBar)
            {
                Instantiate(Bar, new Vector3(i, gameObject.transform.position.y,0f), 
                    Quaternion.identity, gameObject.transform);
            }
        }
        
    }
}
