using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// GlassesはmanageHPUIのDestroyTextメソッドでDestoryできないため、
// GalssesのBlockへの参照がnullになったら自動的にDestoryする
public class Glasses : MonoBehaviour
{
    
    FollowTransform followTransform;

    void Start()
    {
        followTransform = gameObject.GetComponent<FollowTransform>();
    }

    
    void Update()
    {
        if(followTransform != null) {
            destoryCheck();
        }
    }

    void destoryCheck()
    {
        if(followTransform._objectTransform == null) {
            Destroy(gameObject);
        }
    }
}
