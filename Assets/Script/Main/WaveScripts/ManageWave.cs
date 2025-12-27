using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 全てのWaveプレハブにアタッチするクラス
[System.Serializable]
public struct WaveTypeSettings {
    public bool isFull;
    public bool isRandom;
    public bool isEmpty;
    public bool isTax;
}
public class ManageWave : MonoBehaviour
{
	enum objects {
		Block,
		Item,
		empty
	}
	// ScriptableObject
    [SerializeField] private ValueData data;
	[SerializeField] private ObjectReference objRef;
	private GameObject Block, Bar, Item;
	[SerializeField] private bool IsWaveFull, IsWaveRandom, IsWaveEmpty, IsWaveTax;
	// [SerializeField] private bool IsWaveRandom = false;
	// [SerializeField] private bool IsWaveEmpty = false;
	[SerializeField] private bool IsNoBlock = false;
	
	const float NumOfGrids = 5;
    const float space = 1.12f;
    const float minx_block = -2.24f;
    const float minx_bar = -1.68f;

	void Awake()
	{
		// TaxWaveなら参照は必要ない
		if(IsWaveTax)
		{
			return;
		}
		Block = objRef.Block;
		Bar = objRef.Bar;
		Item = objRef.Item;
	}
    void Start()
    {
		if(IsWaveEmpty)
		{
			return;
		}
		else if (IsWaveRandom)
		{
			GenerateBlockOrItem();
			GenerateBar();
		}
		else if (IsWaveFull)
		{
			for(int i = 0; i < NumOfGrids; i++)
			{
				InstantiateBlock(i);
			}
		}
    }

    
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

    void GenerateBlockOrItem()
    {
        // ブロックは最大5個並べられる
        for(int i = 0; i < NumOfGrids; i++)
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
                    Vector3 itempos = ComputeBlockPos(i);
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


    void InstantiateBlock(int i)
    {
        if(IsNoBlock == true) {
            return;
        }
        Vector3 blockpos = ComputeBlockPos(i);
        Instantiate(Block, blockpos, Quaternion.identity, gameObject.transform);
		Debug.Log("ins bl"+i);
        return;
    }

	private Vector3 ComputeBlockPos(int i)
	{
		return new Vector3(minx_block + space*i, gameObject.transform.position.y,0f);
	}
    
}
