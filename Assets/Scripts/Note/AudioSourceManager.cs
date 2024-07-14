using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    
    public int nowMultiply
    {
        get
        {
            return NoteManager.instance.nowmultiply;
        }
    }

    public AudioSource audioSourcex1;
    public AudioSource audioSourcex2;
    public AudioSource audioSourcex3;
    public AudioSource audioSourcex5;
}
