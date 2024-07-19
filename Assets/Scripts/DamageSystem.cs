using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageSystem : MonoBehaviour
{
    Transform target;
    RectTransform rt;
    Text text;
    Vector3 up;

    public float uiSpeed = 30;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        text = GetComponent<Text>();        
    }

    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(target.position);
        up += Vector3.up * (uiSpeed * Time.deltaTime);
        rt.anchoredPosition = pos + up;

        float angle = Vector3.Angle(Camera.main.transform.forward, target.position - Camera.main.transform.position);
        if (angle < 60) text.enabled = true;
        else text.enabled = false;
    }
    public void DamageMove(Transform tr)
    {
        target = tr;
    }
}
