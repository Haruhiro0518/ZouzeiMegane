using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// TaxAreaオブジェクトにアタッチするクラス
// Playerと衝突した場合、税率を税率変化率によって増減させる
public class TaxArea : MonoBehaviour
{
    public enum TaxChangeType { Increase, Decrease }

    [Header("設定")]
    [SerializeField] private TaxChangeType type; // 増税か減税か
    [SerializeField] private int hpBonusOnDecrease = 25; // 減税時のHPボーナス
    [SerializeField] private float speedOffsetOnIgnore = 0.2f; // 無視時の速度オフセット増加量

    [Header("参照コンポーネント")]
    [SerializeField] private ValueData data;
    private ManageHPUI manageHPUI;
    private ManageImgUI manageImgUI;
    private Player player;
    private WaveGenerate waveGenerate;
    private TaxRateText taxRateText;
    private AudioManager audioManager;
    private SpeedBar speedBar;

    private float changeTaxRate;
    private bool cantChange = false; //増減できない状態か (cantIncrease/cantDecreaseを統合)

    void Start()
    {
        // GameManagerから必要なものを取得する
        player = GameManager.Instance.Player;
        taxRateText = GameManager.Instance.TaxRateTextUI;
        waveGenerate = GameManager.Instance.WaveGenerate;
        speedBar = GameManager.Instance.SpeedBar;
        audioManager = GameManager.Instance.AudioManager;

        manageHPUI = gameObject.GetComponent<ManageHPUI>();
        manageImgUI = gameObject.GetComponent<ManageImgUI>();

        if (player != null)
        {
            changeTaxRate = SelectChangeRate(player.taxRate);
            ChangeTaxAreaText();
        }

    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag != "Player") return;

        if (type == TaxChangeType.Increase)
        {
            // 増税エリアの衝突処理
            if (player.IsInvincible && player.taxRate >= 1.5f) {    // 無敵で増税不可なら次のウェーブが無敵になる
                waveGenerate.NextBlockWaveSetGlasses();
            } else {                        // 増税可能ならApplyTaxChange
                ApplyTaxChange();
            }
        }
        else // Decrease
        {
            // 減税エリアの衝突処理
            if (player.IsInvincible) {
                IgnoreDecreaseTaxRate();
            } else {
                ApplyTaxChange();
            }
        }
    }

    float SelectChangeRate(float p_rate)
    {
        switch (type)
        {
            case TaxChangeType.Increase:
                if (p_rate == 0f) return 1.5f;
                if (p_rate == 0.5f) return 1.0f;
                if (p_rate == 1.0f) return 0.5f;
                cantChange = true;
                return 0f;

            case TaxChangeType.Decrease:
                if (p_rate == 0f) { cantChange = true; return 0f; }
                if (p_rate == 0.5f || p_rate == 1.0f) return -0.5f;
                return -1.0f;

            default:
                return 0f;
        }
    }

    void ApplyTaxChange()
    {
        // 税率変更とそれに伴うパラメータ更新
        player.taxRate += changeTaxRate;
        if (player.taxRate < 0f) player.taxRate = 0f;

        // data.ChangeItemHPminmax(player.taxRate);
        // data.ChangeBlockHPDistribution(player.taxRate);
		data.UpdateParamsByTaxRate(player.taxRate, player.IsInvincible);
        player.PlayerSpeed = player.SelectPlayerSpeed();
        player.Move();

        // UIエフェクト
        if (cantChange)
        {
            if (type == TaxChangeType.Increase) taxRateText.VibrateScaleUp();
            else taxRateText.VibrateScaleDown();
        }

        // タイプ別の追加処理
        if (type == TaxChangeType.Increase && data.currentGameMode == ValueData.GameMode.Endless)
        {
            player.HalveHP(); // 無敵モードなら増税でHPを半分にする
        }
        else if (type == TaxChangeType.Decrease)
        {
            waveGenerate.AccelerateNextTaxArea(15);
            player.AddHP(hpBonusOnDecrease);
        }
    }

    // 減税を検討(ignore)する。検討すると、PlayerSpeedOffsetが増え、総理が加速する
    void IgnoreDecreaseTaxRate()
    {
        audioManager.Play_ignoreTaxArea();
        manageHPUI.ChangeText("<size=120>検討</size>");

        if (player.PlayerSpeedOffset < 1.0f)
        {
            player.PlayerSpeedOffset += speedOffsetOnIgnore;
            SpeedBarUpdate();
        }
        else
        {
            player.PlayerSpeedOffset = 1.0f;
        }

        player.PlayerSpeed = player.SelectPlayerSpeed();
        player.Move();
        StartCoroutine(AnimateTaxArea());
    }

    public void ChangeTaxAreaText()
    {
        string label = (type == TaxChangeType.Increase) ? "増税" : "減税";
        if (player.IsInvincible)
        {
            manageHPUI.ChangeText(label + "\n\n\n");
            manageImgUI.image.enabled = true;
        }
        else
        {
            manageHPUI.ChangeText(label + "\n<size=100>" + (changeTaxRate * 100f).ToString() + "</size>%");
            manageImgUI.image.enabled = false;
        }
    }

    private float whichside, move_x, move_y, x, y;
    // PlayerがTaxAreaを突き飛ばすアニメーション
    IEnumerator AnimateTaxArea()
    {
        x = y = 0;
        if (gameObject.transform.position.x > 0f) { // 右側
            whichside = 1f;
        } else {                                   // 左側
            whichside = -1f;
        }

        Vector3 offset = new Vector3(0f, 0f, 0f);

        while (true)
        {
            // -3まで下に落ちたら終了
            if (move_y < -3f) {
                break;
            }
            // それぞれの増加率は試して上手くいった数値
            x += 0.005f;
            y += 0.05f;
            move_x = (0.3f - (1f - Mathf.Pow(1f - x, 3f))) * whichside;    // 0.3 - (1-(1-x)^3)
            move_y = 0.4f - (Mathf.Pow(y, 3f));     // 0.4 - (y*y*y)
            gameObject.transform.position += new Vector3(move_x, move_y, 0f);

            gameObject.transform.eulerAngles = new Vector3(0f, 0f, move_x * 200f);

            yield return new WaitForSeconds(0.02f);
        }
        yield break;
    }

    // スピードバー表示更新. speedUptextの表示
    void SpeedBarUpdate()
    {
        speedBar = GameObject.Find("SpeedBar").GetComponent<SpeedBar>();
        speedBar.SetValue(player.PlayerSpeedOffset);
        speedBar.SpeedUpTextFX();
    }

}