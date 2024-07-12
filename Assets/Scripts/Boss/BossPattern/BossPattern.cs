using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    public Transform BossRig;
    public bool isPatternOver = false;

    public virtual void OnEnable()
    {
        isPatternOver = false;
    }

    public virtual void OnDisable()
    {

    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
