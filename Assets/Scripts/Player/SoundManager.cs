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

    //���� ���� static ����
    public static SoundManager instance;

    //audioSource
    public AudioSource eftAudio;

    //effect audioclip�� ������ ��� ���� ����
    public AudioClip[] eftAudios;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //���� ��ȯ�� �ŵ� ���ӿ�����Ʈ�� �ٱ��ϰ� ���� �ʴ�
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
