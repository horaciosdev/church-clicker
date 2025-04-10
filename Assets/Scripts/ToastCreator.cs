using UnityEngine;

public static class ToastCreator
{
    public static void CreateToast(string message)
    {
        // Carrega o prefab do Toast
        GameObject toastPrefab = Resources.Load<GameObject>("ToastPrefab");
        if (toastPrefab != null)
        {
            // Encontra o Canvas na cena
            Canvas canvas = Object.FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Nenhum Canvas encontrado na cena. Certifique-se de que há um Canvas ativo.");
                return;
            }

            // Instancia o Toast como filho do Canvas
            GameObject toastInstance = Object.Instantiate(toastPrefab, canvas.transform);
            //toastInstance.transform.localPosition = Vector3.zero; // Ajusta a posição inicial, se necessário

            // Configura a mensagem no Toast
            Toast toast = toastInstance.GetComponent<Toast>();
            if (toast != null)
            {
                toast.SetMessage(message);
            }
            else
            {
                Debug.LogError("O componente Toast não foi encontrado no prefab.");
            }
        }
        else
        {
            Debug.LogError("Toast prefab not found in Resources folder.");
        }
    }
}