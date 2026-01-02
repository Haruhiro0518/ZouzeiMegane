using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assets->Create->ScriptableObj->Create ObjectReference でインスタンス化して使用する
[CreateAssetMenu(menuName = "ScriptableObj/Create ObjRefData")]
public class ObjectReference : ScriptableObject
{
	[SerializeField] private GameObject block, bar;
	public GameObject Block => block;
	public GameObject Bar => bar;
}