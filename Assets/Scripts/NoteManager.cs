using System.Collections;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    private static NoteManager _instance;
    public static NoteManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    public AudioSource audioSource;
    public RectTransform parent;
    public Vector2 StartPos;
    public Vector2 EndPos;
    public GameObject prefab;
    public int bpm = 120;

    private float interval;
    private Player player;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        interval = 60f / bpm;
        player = FindObjectOfType<Player>();
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
        StartCoroutine(SpawnNotes());
    }

    private IEnumerator SpawnNotes()
    {
        audioSource.Play();
        while (true)
        {
            if ((audioSource.time % interval) >= 0)
            {
                GameObject go = Instantiate(prefab, parent);
                go.GetComponent<RectTransform>().anchoredPosition = StartPos;
                NoteData note = go.GetComponent<NoteData>();
                if (note != null)
                {
                    note.Initialize(StartPos, EndPos, interval * 3);
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