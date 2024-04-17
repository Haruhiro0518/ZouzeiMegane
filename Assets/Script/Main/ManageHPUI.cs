using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManageHPUI : MonoBehaviour
{
    
    private RectTransform _uiParentObjectTransform;
    [SerializeField] private GameObject _uiObjectPrefab;
    [System.NonSerialized] public GameObject uiObject;
    private FollowTransform followTransform;
    private TMPro.TMP_Text TextHP;
    
    // UIをもつオブジェクトがStartメソッドでChangeTextメソッドを呼ぶため、
    // それよりも早くAwakeメソッドでTMProコンポーネントを取得する
    void Awake()
    {
        _uiParentObjectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        uiObject = Instantiate(_uiObjectPrefab, _uiParentObjectTransform);
        TextHP = uiObject.GetComponent<TMPro.TMP_Text>();
        followTransform = uiObject.GetComponent<FollowTransform>();
        followTransform.Initialize(gameObject.transform);

    }

    void Start()
    {
        
    }

    public void ChangeText(string text)
    {   
        TextHP.SetText(text);
    }

    public void DestroyText() 
    {
        Destroy(uiObject);
    }

    public void ChangeWorldOffset(Vector3 v)
    {
        followTransform._worldOffset = v;
    }

    void Update()
    {
        
    }
}
