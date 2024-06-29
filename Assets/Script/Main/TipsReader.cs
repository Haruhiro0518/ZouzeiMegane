using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

// MenueMangerにアタッチ

public class TipsReader : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile; 
    private List<string[]> csvData = new List<string[]>(); 
    [SerializeField] GameObject[] TipsImage;

    public void ReadCSV()
    {
        StringReader reader = new StringReader(csvFile.text); 

        // 1行を読み込んでカンマごとに要素を区切り、csvDataリストに順番に加える。list[行][列]
        // それをcsvFileの終わりまでループする
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine(); 
            csvData.Add(line.Split(',')); 
        }
    }

    [SerializeField] Transform TipsParent;
    public void SetTips(TMPro.TMP_Text tips_text)
    {
        // csvファイルの0行目はヒントではないから、1 ~ 行数+画像個数 としている
        int r = Random.Range(1,csvData.Count + TipsImage.Length);
        
        if(r < csvData.Count) {
            string text;
            text = csvData[r][0];
            tips_text.SetText(text);
        } else {
            tips_text.SetText("");
            Instantiate (TipsImage[Random.Range(0, TipsImage.Length)], TipsParent);
        }
    
    }
}