using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BlockとBarの共通部分を抜き出す

public class Descent1 : MonoBehaviour
{
    public float speed;

    
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

    void Start()
    {
        // descent
        Move(transform.up * -1);
    }
    void Update()
    {
        /*
        if(Mathf.Approximately(GetComponent<Rigidbody2D>().velocity.y, 0f)){
            Debug.Log(GetComponent<Rigidbody2D>().velocity);
        }*/
        
    }

    
}
