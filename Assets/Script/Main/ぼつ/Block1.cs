using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block1 : MonoBehaviour
{
    // ブロックの辺の長さの半分
    private float sideLength;
    // 降下するスピード
    public float speed = 2f;

    // Animator宣言
    private Animator anim;

    // 親オブジェクト
    public GameObject parent;
    // BlockManagerコンポーネント
    BlockManager1 manager;


    void Start()
    {
        // Animator取得
        anim = gameObject.GetComponent<Animator>();
        // Descentコンポーネント
        // descent = gameObject.GetComponent<Descent>();
        // 親オブジェクト取得
        parent = transform.parent.gameObject;
        // BlockManagerコンポーネントを取得
        manager = parent.GetComponent<BlockManager1>();

        // ブロックの辺の長さ/2
        sideLength = GetComponent<Transform>().transform.localScale.y / 2;
        // 移動
        Move(transform.up * -1);
    }

    void Update()
    {
        // BlockManagerのIsMovingがfalseのとき
        if(manager.IsMoving == false) {
            // 動きを止める
            Move(Vector3.zero);
        }
        // IsMovingがtrueなら動く
        else if(manager.IsMoving == true) {
            Move(transform.up * -1);
        }
    }

    // 衝突したとき
    void OnCollisionEnter2D(Collision2D c)
    {
        // BlockManagerに接触したことを伝える
        manager.colCount += 1;
        manager.IsMoving = false;

        Damage(c);
        
    }

    // 接触中
    void OnCollisionStay2D(Collision2D c)
    {
        // 相手の座標取得
        Vector2 pos = c.gameObject.transform.position;
        // Block自身のy座標
        float by = gameObject.transform.position.y;
        // 相手の大きさ
        float radius = c.transform.localScale.y/2;

        // 衝突しているのがblockの真下面であるかの判定
        if(by-radius-sideLength >= pos.y - 0.05) {
            // Debug.Log("真下");
        } 
        else {
            // Debug.Log(by-radius-sideLength);
        }
    }

    // 離れた時
    void OnCollisionExit2D(Collision2D c) 
    {
        // BlockManagerに離れたことを報告
        manager.colCount -= 1;
        // 他に接触しているcolliderがない場合はIsMovingをtrueにする
        if(manager.colCount == 0) {
            manager.IsMoving = true;
        }
        
        
    }

    void OnAnimationFinish()
    {
        anim.SetBool("Damage", false);
        // Destroy(gameObject);
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

    public void Damage(Collision2D c) 
    {
        // 相手の座標取得
        Vector2 pos = c.gameObject.transform.position;
        // Block自身のy座標
        float by = gameObject.transform.position.y;
        // 相手の大きさ
        float radius = c.transform.localScale.y/2;

        // 衝突したのがblockの真下面であるかの判定
        if(by-radius-sideLength >= pos.y - 0.05) {
        
            // プレイヤーの削除
            // Destroy(c.gameObject);
            // Debug.Log(GetComponent<Rigidbody2D>().velocity);

            // animation 再生
            anim.SetBool("Damage", true);

        } 
    }
}
