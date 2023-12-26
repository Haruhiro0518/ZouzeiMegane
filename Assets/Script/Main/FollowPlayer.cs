using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 2.5f, -1);

    // Stageオブジェクトしている
    [SerializeField] private GameObject Stage;
    // stagescript
    private StageScript stageScript;
    
    void Start()
    {
        // StageScript取得
        stageScript = Stage.GetComponent<StageScript>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // gameOverだったらfollowplayerを停止する
        if(stageScript.IsGameover == true) return;
        // playerのy座標値についていく
        Vector3 pos = transform.position;
        pos.y = player.transform.position.y + offset.y;
        pos.z = player.transform.position.z + offset.z;
        transform.position = pos;
        
    }
}
