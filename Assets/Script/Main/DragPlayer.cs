using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPlayer : MonoBehaviour
{
    // 当たり判定調整に使うレイヤーマスク
    private int layermask;
    
    public float sensitivity;

    void Start()
    {
        // playerのlayer7とitemのlayer9, taxareaのlayer10, insidescreenのlayer11を無視する
        layermask = (1 + 4 + 8 + 16) << 7;
        layermask = ~layermask;

        sensitivity = SettingManager.instance.dragSensitivity;
        if(init_player_pos == true) DebugPos();
    }

    private float previousPosX;
    private float currentPosX;
    // x座標をclampするときの最小値、最大値
    private float xMin;
    private float xMax;

    // ドラッグ処理
    public void Drag() 
    {
        if(Time.timeScale == 0) return;

        // mouse左ボタンを押したとき
        if (Input.GetMouseButtonDown(0)) {
            previousPosX = Input.mousePosition.x;
            
        }
        // mouse左ボタンを押している間
        if(Input.GetMouseButton(0)) {
            
            currentPosX = Input.mousePosition.x;
            // 移動距離の計算　Screen.widthで割って1に正規化. 定数をかけて,マウスの移動とplayerが一致
            float diffDistance = (currentPosX - previousPosX) / Screen.width * displayWidth*2;

            /// 感度乗算. (1 + x)倍にする (ただし,0 <= x <= 1)
            diffDistance *= (1 + sensitivity);

            UpdateXminXmax();
            Vector2 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x + diffDistance,  xMin,  xMax);
            transform.position = pos;

            // タップ位置を更新
            previousPosX = currentPosX;
        } 
    }

    // 当たり判定に使う定数
    private const float ColliderRadius = 0.1965f;
    private const float PlayerScale = 0.195f;
    private const float colYoffset = 0.8f * PlayerScale;
    private const float colXoffset = 0.1f * PlayerScale;
    private float displayWidth = 2.8f;
    // レイキャスト位置のを少し下にずらす. これにより通り過ぎたブロックと下部が衝突しないようにし、
    // 上部では正しい衝突時にレイキャストが誤作動しないようにする. 実験で決めた値
    private float space = 0.0525f;

    // プレイヤーの球コライダーの上部と下部から、水平方向にレイキャストを伸ばす
    // レイキャストがぶつかったブロックの端の座標をclampするx座標のxmin or xmaxとする
    void UpdateXminXmax()
    {

        RaycastHit2D topL = Physics2D.Raycast(transform.position+new Vector3(colXoffset,colYoffset+ColliderRadius-space*2,0), Vector2.left, displayWidth*2, layermask);
        RaycastHit2D topR = Physics2D.Raycast(transform.position+new Vector3(colXoffset,colYoffset+ColliderRadius-space*2,0), Vector2.right, displayWidth*2, layermask);
        RaycastHit2D botL = Physics2D.Raycast(transform.position+new Vector3(colXoffset,colYoffset-ColliderRadius-space,0), Vector2.left, displayWidth*2, layermask);
        RaycastHit2D botR = Physics2D.Raycast(transform.position+new Vector3(colXoffset,colYoffset-ColliderRadius-space,0), Vector2.right, displayWidth*2, layermask);

        
        if(topL.collider != null || botL.collider != null) {
            
            if(botL.collider != null) xMin = botL.point.x + ColliderRadius - colXoffset + space;
            else xMin = topL.point.x + ColliderRadius - colXoffset + space;
        } else {
            xMin = -displayWidth + ColliderRadius - colXoffset;
        }
        

        if(topR.collider != null || botR.collider != null) {
            if(botR.collider != null) xMax = botR.point.x - ColliderRadius - colXoffset - space;
            else xMax = topR.point.x - ColliderRadius - colXoffset - space; 
        } else {
            xMax = displayWidth - ColliderRadius - colXoffset;
        }

    }

    // unityroomで実行するときにplayerの位置が原点からずれてしまうため
    // 最初にプレイヤーを原点に配置する。
    private bool init_player_pos = true;
    void DebugPos()
    {
        Vector3 pos = new Vector3(0,0,0);
        gameObject.transform.position = pos;

        init_player_pos = false;
    }
}
