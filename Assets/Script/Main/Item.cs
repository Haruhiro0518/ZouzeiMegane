using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int HP;
    [System.NonSerialized] public int maxHP;
    // コンポ―ネント
    private ManageHPUI manageHPUI;
    private Player player;
    [SerializeField] private GameObject SEitem;
    // ScriptableObject
    [SerializeField] private ValueData valueData;


    void Awake()
    {
        
    }
    
    void Start()
    {
        player = GameObject.Find("Player").gameObject.GetComponent<Player>();
        manageHPUI = gameObject.GetComponent<ManageHPUI>();
        
        SetItemHP();
        manageHPUI.ChangeText(HP.ToString());
    }

    
    void OnTriggerEnter2D(Collider2D c)
    {
        // playerと衝突することだけを考える
        // playerのHPにItemのHPを足す
        player.HP += HP;

        manageHPUI.DestroyText();
        Instantiate(SEitem);
        Destroy(gameObject);
    }

    
    void SetItemHP()
    {
        valueData.ChangeItemHP_Percent(player.taxRate);
        maxHP = valueData.maxItemHP;
        HP = Random.Range(1, maxHP);
    }

    
}
