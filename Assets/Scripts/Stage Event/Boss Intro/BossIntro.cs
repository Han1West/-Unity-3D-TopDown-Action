using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BossIntro : SkippableEvent
{
    [Header("Texts")]
    [SerializeField] TMP_Text[] texts;

    [Header("Texts Move")]
    [SerializeField] float moveDistance = 1000f;
    [SerializeField] float effectDelay = 0.3f;
    [SerializeField] float groupDelay = 1.5f;    
    [SerializeField] float textDuration = 0.3f;

    [Header("Images")]
    [SerializeField] UnityEngine.UI.Image bossImages;
    [SerializeField] Sprite[] sprites;
    [SerializeField] RectTransform imageRect;

    [Header("Images Move")]
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;
    [SerializeField] float imageDuration = 2f;

    [Header("SFX")]
    [SerializeField] AudioClip growlingSFX;
 
    Vector2[] originPos;

    private void Start()
    {
        originPos = new Vector2[texts.Length];

        for (int i = 0; i < texts.Length; i++)
        {
            RectTransform rect = texts[i].rectTransform;

            originPos[i] = rect.anchoredPosition;

            if (i % 2 == 0)
                rect.anchoredPosition += Vector2.left * moveDistance;
            else
                rect.anchoredPosition += Vector2.right * moveDistance;
        }

        StartCoroutine(PlayCutSceneSequence());        
    }

    IEnumerator PlayCutSceneSequence()
    {
        Coroutine textCoroutine = StartCoroutine(PlayTextEffect());
        Coroutine imageCoroutine = StartCoroutine(PlayCutScene());

        yield return textCoroutine;
        yield return imageCoroutine;

        yield return new WaitForSeconds(1f);

        EventManager.Instance.LoadNextScene();
    }

    IEnumerator PlayTextEffect()
    {
        yield return StartCoroutine(MoveText(0));
        yield return new WaitForSeconds(effectDelay);
        yield return StartCoroutine(MoveText(1));

        yield return new WaitForSeconds(groupDelay);

        yield return StartCoroutine(MoveText(2));
        yield return new WaitForSeconds(effectDelay);
        yield return StartCoroutine(MoveText(3));
    }

    IEnumerator MoveText(int index)
    {
        RectTransform rect = texts[index].rectTransform;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = originPos[index];

        float time = 0f;

        while(time < textDuration)
        {
            time += Time.deltaTime;

            float t = time / textDuration;

            t = 1f - Mathf.Pow(1f - t, 3f);

            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            yield return null;
        }

        AudioManager.Instance.PlayTextSlam();
        rect.anchoredPosition = endPos;
    }
    IEnumerator PlayCutScene()
    {
        float time = 0f;

        AudioManager.Instance.PlaySFX(growlingSFX, 0.5f);

        while(time < imageDuration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / imageDuration);

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            imageRect.anchoredPosition = Vector2.Lerp(startPos, endPos, smoothT);

            int index = Mathf.FloorToInt(smoothT * (sprites.Length - 1));
            bossImages.sprite = sprites[index];

            yield return null;
        }

        imageRect.anchoredPosition = endPos;
        bossImages.sprite = sprites[sprites.Length - 1];   
    }

    public override void SkipEvent()
    {
        // ´ÙÀ½ ¾À ·Îµå
        EventManager.Instance.LoadNextScene();
    }
}
