using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    private Image toastImage;
    private float fadeOutDuration = 1f;
    private float moveUpDuration = 0.5f; // Duração da animação de subir (ajuste conforme necessário)
    private float moveUpAmount = 150f; // Quantidade que o toast subirá no eixo Y
    private float scaleDownDuration = 1f;
    private float minScale = 0.01f;
    private float startDelay = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        toastImage = GetComponent<Image>();
        StartCoroutine(AnimateToastSequence());
    }

    public void SetMessage(string message)
    {
        // Procura pelo objeto filho chamado "ToastMessage"
        Transform toastMessageTransform = transform.Find("ToastMessage");
        if (toastMessageTransform != null)
        {
            // Tenta obter o componente TextMeshPro
            TextMeshProUGUI toastMessage = toastMessageTransform.GetComponent<TextMeshProUGUI>();
            if (toastMessage != null)
            {
                toastMessage.text = message; // Define a mensagem
            }
            else
            {
                Debug.LogError("O componente TextMeshPro não foi encontrado no objeto 'ToastMessage'.");
            }
        }
        else
        {
            Debug.LogError("O objeto filho 'ToastMessage' não foi encontrado no prefab do Toast.");
        }
    }

    IEnumerator AnimateToastSequence()
    {
        // Inicia a animação de subir e espera que ela termine
        yield return StartCoroutine(MoveUpAnimation());

        // Espera o delay antes de iniciar o desaparecimento e a diminuição
        yield return new WaitForSeconds(startDelay);

        // Inicia a animação de desaparecer e diminuir
        yield return StartCoroutine(FadeOutAndScaleDownAnimation());

        // Destrói o objeto após a animação completa
        Destroy(gameObject);
    }

    IEnumerator MoveUpAnimation()
    {
        float timer = 0f;
        Vector3 initialPosition = transform.localPosition;

        while (timer < moveUpDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / moveUpDuration);
            float yOffset = Mathf.Lerp(0f, moveUpAmount, progress);
            transform.localPosition = initialPosition + new Vector3(0f, yOffset, 0f);
            yield return null;
        }

        // Garante que a posição final da subida seja atingida
        transform.localPosition = initialPosition + new Vector3(0f, moveUpAmount, 0f);
    }

    IEnumerator FadeOutAndScaleDownAnimation()
    {
        float timer = 0f;
        Vector3 initialScale = transform.localScale;
        float initialAlpha = (toastImage != null) ? toastImage.color.a : 1f;

        while (timer < Mathf.Max(fadeOutDuration, scaleDownDuration) && transform.localScale.x > minScale)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / Mathf.Max(fadeOutDuration, scaleDownDuration));

            // Interpolação para a escala
            float scaleFactor = Mathf.Lerp(1f, 0f, progress);
            transform.localScale = initialScale * scaleFactor;

            // Interpolação para o alpha
            if (toastImage != null)
            {
                Color currentColor = toastImage.color;
                currentColor.a = Mathf.Lerp(initialAlpha, 0f, progress);
                toastImage.color = currentColor;
            }

            yield return null;
        }

        // Garante que a escala final seja zero e o alpha seja zero
        transform.localScale = Vector3.zero;
        if (toastImage != null)
        {
            Color finalColor = toastImage.color;
            finalColor.a = 0f;
            toastImage.color = finalColor;
        }
    }
}