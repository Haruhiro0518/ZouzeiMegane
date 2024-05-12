using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// MenueMangerにアタッチ

public class CSVreader : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile; 
    private List<string[]> csvData = new List<string[]>(); 

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

    public void SetupText(TMPro.TMP_Text tmpro)
    {
        string text;
        text = csvData[Random.Range(1,csvData.Count)][0];
        tmpro.SetText(text);
        
    }
}