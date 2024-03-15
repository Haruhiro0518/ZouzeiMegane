using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerate : MonoBehaviour
{
    // 自動生成したいオブジェクトの端から端までの座標の大きさ
    private const float BlockHeight = 1.1f;
    // プレイ画面中に存在する行の数
    private const int maxRawInScreen = 10;
    // ちょうど画面の上からオブジェクト生成するためのオフセット
    private const float offset = 7.55f;

    private GameObject Player;
    private Transform playerTransform;
    public GameObject PlayerPrefab;

    // ステージの配列. プレハブをいれておく 
    public GameObject[] WavePrefabs;
    // 初めのTipIndex
    public int startTipIndex;
    // 現在のTipIndex
    int currentTipIndex;
    // ステージ生成の先読み個数
    public int preInstantiateNum;
    // 作ったステージの保持リスト
    public List<GameObject> generatedStageList = new List<GameObject>();

    // 行数を数えて、その値を参考に生成するオブジェクトを変える
    private int rawCount;

    public bool IsGameover = false;
    
    void Awake()
    {
        Player = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Player.name = ("Player");
        playerTransform = Player.GetComponent<Transform>();
    }
    
    void Start()
    {
        IsGameover = false;
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiateNum);
    }

    
    void Update()
    {
        if(IsGameover == true) return;

        // playerの位置から現在のステージチップインデックスを計算
        int charaPositionIndex = (int)(playerTransform.position.y / BlockHeight);
        // 次のステージチップに入ったらステージの更新処理を行う
        if(charaPositionIndex + preInstantiateNum > currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiateNum);
        }
    }

    // 指定のインデックスまでのステージを生成して、管理下に置く
    void UpdateStage(int toTipIndex)
    {
        if(toTipIndex <= currentTipIndex) return;

        // 指定のステージチップまで生成
        for(int i = currentTipIndex + 1; i <= toTipIndex; i++) {
            // 行数をカウント
            rawCount++;

            GameObject stageObject = GenerateStage(i);
            generatedStageList.Add(stageObject);
        }
        // 同時に存在することができるステージの個数を超えている場合, ステージ削除
        while(generatedStageList.Count > preInstantiateNum + maxRawInScreen) {
            DestoryOldStage();
        }
        currentTipIndex = toTipIndex;
    }

    // 指定のインデックス位置にstageオブジェクトを生成
    GameObject GenerateStage(int tipIndex)
    {
        // prefabsの中からどのプレハブを生成するかを選ぶ
        // int nextStageTip = Random.Range(0, WavePrefabs.Length);
        int nextStageTip = SelectStage();

        // nextStageTip番目のオブジェクトを生成
        GameObject stageObject = (GameObject)Instantiate(
            WavePrefabs[nextStageTip],    
            new Vector3(0, tipIndex * BlockHeight + offset, 0),     // y軸方向に無限に生成. 位置はPlayerより上
            Quaternion.identity) as GameObject;
        
        return stageObject;
    }

    // 一番古いステージを削除
    void DestoryOldStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        DestroyWave wave = oldStage.GetComponent<DestroyWave>();
        
        wave.destroyObject();
    }
    
    // 生成するステージの選択
    int nextWavePrefab;
    int SelectStage()
    {
        
        // rawCount 0~9 random, 10~15 full or random, 16 full

        // 10行生成していないとき
        if(rawCount < 10)
        {
            // 0を除いたランダム
            nextWavePrefab = Random.Range(1, WavePrefabs.Length);
        } 
        // 10から15の間
        else if(rawCount >= 10 && rawCount <= 15)
        {
            int sel;
            sel = Random.Range(0,2);

            // full or not full
            if(sel == 0){
                nextWavePrefab = 0;   // full
                rawCount = 0;
            } 
            else if(sel == 1) {
                nextWavePrefab = Random.Range(1, WavePrefabs.Length); // random
            }
        }
        // 15超えたら
        else {
            nextWavePrefab = 0;   // full
            rawCount = 0;
        }


        return nextWavePrefab;
    }

}
