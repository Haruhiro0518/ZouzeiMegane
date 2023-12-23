using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 2, -1);
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // playerのy座標値についていく
        Vector3 pos = transform.position;
        pos.y = player.transform.position.y + offset.y;
        pos.z = player.transform.position.z + offset.z;
        transform.position = pos;
        
    }
}
