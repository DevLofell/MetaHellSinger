using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActBlock : MonoBehaviour
{
    public int songBPM;
    public AudioClip ChangeClipx1;
    public AudioClip ChangeClipx2;
    public AudioClip ChangeClipx3;
    public AudioClip ChangeClipx5;

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

    }
}
