using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assets->Create->ScriptableObj->Create ObjectReference でインスタンス化して使用する
[CreateAssetMenu(menuName = "ScriptableObj/Create ObjRefData")]
public class ObjectReference : ScriptableObject
{
	[SerializeField] private GameObject block, bar;
	[SerializeField] private GameObject taxArea_increase, taxArea_decrease;
	public GameObject Block => block;
	public GameObject Bar => bar;
	public GameObject TaxArea_increase => taxArea_increase;
	public GameObject TaxArea_decrease => taxArea_decrease;
}