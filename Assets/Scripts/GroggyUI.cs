using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroggyUI : MonoBehaviour
{
    Transform target;
    RectTransform rt;
    Text text;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 pos = Camera.main.WorldToScreenPoint(target.position);
        //rt.anchoredPosition = pos;
    }
}
