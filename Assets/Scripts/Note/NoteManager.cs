using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoSingleton<NoteManager>
{
    public AudioSource audioSource;
    public Coroutine nowCoroutine;
    public AudioClip startClip;
    public AudioClip nowx1Clip;
    public AudioClip nowx2Clip;
    public AudioClip nowx3Clip;
    public AudioClip nowx5Clip;
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
    public float interval;
    private Player player;
    public Text comboText;
    public int comboint = 0;
    private float nextNoteTime;

    // Add an offset to adjust the note spawning time
    public float offset = 0f;

    private void Awake()
    {
        interval = 60f / bpm;
        player = FindObjectOfType<Player>();
    }

    public void AudioChange(ActBlock block) => nowCoroutine = StartCoroutine(AudioChangeSync(block));
    private IEnumerator AudioChangeSync(ActBlock block)
    {
        float nowtime = audioSource.time;
        interval = 60f / block.songBPM;
        audioSource.Pause();
        AudioClip tempClip;
        switch (nowmultiply)
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
        audioSource.clip = tempClip;
        audioSource.time = nowtime;
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
        nextNoteTime = audioSource.time + interval + (interval/3);
        yield return StartCoroutine(SpawnNotes());
    }

    private IEnumerator SpawnNotes()
    {
        audioSource.Play();
        while (true)
        {
            if (audioSource.time >= nextNoteTime)
            {
                SpawnNote();
                nextNoteTime += interval;
            }
            yield return null; // Wait for the next frame
        }
    }

    private void SpawnNote()
    {
        GameObject lgo = Instantiate(notePrefab, leftNoteparent);
        GameObject rgo = Instantiate(notePrefab, rightNoteparent);
        lgo.GetComponent<RectTransform>().anchoredPosition = leftStartPos;
        rgo.GetComponent<RectTransform>().anchoredPosition = rightStartPos;
        lgo.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 180);
        NoteData lnote = lgo.GetComponent<NoteData>();
        NoteData rnote = rgo.GetComponent<NoteData>();
        if (lnote != null && rnote != null)
        {
            lnote.Initialize(leftStartPos, leftEndPos, interval * 3, false);
            rnote.Initialize(rightStartPos, rightEndPos, interval * 3, true);
        }
    }

    private void HandleNoteHit(string hitType)
    {
        if (hitType == "Great" || hitType == "Good")
        {
            player.Fire();
            //comboText.gameObject.SetActive(true);
            //comboint++;
            //comboText.text = comboint.ToString();
        }
        else
        {
            //comboint = 0;
        }
    }
}
