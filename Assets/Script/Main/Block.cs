using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // ブロックの辺の長さの半分
    private float sideLength;
    // 降下するスピード
    public float speed = 2f;

    // Animator宣言
    private Animator anim;

    void Start()
    {
        // Animator取得
        anim = gameObject.GetComponent<Animator>();
        // ブロックの辺の長さ/2
        sideLength = GetComponent<Transform>().transform.localScale.y / 2;
        // 移動
        Move(transform.up * -1);
    }

    // ブロックの移動
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

    void Update()
    {
        
    }

    // 衝突したとき
    void OnCollisionEnter2D(Collision2D c)
    {
        // 相手の座標取得
        Vector2 pos = c.gameObject.transform.position;
        // Block自身のy座標
        float by = gameObject.transform.position.y;
        // 相手の大きさ
        float radius = c.transform.localScale.y/2;

        // 衝突したのがblockの下面であるかの判定
        if(by-radius-sideLength >= pos.y) {
        
            // プレイヤーの削除
            // Destroy(c.gameObject);

            // animation 再生
            anim.SetBool("Damage", true);

        } else {
            c.gameObject.transform.position = pos;
            GetComponent<Rigidbody2D>().velocity = (transform.up * -1) * speed;
        }
        
    }

    void OnCollisionExit2D(Collision2D c) 
    {
        GetComponent<Rigidbody2D>().velocity = (transform.up * -1) * speed;
    }
    void OnAnimationFinish()
    {
        anim.SetBool("Damage", false);
        // Destroy(gameObject);
    }
}
