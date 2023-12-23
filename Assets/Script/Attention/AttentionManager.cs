using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AttentionManager : MonoBehaviour
{
    [SerializeField, Header("アテンションUI")]
    private GameObject AttentionUI;

    public void SelectOK()
    {
        SceneManager.LoadScene("Title");
    }
}
