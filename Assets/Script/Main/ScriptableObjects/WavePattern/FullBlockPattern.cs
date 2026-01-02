using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave/Full")]
public class FullBlockPattern : WavePattern
{
	[SerializeField] bool needBlock = true;
    public override void Generate(Transform parent, ManageWave mw) //, ItemLibrary library, ValueData data)
    {
        for(int i = 0; i < ManageWave.NumOfGrids; i++) // NumOfGridsの数だけ
        {
			if(needBlock)
			{
				mw.InstantiateBlock(i);
			}
				
		}
    }
}