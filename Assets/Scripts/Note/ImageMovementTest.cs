using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImageMovementTest : MonoBehaviour
{
    //�̹����� �θ� �̹����� �����ʿ��� ���ʱ��� �������� �̵���Ű�� �ʹ�.
    //1. �θ� �̹����� ��� �´�.
    public RectTransform parentRectTransform;
    //2. �ڽ� �̹����� ���´�.
    public RectTransform targetRectTransform;
    //2. �θ� �̹����� ���� ������ ��� �´�.
    Vector2 startPos;
    //3. �θ� �̹����� �� ������ ��� �´�.
    Vector2 endPos;
    //4. ��� �̹����� ������ ������ ���ͼ� ������ ���� �����Ѵ�.
    Vector2 childOffset;

    float nowValue = 1;
    // Start is called before the first frame update
    void Start()
    {
        //5. ��� �̹����� ���� �������� �� �������� �̵���Ų��.
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
                nowValue += Time.deltaTime; // �����Ӹ��� nowValue ����
                targetRectTransform.localPosition = Vector2.Lerp(startPos, endPos, nowValue);
                yield return null; // ���� �����ӱ��� ���
            }

            // �̵��� ������ nowValue�� �ٽ� 0���� �����Ͽ� ���� �̵�
            nowValue = 0;
        }


    }

}
