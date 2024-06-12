using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleBGM : MonoBehaviour
{
    [SerializeField]
    private AudioSource playerAudio;
    private AudioSource main_bgm;
    [SerializeField]
    private Player player;

    void Start()
    {
        main_bgm = GameObject.Find("MainManager").GetComponent<AudioManager>().main_bgm;
        // SettingManager側からplayerのAudioSource.volumeを変更するため、参照を渡す
        SettingManager.instance.mainSource.Add(playerAudio);
    }

    void Update()
    {
        if(player.IsInvincible == true) {
            InvincibleBGMLoopControl(); 
        }
    }

    public void Play()
    {
        playerAudio.Play();
        main_bgm.Stop();
    }

    public void Stop()
    {
        playerAudio.Stop();
        main_bgm.Play();
    }

    void InvincibleBGMLoopControl()
    {
        int LoopEndSamples = 302088;
        int LoopLengthSamples = 278856;

        if(playerAudio.timeSamples >= LoopEndSamples)
        {
            playerAudio.timeSamples -= LoopLengthSamples;
        }
    }
}
