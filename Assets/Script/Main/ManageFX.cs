using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アニメーション再生後のオブジェクトを削除する

public class ManageFX : MonoBehaviour
{
    void OnAnimationFinish()
    {
        Destroy (gameObject);
    }
}
