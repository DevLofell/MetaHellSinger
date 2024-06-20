using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImageMovementTest : MonoBehaviour
{
    //이미지를 부모 이미지의 오른쪽에서 왼쪽까지 무한으로 이동시키고 싶다.
    //1. 부모 이미지를 들고 온다.
    public RectTransform parentRectTransform;
    //2. 자식 이미지를 들고온다.
    public RectTransform targetRectTransform;
    //2. 부모 이미지의 시작 지점을 들고 온다.
    Vector2 startPos;
    //3. 부모 이미지의 끝 지점을 들고 온다.
    Vector2 endPos;
    //4. 대상 이미지의 사이즈 정보를 들고와서 오차를 빼서 지정한다.
    Vector2 childOffset;

    float nowValue = 1;
    // Start is called before the first frame update
    void Start()
    {
        //5. 대상 이미지의 시작 지점에서 끝 지점까지 이동시킨다.
        if (parentRectTransform == null) return;
        if (targetRectTransform == null) return;

        childOffset = targetRectTransform.sizeDelta/2;
        startPos = new Vector2(parentRectTransform.rect.xMax - childOffset.x, (parentRectTransform.rect.yMax + parentRectTransform.rect.yMin)/2);
        endPos = new Vector2(parentRectTransform.rect.xMin + childOffset.x, ((parentRectTransform.rect.yMax + parentRectTransform.rect.yMin)/2));

        StartCoroutine(Looping());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator Looping()
    {

        while (true)
        {
            nowValue = 0;
            while (nowValue < 1)
            {
                nowValue += Time.deltaTime; // 프레임마다 nowValue 증가
                targetRectTransform.localPosition = Vector2.Lerp(startPos, endPos, nowValue);
                yield return null; // 다음 프레임까지 대기
            }

            // 이동이 끝나면 nowValue를 다시 0으로 리셋하여 무한 이동
            nowValue = 0;
        }


    }

}
