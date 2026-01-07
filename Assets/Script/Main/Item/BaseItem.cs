using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 衝突時にSE再生、Event、オブジェクト削除するクラス
/// Playerのシングルトンから、処理を呼ぶことも可
/// </summary>
public class BaseItem: MonoBehaviour
{
	public UnityEvent onHitEvent;	// UI消去などの演出用
	[SerializeField] private GameObject SEitem;
	// [SerializeField] private bool destroyOnHit = true;

	// アイテムの振る舞いリスト. Item の種類が増えたら増やす
    public enum PlayerAction { None, HalveHP, StartObstacleUI}	// , FullHeal}
    
    [Header("Playerへの特殊効果")]
    [SerializeField] private PlayerAction playerAction = PlayerAction.None;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
			if(SEitem) Instantiate(SEitem);
            ExecutePlayerAction();
            // 登録された汎用イベント（演出やUI削除など）を実行
            onHitEvent?.Invoke();
            Destroy(gameObject);
        }
    }

    private void ExecutePlayerAction()
    {
        if (playerAction == PlayerAction.None) return;

        switch (playerAction)
        {
            case PlayerAction.HalveHP:
				GameManager.Instance.Player.HalveHP();
                break;
            case PlayerAction.StartObstacleUI:
				// TODO : 小泉のシステム。アニメーション再生とか
                break;
            // 処理が増えたら追加
        }
    }
}
