using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// PlayerのtaxRateを百分率に直して表示するクラス
public class TaxRateText : MonoBehaviour
{
    private TMPro.TMP_Text taxRateText;
    private Player player;
    private int display_taxRate;
    private bool IsChanging = false;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource TaxRateTextAudio;
    [SerializeField] private AudioClip se_TaxRateUp;
    [SerializeField] private AudioClip se_TaxRateDown;

    void Start()
    {
        taxRateText = gameObject.GetComponent<TMPro.TMP_Text>();
        player = GameObject.Find("Player").GetComponent<Player>();
        display_taxRate = (int)(player.taxRate*100);
        taxRateText.SetText("<size=50>"+display_taxRate.ToString()+"</size>%");
    }

    void Update()
    {
        if(display_taxRate != (int)(player.taxRate*100)) {
            if(IsChanging == false) {
                IsChanging = true;
                ChangeText();
            }
        }
    }

    public void ChangeText()
    {
        StartCoroutine(ChangeGradually());
    }

    IEnumerator ChangeGradually()
    { 
        
        int target = (int)(player.taxRate*100);
        
        if(display_taxRate < target) {
            animator.SetTrigger("scaleup");
            TaxRateTextAudio.PlayOneShot(se_TaxRateUp);
        } else if(display_taxRate > target) {
            animator.SetTrigger("scaledown");
            TaxRateTextAudio.PlayOneShot(se_TaxRateDown);
        }

        while(true)
        {
            if(display_taxRate == target) {
                break;
            } else if(display_taxRate < target) {
                display_taxRate++;
            } else {
                display_taxRate--;
            }
            taxRateText.SetText("<size=50>"+display_taxRate.ToString()+"</size>%");
            yield return new WaitForSeconds(0.009f);
        }

        IsChanging = false;
        yield break;
    }

    public void VibrateScaleUp()
    {
        animator.SetTrigger("scaleup_vibrate");
    }

    public void VibrateScaleDown()
    {
        animator.SetTrigger("scaledown_vibrate");
    }
}
