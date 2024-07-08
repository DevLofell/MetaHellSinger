using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform trDamage;
    public RectTransform rtDamageUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos =Camera.main.WorldToScreenPoint(trDamage.position);
        rtDamageUI.anchoredPosition = pos;
    }
}
