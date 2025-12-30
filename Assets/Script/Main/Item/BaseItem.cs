using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 衝突時にSE再生、Event、オブジェクト削除するクラス
/// アイテムごとに必要な処理をonHitEventに登録
/// </summary>
public class BaseItem: MonoBehaviour
{
	public UnityEvent onHitEvent;
	[SerializeField] private GameObject SEitem;
	[SerializeField] private bool destroyOnHit = true;

	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		onHitEvent?.Invoke();
		if(SEitem) Instantiate(SEitem);
		if(destroyOnHit) Destroy(gameObject);
	}
}
