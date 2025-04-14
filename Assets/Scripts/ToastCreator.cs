using UnityEngine;

public static class ToastCreator
{
    public static void CreateToast(string message, string positionString = "bottom-left", float duration = 2.0f)
    {
        // Carrega o prefab do Toast (deve estar na pasta Resources)
        GameObject toastPrefab = Resources.Load<GameObject>("ToastPrefab");
        if (toastPrefab == null)
        {
            Debug.LogError("Prefab 'ToastPrefab' não encontrado na pasta Resources!");
            return;
        }

        // Encontra o Canvas na cena (assume que há apenas um)
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Nenhum Canvas encontrado na cena!");
            return;
        }

        // Instancia o Toast como filho do Canvas
        GameObject toastInstance = Object.Instantiate(toastPrefab, canvas.transform);
        RectTransform toastRect = toastInstance.GetComponent<RectTransform>();

        // Configura o tamanho e posição inicial
        toastRect.anchoredPosition = Vector2.zero;
        toastRect.sizeDelta = new Vector2(200, 50); // Tamanho padrão (ajuste conforme necessário)

        // Define a posição baseada no parâmetro
        SetToastPosition(toastRect, positionString);

        // Configura a mensagem e duração
        Toast toastScript = toastInstance.GetComponent<Toast>();
        if (toastScript != null)
        {
            toastScript.SetMessage(message, duration);
        }
        else
        {
            Debug.LogError("O GameObject do Toast não tem o script 'Toast' anexado!");
        }
    }

    private static void SetToastPosition(RectTransform toastRect, string position)
    {
        // Configura anchors e pivots para posicionamento responsivo
        switch (position.ToLower())
        {
            case "top":
                toastRect.anchorMin = new Vector2(0.5f, 1f);
                toastRect.anchorMax = new Vector2(0.5f, 1f);
                toastRect.anchoredPosition = new Vector2(0f, -50f); // 50 pixels abaixo do topo
                break;

            case "bottom":
                toastRect.anchorMin = new Vector2(0.5f, 0f);
                toastRect.anchorMax = new Vector2(0.5f, 0f);
                toastRect.anchoredPosition = new Vector2(0f, 50f); // 50 pixels acima do fundo
                break;

            case "left":
                toastRect.anchorMin = new Vector2(0f, 0.5f);
                toastRect.anchorMax = new Vector2(0f, 0.5f);
                toastRect.anchoredPosition = new Vector2(100f, 0f); // 100 pixels à direita da borda esquerda
                break;

            case "right":
                toastRect.anchorMin = new Vector2(1f, 0.5f);
                toastRect.anchorMax = new Vector2(1f, 0.5f);
                toastRect.anchoredPosition = new Vector2(-100f, 0f); // 100 pixels à esquerda da borda direita
                break;

            case "top-left":
                toastRect.anchorMin = new Vector2(0f, 1f);
                toastRect.anchorMax = new Vector2(0f, 1f);
                toastRect.anchoredPosition = new Vector2(100f, -50f);
                break;

            case "top-right":
                toastRect.anchorMin = new Vector2(1f, 1f);
                toastRect.anchorMax = new Vector2(1f, 1f);
                toastRect.anchoredPosition = new Vector2(-100f, -50f);
                break;

            case "bottom-left":
                toastRect.anchorMin = new Vector2(0f, 0f);
                toastRect.anchorMax = new Vector2(0f, 0f);
                toastRect.anchoredPosition = new Vector2(100f, 50f);
                break;

            case "bottom-right":
                toastRect.anchorMin = new Vector2(1f, 0f);
                toastRect.anchorMax = new Vector2(1f, 0f);
                toastRect.anchoredPosition = new Vector2(-100f, 50f);
                break;

            case "center":
            default:
                toastRect.anchorMin = new Vector2(0.5f, 0.5f);
                toastRect.anchorMax = new Vector2(0.5f, 0.5f);
                toastRect.anchoredPosition = Vector2.zero;
                break;
        }
    }
}