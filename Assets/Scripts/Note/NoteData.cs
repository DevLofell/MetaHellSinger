using System.Collections;
using UnityEngine;

public class NoteData : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private float duration;
    private float spawnTime;
    public bool isMainNote = false;

    public delegate void NoteHitHandler(string hitType);
    public static event NoteHitHandler OnNoteHit;

    public void Initialize(Vector2 start, Vector2 end, float time, bool isMain)
    {
        startPos = start;
        endPos = end;
        duration = time;
        spawnTime = Time.time;
        isMainNote = isMain;
        StartCoroutine(MoveNote());
    }

    private IEnumerator MoveNote()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        while (Time.time - spawnTime < duration)
        {
            if (Input.GetMouseButtonDown(0))
            {
                float elapsed = Time.time - spawnTime;
                float timingRatio = elapsed / duration;

                if (timingRatio >= 0.75f && timingRatio < 0.8f)
                {
                    //Debug.Log("Great");
                    if(isMainNote) OnNoteHit?.Invoke("Great");
                    
                    Destroy(gameObject);
                }
                else if (timingRatio >= 0.8f && timingRatio < 0.9f)
                {
                    //Debug.Log("Good");
                    if (isMainNote)
                        OnNoteHit?.Invoke("Good");
                    Destroy(gameObject);
                }
                else
                {
                    if (isMainNote)
                        OnNoteHit?.Invoke("Bad");
                }
            }

            float t = (Time.time - spawnTime) / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        Destroy(gameObject);
    }
}