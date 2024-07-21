using System.Collections;
using System.Collections.Generic;
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

    // RawImage fields for left and right
    public RawImage greatEffectImageLeft;
    public RawImage greatEffectImageRight;

    public float offset;

    private void Awake()
    {
        interval = 60f / bpm;
        player = FindAnyObjectByType<Player>();
    }

    public void AudioChange(ActBlock block)
    {
        if (nowCoroutine != null)
        {
            StopCoroutine(nowCoroutine);
        }
        nowCoroutine = StartCoroutine(AudioChangeSync(block));
    }

    private IEnumerator AudioChangeSync(ActBlock block)
    {
        float nowtime = audioSource.time;
        interval = 60f / block.songBPM;
        offset = block.offset; // Set the offset for the current song
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
        nextNoteTime = audioSource.time + interval + offset;
    }

    private void OnEnable()
    {
        NoteData.OnNoteHit += HandleNoteHit;
    }

    private void OnDisable()
    {
        NoteData.OnNoteHit -= HandleNoteHit;
    }

    private void Start()
    {
        prePlayClipList = new List<ActBlock>();
        StartCoroutine(WaitForPrePlayClipList());
    }

    private IEnumerator WaitForPrePlayClipList()
    {
        yield return new WaitUntil(() => (prePlayClipList.Count > 0));
        nextNoteTime = audioSource.time + interval + offset;
        StartCoroutine(SpawnNotes());
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
            if (hitType == "Great")
            {
                StartCoroutine(FlashGreatEffect());
            }
            //comboText.gameObject.SetActive(true);
            //comboint++;
            //comboText.text = comboint.ToString();
        }
        else
        {
            //comboint = 0;
        }
    }

    private IEnumerator FlashGreatEffect()
    {
        Color colorLeft = greatEffectImageLeft.color;
        Color colorRight = greatEffectImageRight.color;
        float elapsedTime = 0f;
        float duration = 0.3f; // Duration of the flash effect

        // Define a scale factor for zoom effect
        Vector3 originalScaleLeft = greatEffectImageLeft.transform.localScale;
        Vector3 originalScaleRight = greatEffectImageRight.transform.localScale;
        Vector3 zoomScale = new Vector3(1.5f, 1.5f, 1f); // Zoom in effect

        // Define a maximum alpha value for the flash effect
        float maxAlpha = 0.5f; // Adjust this value to control the maximum intensity of the effect

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.PingPong(elapsedTime * 3, maxAlpha); // Increase flashing speed
            colorLeft.a = alpha;
            colorRight.a = alpha;

            // Apply scale effect
            greatEffectImageLeft.transform.localScale = Vector3.Lerp(originalScaleLeft, zoomScale, elapsedTime / duration);
            greatEffectImageRight.transform.localScale = Vector3.Lerp(originalScaleRight, zoomScale, elapsedTime / duration);

            greatEffectImageLeft.color = colorLeft;
            greatEffectImageRight.color = colorRight;

            yield return null;
        }

        // Ensure the alpha value is set to 0 after the effect
        colorLeft.a = 0;
        colorRight.a = 0;
        greatEffectImageLeft.color = colorLeft;
        greatEffectImageRight.color = colorRight;

        // Reset scale to original
        greatEffectImageLeft.transform.localScale = originalScaleLeft;
        greatEffectImageRight.transform.localScale = originalScaleRight;
    }

}
