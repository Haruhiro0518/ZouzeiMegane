using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// PlayerやBlock, ItemなどのHPUIを持つオブジェクトにアタッチするクラス

public class ManageHPUI : MonoBehaviour
{
    
    private RectTransform _uiParentObjectTransform;
    [SerializeField] private GameObject _uiObjectPrefab;
    [System.NonSerialized] public GameObject uiObject;
    private FollowTransform followTransform;
    private TMPro.TMP_Text TextHP;
    
    // UIをもつオブジェクトがStartメソッドでthis.ChangeTextメソッドを呼ぶ時、
    // TMProコンポーネントの取得ができていないとエラーになってしまう。
    // そのため、AwakeメソッドでTMProコンポーネントを取得する。
    void Awake()
    {
        _uiParentObjectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        uiObject = Instantiate(_uiObjectPrefab, _uiParentObjectTransform);
        TextHP = uiObject.GetComponent<TMPro.TMP_Text>();
        followTransform = uiObject.GetComponent<FollowTransform>();
        followTransform.Initialize(gameObject.transform);

    }

    public void ChangeText(string text)
    {   
        TextHP.SetText(text);
    }

    public void DestroyText() 
    {
        if(uiObject != null) {
            Destroy(uiObject);
        }
    }

    public void ChangeWorldOffset(Vector3 v)
    {
        followTransform._worldOffset = v;
    }

    public void DisableTextComponent()
    {
        TextHP.enabled = false;
    }
}
