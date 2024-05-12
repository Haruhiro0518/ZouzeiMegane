using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTaxArea : MonoBehaviour
{
    [SerializeField] private GameObject _taxArea;
    private GameObject _leftTaxArea;
    private GameObject _rightTaxArea;
    [System.NonSerialized] public string left, right;

    void Start()
    {
        // TaxAreaを右と左に配置する。どちらが増税か減税かはランダムで決定。
        if(Random.Range(0,2) == 0) {
            _leftTaxArea = InstantiateTaxArea(-1f, "increase");
            _rightTaxArea = InstantiateTaxArea(1f, "decrease");
        } else {
            _leftTaxArea = InstantiateTaxArea(-1f, "decrease");
            _rightTaxArea = InstantiateTaxArea(1f, "increase");
        }
    }

    GameObject InstantiateTaxArea(float sign, string str)
    {
        GameObject area = Instantiate(_taxArea, gameObject.transform);
        area.transform.localPosition = new Vector3(1.4f*sign, 0f, 0f);
        area.GetComponent<TaxArea>().de_or_increase = str;

        return area;
    }

    
}
