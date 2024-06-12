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
    private int preInstantiateNum = 2;
    // 作ったステージの保持リスト
    public List<GameObject> GeneratedWaveList = new List<GameObject>(13);

    // 行数を数えて、その値を参考に生成するオブジェクトを変える
    private int rawCount_full;
    private int rawCount_tax;

    public enum kindsOfWaves {
        full,
        random,
        empty,
        tax,
        random_noblock
    }

    public bool IsGameOver = false;
    public bool IsGameClear = false;
    
    void Awake()
    {
        Player = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Player.name = ("Player");
        playerTransform = Player.GetComponent<Transform>();
    }

    
    void Start()
    {
        IsGameOver = false;
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiateNum);
    }

    
    void Update()
    {
        if(IsGameOver == true || IsGameClear == true) return;

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
            rawCount_full++;
            rawCount_tax++;
            // Debug.Log("full count:"+rawCount_full+"::: tax count:"+rawCount_tax);

            GameObject waveObject = GenerateWave(i);
            GeneratedWaveList.Add(waveObject);
        }
        // 同時に存在することができるステージの個数を超えている場合, ステージ削除
        while(GeneratedWaveList.Count > preInstantiateNum + maxRawInScreen) {
            DestroyOldWave();
        }
        currentTipIndex = toTipIndex;
    }


    // 指定のインデックス位置にstageオブジェクトを生成
    GameObject GenerateWave(int tipIndex)
    {
        // prefabsの中からどのプレハブを生成するかを選ぶ
        int nextWaveTip = SelectWave();
    
        // nextWaveTip番目のオブジェクトを生成
        GameObject waveObject = (GameObject)Instantiate(
            WavePrefabs[nextWaveTip],
            new Vector3(0, tipIndex * BlockHeight + offset, 0),     // y軸方向に無限に生成. 
            Quaternion.identity) as GameObject;

        return waveObject;
    }


    // 生成するステージの選択
    // waveが生成されるたびにrawCount_full / _tax変数がインクリメントされているため、
    // それぞれが数える行数に応じて、nextWavePrefabを決定する。
    // kindsOfWavesはWavePrefabs[]に格納されているプレハブの順番に等しい。
    int nextWavePrefab;
    int random_sel;
    Stack<int> stack = new Stack<int>();

    int SelectWave()
    {
        // stackの中身がある場合はstackを優先する
        if(stack.Count != 0) {
            return nextWavePrefab = stack.Pop();
        }
        
        if(rawCount_full < 10)
        {
            nextWavePrefab = Push_WaveRandom();
        }
        else if(rawCount_full >= 10 && rawCount_full <= 15)
        {
            random_sel = Random.Range(0,2);
            if(random_sel == 0){
                nextWavePrefab = Push_WaveRandom();
            } 
            else if(random_sel == 1) {
                nextWavePrefab = Push_WaveFull();   
                rawCount_full = 0;
            }
        }
        else {  // rawCount_full > 15
            nextWavePrefab = Push_WaveFull();   
            rawCount_full = 0;
            return nextWavePrefab;
        }

        // rawCount_tax 20~39 1%, 40~60 80%, 70 taxarea
        random_sel = Random.Range(0, 100);
        if(rawCount_tax >= 20 && rawCount_tax < 40) {
        
            if(random_sel < 1) {
                nextWavePrefab = Push_WaveTax();
                rawCount_tax = 0;
            } else {
                return nextWavePrefab;
            }
        } else if(rawCount_tax >= 40 && rawCount_tax < 60) {

            if(random_sel < 70) {
                nextWavePrefab = Push_WaveTax();
                rawCount_tax = 0;
            } else {
                return nextWavePrefab;
            }
        } else if(rawCount_tax >= 70) {
            nextWavePrefab = Push_WaveTax();
            rawCount_tax = 0;
        }

        return nextWavePrefab;
    }


    // それぞれのwaveは複数のwaveを組み合わせるため、Stackを使用する
    int Push_WaveTax()
    {
        stack.Push((int)kindsOfWaves.empty);
        stack.Push((int)kindsOfWaves.empty);
        stack.Push((int)kindsOfWaves.tax);
        stack.Push((int)kindsOfWaves.empty);
        stack.Push((int)kindsOfWaves.empty);

        return stack.Pop();
    }

    int Push_WaveRandom()
    {
        stack.Push((int)kindsOfWaves.random_noblock);
        stack.Push((int)kindsOfWaves.random);

        return stack.Pop();
    }

    int Push_WaveFull()
    {
        stack.Push((int)kindsOfWaves.random_noblock);
        stack.Push((int)kindsOfWaves.full);

        return stack.Pop();
    }

    // 一番古いウェーブを削除
    void DestroyOldWave()
    {
        GameObject oldWave = GeneratedWaveList[0];
        GeneratedWaveList.RemoveAt(0);
        ManageWave wave = oldWave.GetComponent<ManageWave>();
        
        wave.destroyObject();
    }

    public void AllItemSmokeAndChangeParam()
    {
        for(int i = 0; i < GeneratedWaveList.Count; i++)
        {
            GeneratedWaveList[i].GetComponent<ManageWave>().ItemSmokeAndChangeParam();
        }
    }

    public void AccelerateNextTaxArea(int incrementValue)
    {
        rawCount_tax += incrementValue;
    }

}
