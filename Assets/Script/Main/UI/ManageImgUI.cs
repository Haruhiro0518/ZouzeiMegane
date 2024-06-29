using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Unity.VisualStudio.Editor;

// TaxAreaのようにImageコンポーネントを持つオブジェクトにアタッチするクラス
// ManageHPUIとほぼ同じ
public class ManageImgUI : MonoBehaviour
{
    
    private RectTransform _uiParentObjectTransform;
    [SerializeField] private GameObject _uiObjectPrefab;
    [System.NonSerialized] public GameObject uiObject;
    [System.NonSerialized] public UnityEngine.UI.Image image;
    private FollowTransform followTransform;
    
    // UIをもつオブジェクトがStartメソッドでthis.ChangeTextメソッドを呼ぶ時、
    // TMProコンポーネントの取得ができていないとエラーになってしまう。
    // そのため、AwakeメソッドでTMProコンポーネントを取得する。
    void Awake()
    {
        _uiParentObjectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        uiObject = Instantiate(_uiObjectPrefab, _uiParentObjectTransform);
        image = uiObject.GetComponent<UnityEngine.UI.Image>();
        followTransform = uiObject.GetComponent<FollowTransform>();
        followTransform.Initialize(gameObject.transform);

    }

    public void DestroyImage() 
    {
        if(uiObject != null) {
            Destroy(uiObject);
        }
    }

    public void ChangeWorldOffset(Vector3 v)
    {
        followTransform._worldOffset = v;
    }

}
