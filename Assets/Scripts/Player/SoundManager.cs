using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum ESoundType
    {
        EFT_FIRE,
        EFT_SWORD,
        EFT_THSWORD
    }

    //나를 담을 static 변수
    public static SoundManager instance;

    //audioSource
    public AudioSource eftAudio;

    //effect audioclip을 여러개 담아 놓을 변수
    public AudioClip[] eftAudios;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //씬이 전환이 돼도 게임오브젝트를 바괴하고 싶지 않다
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayEftSound(ESoundType idx)
    {
        int audioIdx = (int)idx;
        eftAudio.PlayOneShot(eftAudios[audioIdx]);
    }


}
