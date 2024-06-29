using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BlockはmanageHPUIを使わずにGlassesを保持するため、BlockのGlasses用のクラス
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
