using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class NoteManager : MonoSingleton<NoteManager>
{
    public AudioSource audioSource;

    public Coroutine nowCoroutine;

    public AudioClip startClip;
    public ActBlock nowActBlock;
    public List<ActBlock> prePlayClipList;

    public RectTransform leftNoteparent;
    public RectTransform rightNoteparent;
    public Vector2 rightStartPos;
    public Vector2 rightEndPos; 
    public Vector2 leftStartPos;
    public Vector2 leftEndPos;

    public GameObject notePrefab;
    
    public int bpm = 120;

    public int nowmultiply = 1;


    private float interval;
    private Player player;


    private void Awake()
    {
        
        interval = 60f / bpm;
        player = FindObjectOfType<Player>();
    }


    
    public void AudioChange(ActBlock block) => nowCoroutine = StartCoroutine(AudioChangeSync(block));
    private IEnumerator AudioChangeSync(ActBlock block)
    {
        interval = 60f / block.songBPM;
        audioSource.Pause();
        AudioClip tempClip;
        switch(nowmultiply)
        {
            case 1:
                tempClip = block.ChangeClipx1;
                break;
            case 2:
                tempClip = block.ChangeClipx2;
                break;
            case 3:
                tempClip = block.ChangeClipx3;
                break;
            case 5:
                tempClip = block.ChangeClipx5;
                break;
            default:
                tempClip = null;
                break;

        }
        audioSource.resource = tempClip;
        audioSource.Play();
        yield return new WaitUntil(() => (audioSource.isPlaying));
    }
   

    private void OnEnable()
    {
        NoteData.OnNoteHit += HandleNoteHit;
    }

    private void OnDisable()
    {
        NoteData.OnNoteHit -= HandleNoteHit;
    }

    private IEnumerator Start()
    {
        prePlayClipList = new List<ActBlock>();
        
        yield return new WaitUntil(() => (prePlayClipList.Count > 0));

        
        
    }

    //ToDo : 왼쪽 노트 시작 위치/ 왼쪽 노트 끝 위치/ 우측 노트 시작 위치/ 우측 노트 끝 위치
    //계산하는 함수 만들어서 대입 하기

    private IEnumerator SpawnNotes()
    {
        audioSource.Play();
        while (true)
        {
            if ((audioSource.time % interval) >= 0)
            {
                GameObject lgo = Instantiate(notePrefab, leftNoteparent);
                GameObject rgo = Instantiate(notePrefab, rightNoteparent);
                lgo.GetComponent<RectTransform>().anchoredPosition = leftStartPos;
                rgo.GetComponent<RectTransform>().anchoredPosition = rightStartPos;
                rgo.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 180);
                NoteData lnote = lgo.GetComponent<NoteData>();
                NoteData rnote = rgo.GetComponent<NoteData>();
                if (lnote != null && rnote != null)
                {
                    lnote.Initialize(leftStartPos, leftEndPos, interval * 3);
                     rnote.Initialize(rightStartPos, rightEndPos, interval * 3);
                }
            }
            
            yield return new WaitForSeconds(interval);
        }
    }

    private void HandleNoteHit(string hitType)
    {
        if (hitType == "Great" || hitType == "Good")
        {
            player.Fire();
        }
    }
}