using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ActBlock : MonoBehaviour
{
    public int songBPM;
    public AudioClip ChangeClipx1;
    public AudioClip ChangeClipx2;
    public AudioClip ChangeClipx3;
    public AudioClip ChangeClipx5;

    public bool isBossTrigger = false;

    public IEnumerator Start()
    {
        yield return new WaitUntil(() => (NoteManager.instance.prePlayClipList != null));
        NoteManager.instance.prePlayClipList.Add(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered");
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            NoteManager.instance.AudioChange(this);
        }
        if(isBossTrigger)
        {
            BossPatternManager.instance.isBossStart = true;
        }
    }
}
