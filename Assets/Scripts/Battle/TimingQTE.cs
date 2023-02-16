using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class TimingQTE : MonoBehaviour
{
    [SerializeField] GameObject badBar;
    [SerializeField] GameObject goodBar;
    [SerializeField] GameObject greatBar;
    [SerializeField] GameObject key;

    private float value;

    private void OnEnable()
    {
        //Initialize();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        value = 0f;

        badBar.transform.localScale = Vector3.one;
        goodBar.transform.localScale = Vector3.one;
        greatBar.transform.localScale = Vector3.one;

        badBar.transform.position = transform.position;
        goodBar.transform.position = transform.position;
        greatBar.transform.position = transform.position;
        key.transform.position = transform.position;

        SetUp(0.4f, 0.1f);
    }

    public void SetUp(float goodRatio, float greatRatio)
    {
        goodBar.GetComponent<RectTransform>().localScale = new Vector3(goodRatio, 1f);
        greatBar.GetComponent<RectTransform>().localScale = new Vector3(greatRatio, 1f);

        value = UnityEngine.Random.Range(0 + goodBar.GetComponent<RectTransform>().localScale.x / 2, 1 - goodBar.GetComponent<RectTransform>().localScale.x / 2);
        Debug.Log(value);

        goodBar.GetComponent<RectTransform>().localPosition = new Vector3((value * badBar.GetComponent<RectTransform>().rect.width) - (badBar.GetComponent<RectTransform>().rect.width / 2), 0);
        greatBar.GetComponent<RectTransform>().localPosition = new Vector3((value * badBar.GetComponent<RectTransform>().rect.width) - (badBar.GetComponent<RectTransform>().rect.width / 2), 0);
        key.GetComponent<RectTransform>().localPosition = new Vector3((value * badBar.GetComponent<RectTransform>().rect.width) - (badBar.GetComponent<RectTransform>().rect.width / 2), 0);
    }
}
