using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int HP;
    [System.NonSerialized] public int maxHP;
    [System.NonSerialized] public int minHP;
    // コンポ―ネント
    private ManageHPUI manageHPUI;
    private Player player;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject SEitem;
    [SerializeField] private GameObject FXsmoke;
    // ScriptableObject
    [SerializeField] private ValueData data;

    
    void Start()
    {
        player = GameObject.Find("Player").gameObject.GetComponent<Player>();
        manageHPUI = gameObject.GetComponent<ManageHPUI>();
        
        ItemDamageAnimOnOff();
        SetItemHP();
    }

    
    void OnTriggerEnter2D(Collider2D c)
    {
        // playerとの衝突のみ考える
        ComputePlayerHP();

        Instantiate(SEitem);
        manageHPUI.DestroyText();
        Destroy(gameObject);
    }

    void ComputePlayerHP()
    {
        player.HP += HP;
        if(player.HP <= 0) {
            player.HP = 0;
        }
    }

    public void SetItemHP()
    {
        // maxItemHP+1するのは、[min, max)を[min, max]にするため
        HP = Random.Range(data.maxItemHP, data.minItemHP+1);
        manageHPUI.ChangeText(HP.ToString());
    }

    public void Smoke()
    {
        Instantiate(FXsmoke, gameObject.transform.position, Quaternion.identity);
    }
    
    public void ItemDamageAnimOnOff()
    {
        if(player.IsInvincible == true)
        {
            animator.SetBool("damage", true);
        } else {
            animator.SetBool("damage", false);
        }
    }
    
}
