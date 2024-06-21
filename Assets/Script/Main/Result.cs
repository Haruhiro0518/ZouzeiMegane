using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] WaveGenerate waveGenerate;
    [SerializeField] private TMPro.TMP_Text FinalState;

    public void SetFinalState()
    {
        if(waveGenerate.IsGameOver == true) {
            FinalState.SetText("<size=55>＜解散＞</size>");
        } 
        else {
            FinalState.SetText("<size=55>＜任期満了！＞</size>");
        }
    }
}
