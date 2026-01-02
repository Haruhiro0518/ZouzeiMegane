using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave/Random")]
public class RandomWavePattern : WavePattern
{
	[SerializeField] bool needBlock = true,needItem = true;
	public override void Generate(Transform parent, ManageWave mw)
	{
		mw.GenerateBlockOrItem(needBlock, needItem);
		mw.GenerateBar();
	}   
}
