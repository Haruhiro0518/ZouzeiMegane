using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Playerがブロック1つの高さ移動するごとに、ウェーブを生成するクラス

public class WaveGenerate : MonoBehaviour
{
    // インデックスの高さ
    private const float BlockHeight = 1.1f;
    // プレイ画面中に存在する行の数
    private const int maxRawInScreen = 10;
    // PlayerからMainCamera上部までの高さ
    private const float offset = 7.55f;

    private GameObject Player;
    private Transform playerTransform;
    [SerializeField] private GameObject PlayerPrefab;

    // ウェーブの配列. プレハブをいれておく 
    public GameObject[] WavePrefabs;

    // 初めにウェーブを生成する位置のインデックス
    private const int startTipIndex = 0;
    // ウェーブ生成位置を指定するインデックス
    private int currentTipIndex;
    // 画面外であらかじめ生成しておくウェーブ数
    private const int preInstantiateNum = 2;
    // 作ったウェーブの保持リスト
    public List<GameObject> GeneratedWaveList = new List<GameObject>(13);

    // 行数を数えて、その値を参考に生成するオブジェクトを変える
    private int rawCount_full;
    private int rawCount_tax;

    // WavePrefabs[]に格納されているプレハブに上から名前を付ける
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
        currentTipIndex = startTipIndex - 1; // TipIndex(0-1=-1)まで生成していたことにする
        if(preInstantiateNum > currentTipIndex) {
            UpdateWave(preInstantiateNum);
        }
    }

    
    void Update()
    {
        if(IsGameOver == true || IsGameClear == true || Player == null) return;

        // 現在のプレイヤー位置のインデックス.(player.posy=0だから初期値は0)
        int playerPosIndex = (int)(playerTransform.position.y / BlockHeight);
        
        // 左辺は次のウェーブを生成するインデックス。右辺は現在最も新しいウェーブを生成したインデックス
        if(playerPosIndex + preInstantiateNum > currentTipIndex)
        {
            UpdateWave(playerPosIndex + preInstantiateNum);
        }
    }


    // 指定のインデックスまでのウェーブを生成して、管理下に置く
    void UpdateWave(int toTipIndex)
    {
        if(currentTipIndex == toTipIndex) return;

        // currentTipIndex（ウェーブ生成位置）を 前回のウェーブ更新のtoTipIndex から1つ上の位置とする
        currentTipIndex++;

        // 指定のステージチップまで生成
        for(int i = currentTipIndex; i <= toTipIndex; i++) {
            // 行数をカウント
            rawCount_full++;
            rawCount_tax++;

            GameObject waveObject = GenerateWave(i);
            GeneratedWaveList.Add(waveObject);
        }
        // 同時に存在することができるウェーブ数を超えている場合, 古いウェーブから削除
        while(GeneratedWaveList.Count > preInstantiateNum + maxRawInScreen) {
            DestroyOldWave();
        }
        currentTipIndex = toTipIndex;
    }


    // 指定のインデックス位置にウェーブを生成
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


    // 生成するウェーブの選択
    // waveが生成されるたびにrawCount_full / _tax変数がインクリメントされているため、
    // それぞれが数える行数に応じて、nextWavePrefabを決定する。
    int nextWavePrefab;
    int random_sel;
    Stack<int> stack = new Stack<int>();

    int SelectWave()
    {
        // stackの中身がある場合はstackを優先する
        if(stack.Count != 0) {
            return nextWavePrefab = stack.Pop();
        }
        
        // fullを生成するか判定
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

        // taxを生成するか判定.
        // rawCount_tax 20~39 1%, 40~60 80%, 70 taxarea
        random_sel = Random.Range(0, 100);
        if(rawCount_tax >= 20 && rawCount_tax < 40) {
        
            if(random_sel < 1) {
                nextWavePrefab = Push_WaveTax();
            } else {
                return nextWavePrefab;
            }
        } else if(rawCount_tax >= 40 && rawCount_tax < 60) {

            if(random_sel < 70) {
                nextWavePrefab = Push_WaveTax();
            } else {
                return nextWavePrefab;
            }
        } else if(rawCount_tax >= 70) {
            nextWavePrefab = Push_WaveTax();
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

        rawCount_tax = 0;
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
    
    // playerが無敵に入るとき・出るときに呼ばれる
    public void AllItemSmokeAndChangeParam()
    {
        for(int i = 0; i < GeneratedWaveList.Count; i++)
        {
            GeneratedWaveList[i].GetComponent<ManageWave>().ItemSmokeAndChangeParam();
        }
    }
    // playerが無敵状態かつ150%以上で、増税するときに呼ばれる.
    [SerializeField] AudioManager audioManager;
    public void NextBlockWaveSetGlasses()
    {
        int WaveTaxIndex = 6;  // playerがTaxAreaと衝突する際の、Wave-Taxの一番上のインデックス

        for(int i = WaveTaxIndex + 1; i < GeneratedWaveList.Count; i++) 
        {
            int blockcount =  GeneratedWaveList[i].GetComponent<ManageWave>().BlockSmokeAndSetGlasses();
            if(blockcount > 0) {    // ウェーブにblockが含まれていたら終了
                audioManager.Play_Smoke();
                break;
            }
        }
    }

    public void AccelerateNextTaxArea(int incrementValue)
    {
        // TaxAreaが生成され、Playerと衝突するときのrawCount_tax = 11.
        // TaxAreaが同時に2つ生成される可能性があるのは、rawCount_taxが0~11の間に
        // incrementValueが足されて20以上になったときである。よってその場合はrawCount_tax<20に調整する
        if(rawCount_tax < 12) { // Wave-Taxが生成されて間もない場合
            rawCount_tax = 19;
        } else {
            rawCount_tax += incrementValue;
        }
    }

}
