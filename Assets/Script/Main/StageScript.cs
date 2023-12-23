using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript : MonoBehaviour
{
    // 自動生成したいオブジェクトの端から端までの座標の大きさ
    const float StageTipSize = 1.1f;
    // プレイ画面中に存在する行の数
    const int maxRaw = 10;
    // ちょうど画面の上からオブジェクト生成するためのオフセット
    private float offset = 7.55f;
    // 現在のTipIndex
    int currentTipIndex;
    // Playerの位置
    public Transform player;
    // ステージの配列. プレハブをいれておく
    public GameObject[] stageTips;
    // 初めのTipIndex
    public int startTipIndex;
    // ステージ生成の先読み個数
    public int preInstantiate;
    // 作ったステージの保持リスト
    public List<GameObject> generatedStageList = new List<GameObject>();

    // 行数を数えて、その値を参考に生成するオブジェクトを変える
    private int rawCount;
    
    void Start()
    {
        // 初期化
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiate);

    }

    
    void Update()
    {
        // playerの位置から現在のステージチップインデックスを計算
        int charaPositionIndex = (int)(player.position.y / StageTipSize);
        // 次のステージチップに入ったらステージの更新処理を行う
        if(charaPositionIndex + preInstantiate > currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    // 指定のインデックスまでのステージを生成して、管理下に置く
    void UpdateStage(int toTipIndex)
    {
        // 
        if(toTipIndex <= currentTipIndex) return;

        // 指定のステージチップまで生成
        for(int i = currentTipIndex + 1; i <= toTipIndex; i++) {
            // 行数をカウント
            rawCount++;
            // generate
            GameObject stageObject = GenerateStage(i);
            generatedStageList.Add(stageObject);
        }
        // 同時に存在することができるステージの個数を超えている場合, ステージ削除
        while(generatedStageList.Count > preInstantiate + maxRaw) {
            DestoryOldStage();
        }
        currentTipIndex = toTipIndex;
    }

    // 指定のインデックス位置にstageオブジェクトを生成
    GameObject GenerateStage(int tipIndex)
    {
        // prefabsの中からどのプレハブを生成するかを選ぶ
        // int nextStageTip = Random.Range(0, stageTips.Length);
        int nextStageTip = SelectStage();

        // nextStageTip番目のオブジェクトを生成
        GameObject stageObject = (GameObject)Instantiate(
            stageTips[nextStageTip],    
            new Vector3(0, tipIndex * StageTipSize + offset, 0),     // y軸方向に無限に生成. 位置はPlayerより上
            Quaternion.identity) as GameObject;
        
        return stageObject;
    }

    // 一番古いステージを削除
    void DestoryOldStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
    
    // 生成するステージの選択
    int SelectStage()
    {
        int nextStageTip;

        nextStageTip = Random.Range(0, stageTips.Length);

        return nextStageTip;
    }
}
