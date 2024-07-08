using System.Collections;
using UnityEngine;

public class NoteData : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private float duration;
    private float spawnTime;

    public delegate void NoteHitHandler(string hitType);
    public static event NoteHitHandler OnNoteHit;

    public void Initialize(Vector2 start, Vector2 end, float time)
    {
        startPos = start;
        endPos = end;
        duration = time;
        spawnTime = Time.time;
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
                    Debug.Log("Great");
                    OnNoteHit?.Invoke("Great");
                    Destroy(gameObject);
                }
                else if (timingRatio >= 0.8f && timingRatio < 0.9f)
                {
                    Debug.Log("Good");
                    OnNoteHit?.Invoke("Good");
                    Destroy(gameObject);
                }
            }

            float t = (Time.time - spawnTime) / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        Destroy(gameObject);
    }
}