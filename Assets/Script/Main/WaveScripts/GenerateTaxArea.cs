using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave-Taxにアタッチするコンポーネント.
// Wave-TaxはWaveGenerate.GeneratedWaveListに1つしか存在しない
public class GenerateTaxArea : MonoBehaviour
{
    [SerializeField] private GameObject taxArea_increase, taxArea_decrease;
    private GameObject ta_increase, ta_decrease;

    void Start()
    {
        // TaxAreaを右と左に配置する。どちらが増税か減税かはランダムで決定。
        if(Random.Range(0,2) == 0) {
            ta_increase = InstantiateTaxArea(-1f, taxArea_increase);
            ta_decrease = InstantiateTaxArea(1f, taxArea_decrease);
        } else {
            ta_decrease = InstantiateTaxArea(-1f, taxArea_decrease);
            ta_increase = InstantiateTaxArea(1f, taxArea_increase);
        }
        gameObject.name = "Wave-Tax";
    }

    GameObject InstantiateTaxArea(float sign, GameObject prefab)
    {
        GameObject area = Instantiate(prefab, gameObject.transform);
        area.transform.localPosition = new Vector3(1.4f*sign, 0f, 0f);
        return area;
    }

    public void ChangeTaxAreaText()
    {
        ta_increase.GetComponent<TaxArea>().ChangeTaxAreaText();
        ta_decrease.GetComponent<TaxArea>().ChangeTaxAreaText();
    }
}