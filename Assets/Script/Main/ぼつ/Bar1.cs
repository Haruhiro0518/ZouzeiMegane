using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar1 : MonoBehaviour
{
    public float speed = 2f;
    // 親オブジェクト
    public GameObject parent;
    // BlockManagerコンポーネント
    BlockManager1 manager;
    
    void Start()
    {
        // 親オブジェクト取得
        parent = transform.parent.gameObject;
        // BlockManagerコンポーネントを取得
        manager = parent.GetComponent<BlockManager1>();


        Move(transform.up*-1);
    }

    
    void Update()
    {
        if(Mathf.Approximately(GetComponent<Rigidbody2D>().velocity.y, 0f)){
            Debug.Log(GetComponent<Rigidbody2D>().velocity);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {

    }
    // 離れた時
    void OnCollisionExit2D(Collision2D c) 
    {
        Move(transform.up * -1);
    }

    // 移動
    public void Move(Vector2 direction)
    {
        // ブロックの座標を取得
        /*
        Vector2 pos = transform.position;
        pos += direction * speed * Time.deltaTime;
        transform.position = pos;
        */
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
