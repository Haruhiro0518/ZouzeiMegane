using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // 降下するスピード
    public float speed = 2f;

    // Animator宣言
    private Animator anim;

    void Start()
    {
        // Animator取得
        anim = gameObject.GetComponent<Animator>();
        // 移動
        Move(transform.up * -1);
    }

    // ブロックの移動
    public void Move(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void Update()
    {
        
    }

    // 衝突したとき
    void OnCollisionEnter2D(Collision2D c)
    {
        // プレイヤーの削除
        Destroy(c.gameObject);

        // animation 再生
        anim.SetBool("Damage", true);
    }

    void OnAnimationFinish()
    {
        anim.SetBool("Damage", false);
        // Destroy(gameObject);
    }
}
