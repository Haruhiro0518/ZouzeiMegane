using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScoreFX : MonoBehaviour
{
    private Animator hp_animator, score_animator;
    [SerializeField] GameObject hpText;
    [SerializeField] ManageHPUI manageHPUI, manageSCOREUI;
    public float AnimationLength;

    void Start()
    {
        hp_animator = manageHPUI.uiObject.GetComponent<Animator>();
        score_animator = manageSCOREUI.uiObject.GetComponent<Animator>();
    }
    
    // 無敵状態でブロックを壊した時のアニメーション再生
    public void InvincibleDestory(float sum)
    {
        manageHPUI.ChangeText("+"+(int)sum);
        hp_animator.SetBool("break_block", true);
    }

    

    // ブロックにヒットしたときのアニメーション
    public void HitBlockScore(float score)
    {
        manageSCOREUI.ChangeText("+"+(int)score);
        score_animator.SetTrigger("play");
    }

    public void HitBlockScoreLong(float score) 
    {
        manageSCOREUI.ChangeText("+"+(int)score);
        score_animator.SetTrigger("play_long");
    }

}
