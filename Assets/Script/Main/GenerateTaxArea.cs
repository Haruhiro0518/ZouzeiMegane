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
            _leftTaxArea = Instantiate(_taxArea, gameObject.transform);
            _leftTaxArea.transform.localPosition = new Vector3(-1.4f, 0f, 0f);
            _leftTaxArea.GetComponent<TaxArea>().de_or_increase = "increase";
            _rightTaxArea = Instantiate(_taxArea, gameObject.transform);
            _rightTaxArea.transform.localPosition = new Vector3(1.4f, 0f, 0f);
            _rightTaxArea.GetComponent<TaxArea>().de_or_increase = "decrease";
        } else {
            _leftTaxArea = Instantiate(_taxArea, gameObject.transform);
            _leftTaxArea.transform.localPosition = new Vector3(-1.4f, 0f, 0f);
            _leftTaxArea.GetComponent<TaxArea>().de_or_increase = "decrease";
            _rightTaxArea = Instantiate(_taxArea, gameObject.transform);
            _rightTaxArea.transform.localPosition = new Vector3(1.4f, 0f, 0f);
            _rightTaxArea.GetComponent<TaxArea>().de_or_increase = "increase";
        }
        
        
    }

    void Update()
    {
        
    }
}
