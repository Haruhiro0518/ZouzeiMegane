using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// 全てのWaveプレハブにアタッチするクラス
public class ManageWave : MonoBehaviour
{
	// ScriptableObject
    [SerializeField] private ValueData data;
	[SerializeField] private ObjectReference objRef;
	[SerializeField] private ItemLibrary itemLib;

	public const float NumOfGrids = 5;
    const float space = 1.12f;
    const float minx_block = -2.24f;
    const float minx_bar = -1.68f;
	const float bar_scaleX = 0.05f;
	const float bar_defaultScaleY = 1.1f;


	// 生成された後にWaveGenerateから呼ばれる
    public void Setup(WavePattern pattern)
    {
        // 受け取ったパターンに従ってブロックやアイテムを並べる
        pattern.Generate(this.transform, GetComponent<ManageWave>()); 
    }
    
    // 子オブジェクトのテキスト削除関数を呼んでからWaveを消す
    public void DestroyObject()
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

	public void RefreshAllChildren()
	{
		foreach(Transform child in this.transform)
        {
            if(child.CompareTag("item"))
            {
                var dynamicitem = child.GetComponent<DynamicHPModifier>();
				if(dynamicitem != null)
				{
					dynamicitem.Refresh();
				}
            }
			else
			{
				var taxArea = child.GetComponent<TaxArea>();
				if(taxArea != null)
				{
					taxArea.ChangeTaxAreaText();
				}
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

	/// <summary>
	/// ブロックまたはアイテムを配置するかしないか、
	/// 配置する場合 Block と Item のどちらを生成するか確率計算から決定する。
	/// Random だけど Block は必要ないという場合にフラグを使用する。
	/// </summary>
	/// <param name="needBlock"></param>
	/// <param name="needItem"></param>
    public void GenerateBlockOrItem(bool needBlock=true, bool needItem=true)
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
					if(needBlock) 
						InstantiateBlock(i);
                }
                else 
                {
					if(needItem) 
						InstantiateItem(i);
                }
            } 
        }
    }

    public void GenerateBarRandom()
    {
        // barは最大3本。左端と右端は不要
        for(int i = 1; i < 4; i++)
        {
            int p = Random.Range(0, 100);
            if(p < data.percentBar)
            {
                float barposX =  minx_bar + space*i;
                InstantiateBar(barposX, bar_defaultScaleY); 
            }
        }
    }

	public void InstantiateBar(float barposX, float scale_y)
	{
		var barpos = new Vector3(barposX, gameObject.transform.position.y,0f);
		var b = Instantiate(objRef.Bar, barpos, Quaternion.identity, gameObject.transform);
		b.GetComponent<Transform>().localScale = new Vector3(bar_scaleX, scale_y, 1f);
	}

    public void InstantiateBlock(int i)
    {
        Vector3 blockpos = ComputeBlockPos(i);
        Instantiate(objRef.Block, blockpos, Quaternion.identity, gameObject.transform);
        return;
    }

	void InstantiateItem(int i)
	{
		Vector3 itempos = ComputeBlockPos(i);
		GameObject item = itemLib.SelectItem(data);
		Instantiate(item, itempos, Quaternion.identity, gameObject.transform);
	}

    private Vector3 ComputeBlockPos(int i)
	{
		return new Vector3(minx_block + space*i, transform.position.y,0f);
	}
	
	// TaxArea
	public void InstantiateTaxArea(int side)
	{
		InstantiateTaxArea(side, objRef.TaxArea_increase);
		InstantiateTaxArea(-side, objRef.TaxArea_decrease);
	}

	GameObject InstantiateTaxArea(float sign, GameObject prefab)
    {
        GameObject area = Instantiate(prefab, gameObject.transform);
        area.transform.localPosition = new Vector3(1.4f*sign, 0f, 0f);
        return area;
    }
}
