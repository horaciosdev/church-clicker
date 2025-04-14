using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Toast : MonoBehaviour
{
    public float displayDuration = 2f;
    public float moveDistance = 100f;
    public float animationDuration = 0.4f;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 initialPosition;
    private bool moveUp;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        // Decide se o toast deve subir ou descer com base na posição vertical
        moveUp = rectTransform.anchorMin.y < 0.5f;
        initialPosition = rectTransform.anchoredPosition;

        // Começa visível
        canvasGroup.alpha = 1f;
        transform.localScale = Vector3.one;

        StartCoroutine(AnimateToastSequence());
    }

    public void SetMessage(string message, float duration)
    {
        displayDuration = duration;

        var messageObj = transform.Find("ToastMessage");
        if (messageObj != null && messageObj.TryGetComponent(out TextMeshProUGUI text))
        {
            text.text = message;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI não encontrado no filho 'ToastMessage'");
        }
    }

    IEnumerator AnimateToastSequence()
    {
        // Move e exibe
        yield return StartCoroutine(MoveAndFade(visible: true));

        // Espera
        yield return new WaitForSeconds(displayDuration);

        // Move e desaparece
        yield return StartCoroutine(MoveAndFade(visible: false));

        Destroy(gameObject);
    }

    IEnumerator MoveAndFade(bool visible)
    {
        float timer = 0f;
        float startAlpha = visible ? 0f : 1f;
        float endAlpha = visible ? 1f : 0f;

        Vector3 startPos = initialPosition + (moveUp ? Vector3.down : Vector3.up) * (visible ? moveDistance : 0f);
        Vector3 endPos = initialPosition + (moveUp ? Vector3.up : Vector3.down) * (visible ? 0f : moveDistance);

        Vector3 startScale = visible ? Vector3.zero : Vector3.one;
        Vector3 endScale = visible ? Vector3.one : Vector3.zero;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / animationDuration);

            rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, t);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            yield return null;
        }

        rectTransform.anchoredPosition = endPos;
        transform.localScale = endScale;
        canvasGroup.alpha = endAlpha;
    }
}
