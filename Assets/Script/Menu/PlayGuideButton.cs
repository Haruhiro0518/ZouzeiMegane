using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuideButton : MonoBehaviour
{
    [SerializeField] GameObject PlayGuidePrefab;
    private GameObject instance;

    public void DisplayPlayGuide()
    {
        if(instance == null) {
            // buttonの親(キャンバス)上にインスタンス化. Canvasの真ん中は(360,640)
            instance = Instantiate(PlayGuidePrefab, new Vector3(Screen.width/2.0f, Screen.height/2.0f, 0f), Quaternion.identity, this.transform.parent);
        } else {
            instance.SetActive(true);
        }
        SettingManager.instance.SelectClose();
    }
}
