using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 1. GameManager自身をシングルトンにする
    public static GameManager Instance { get; private set; }

    // 2. 他の主要なコンポーネントへの参照をすべてここに集める
    [Header("プレイヤー関連")]
    [SerializeField] private Player player;
    public Player Player => player; // 外部からは読み取り専用で公開

    [Header("UI関連")]
    [SerializeField] private TaxRateText taxRateTextUI;
    public TaxRateText TaxRateTextUI => taxRateTextUI;

    [Header("マネージャー関連")]
    [SerializeField] private AudioManager audioManager;
    public AudioManager AudioManager => audioManager;

    [SerializeField] private WaveGenerate waveGenerate;
    public WaveGenerate WaveGenerate => waveGenerate;

    [SerializeField] private SpeedBar speedBar;
    public SpeedBar SpeedBar => speedBar; 

    void Awake()
    {
        // シングルトンの定型文
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
