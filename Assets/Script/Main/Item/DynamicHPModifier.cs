using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicHPModifier : MonoBehaviour
{
	private int _hp = 0;
    public int HP => _hp;
    // コンポ―ネント
    private ManageHPUI manageHPUI;
	private Player player;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Effect;
	// アイテムごとにValueDataのScriptableObjectを作成して、data にセット
    [SerializeField] private ValueData data;
	[SerializeField] private bool hasDamageAnim = false;

    void Start()
    {
        player = GameManager.Instance.Player;
        manageHPUI = GetComponent<ManageHPUI>();
        
        ChangeAnimByPlayerMode();
        SetHPFromData();
    }

	public void Refresh() // 「税率やプレイヤー状態に合わせる」という一括処理
	{
		SetHPFromData();
		ChangeAnimByPlayerMode();
		GenEffect();
	}

	public void SetHPFromData()
    {
		if(data != null)
		{
			// maxItemHP+1するのは、[min, max)を[min, max]にするため
        	_hp = Random.Range(data.maxItemHP, data.minItemHP+1);
		}
		if(manageHPUI != null)
		{
			manageHPUI.ChangeText(HP.ToString());
		}
    }

	// public void Smoke()
	public void GenEffect()
    {
        Instantiate(Effect, gameObject.transform.position, Quaternion.identity);
    }
    
    public void ChangeAnimByPlayerMode()
    {
		if(hasDamageAnim)
		{
			// 無敵状態なら damage のアニメーションになる
			animator.SetBool("damage", player.IsInvincible);
		} 
    }
	/// <summary>
	/// BaseItem.cs の onHitEvent に登録すると、衝突した時にプレイヤーHPを変更できる
	/// </summary>
	public void Execute()
	{
		player.AddHP(_hp);
	}
}
