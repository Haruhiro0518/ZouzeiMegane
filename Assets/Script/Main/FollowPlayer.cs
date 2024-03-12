using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カメラをプレイヤーに追従させるクラス
public class FollowPlayer : MonoBehaviour
{
    private GameObject Player;
    private Player player;
    private Vector3 offset = new Vector3(0, 2.5f, -1);
    private Vector3 extraoffset = new Vector3(0, 0f, -1);
    
    private float divisionNumber = 1f / 125f;
    private float extraoffset_ymax = 1f;
    private const float delayseconds = 0.01f;

    [SerializeField] private GameObject WaveGenerator;
    
    private WaveGenerate waveGenerate;
    
    void Start()
    {
        waveGenerate = WaveGenerator.GetComponent<WaveGenerate>();
        Player = GameObject.Find("Player");
        player = Player.GetComponent<Player>();
    }

    // Updateでプレイヤーの位置を計算した後にLateUpdateでカメラを追従させる
    void LateUpdate()
    {
        if(waveGenerate.IsGameover == true) return;

        Vector3 pos = transform.position;
        pos.y = Player.transform.position.y + offset.y + extraoffset.y;
        pos.z = Player.transform.position.z + offset.z;
        transform.position = pos;
        
    }

    // プレイヤーが衝突していたらextraoffsetの上限まで上昇、そうでなければ0まで下降させる
    // extraoffsetの変化はy = x(2 - x)に従う
    private float x = 0f;
    public IEnumerator CameraMoveupOrDown()
    {
        
        while(true)
        {
            if(player.IsCollisionStay == true) {
                if(extraoffset.y >= extraoffset_ymax) {
                    yield break;
                }
                // ease-out
                extraoffset.y = x*(2 - x);
                x += divisionNumber;
            
            }
            else if(player.IsCollisionStay == false) {
                if(extraoffset.y <= 0f) {
                    yield break;
                }
                // ease-in
                extraoffset.y = x*(2 - x);
                x -= divisionNumber;
            }

            yield return new WaitForSeconds(delayseconds);
        }
    }

}
